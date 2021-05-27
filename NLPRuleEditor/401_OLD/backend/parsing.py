#python.exe -m spacy download en_core_web_lg
#python.exe -m spacy download en_core_web_sm
#python.exe -m spacy download en_core_web_md - this is the model we are using
import spacy
import ruleLanguage
import re
import os
import json
import string
from fuzzywuzzy import process
from fuzzywuzzy import fuzz
from spacy import displacy

class ParsedRule:

    def __init__(self, englishRule, parsedComponents = [], rule = ruleLanguage.Rule()):
        self.englishRule = englishRule
        self.parsedComponents = parsedComponents
        self.rule = rule


class Parser:

    def __init__(self):
        #we are using the medium trained model for better accuracy.
        self.nlp = spacy.load("en_core_web_md")
        
        #using the provided files, we create lists including all types, relations and properties.
        self.objectTypes = self.getComponentsFromFile("RuleLanguage/ObjectTypes.txt")
        self.properties = self.getComponentsFromFile("RuleLanguage/Properties.txt")
        self.relations = self.getComponentsFromFile("RuleLanguage/Relations.txt")

        #when looking for keywords to match with the provided files, we must split the word. Ex: for relation IsAbove, in natural english we are looking for is above
        self.singleWordComponents = {}
        self.multiWordComponents = {}
        for x in self.objectTypes:
            if sum(1 for c in x if c.isupper()) < 2:
                self.singleWordComponents[x] = "Type"
            else:
                self.multiWordComponents[re.sub(r"(\w)([A-Z])", r"\1 \2", x)] = (x, "Type")
        for x in self.properties:
            if sum(1 for c in x if c.isupper()) < 2:
                self.singleWordComponents[x] = "Property"
            else:
                self.multiWordComponents[re.sub(r"(\w)([A-Z])", r"\1 \2", x)] = (x, "Property")
        for x in self.relations:
            if sum(1 for c in x if c.isupper()) < 2:
                self.singleWordComponents[x] = "Relation"
            else:
                self.multiWordComponents[re.sub(r"(\w)([A-Z])", r"\1 \2", x)] = (x, "Relation")

    #main method that calls the other methods to create a rule
    def englishToDRL(self, englishRule, customobjects=[]):
        #using the model, spacy will make predictions based on the sentence provided from frontend. 
        doc = self.nlp(englishRule)
        parsedComponents = self.parseComponents(englishRule, customobjects)
        existentialClauses = self.parseExistentialClauses(parsedComponents)
        logicalExpressions = self.parseLogicalExpressions(parsedComponents, englishRule, existentialClauses)
        rule = ruleLanguage.Rule(0,"Rule1",englishRule,existentialClauses, logicalExpressions, 0)
        return ParsedRule(englishRule, parsedComponents, rule)

    #overall, this method sorts the different words provided from the frontend into 5 types: Type, Property, Relation, Unit, and Unknown.
    def parseComponents(self, englishRule, customobjects=[]):
        doc = self.nlp(englishRule.translate(str.maketrans('', '', string.punctuation)))
        unparsedWords = englishRule.translate(str.maketrans('', '', string.punctuation)).split()
        components = []

        #this for loop will only detect quantaties and single word keywords 
        for token in doc: # match all single word components
            if (token.ent_type_ == "QUANTITY" or token.like_num): #12 inches work, but 12in doesnt. Might need to make a pattern later to detect that.
                components.append(ParsedComponent(token=token, matchedComponentName=token.text, category="Unit", supported=True))
                unparsedWords.remove(token.text)
            else:
                #try to find the closest match using the token.text and the token.lemma to ensure we dont miss out on a close match
                bestCandidateMatch = process.extractOne(token.text, self.singleWordComponents.keys(), scorer=fuzz.ratio)
                bestCandidateMatchLemma = process.extractOne(token.lemma_, self.singleWordComponents.keys(), scorer=fuzz.ratio)
                #if the fuzz ration is greater than 90, we will include that in our parsedcomponents
                if bestCandidateMatch[1] > 90:
                    components.append(ParsedComponent(token=token, matchedComponentName=bestCandidateMatch[0], category=self.singleWordComponents[bestCandidateMatch[0]], supported=True))
                    unparsedWords.remove(token.text)
                elif bestCandidateMatchLemma[1] > 90:
                    components.append(ParsedComponent(token=token, matchedComponentName=bestCandidateMatchLemma[0], category=self.singleWordComponents[bestCandidateMatchLemma[0]], supported=True))
                    unparsedWords.remove(token.text)
        
        #this part of the method is to detect that multiword keywords like isAbove and using fuzzywuzzy to detect a close match
        multiWordCandidates = process.extract(" ".join(unparsedWords), self.multiWordComponents.keys(), scorer=fuzz.partial_ratio)
        for candidate in multiWordCandidates:
            #again, we are looking for quite a close match using fuzzy ratio
            if candidate[1] > 80:
                matchedWord = process.extractOne(candidate[0], unparsedWords, scorer=fuzz.ratio)
                for token in doc:
                    if token.text == matchedWord[0]:
                        components.append(ParsedComponent(token=token, matchedComponentName=candidate[0].replace(" ", ""), category=self.multiWordComponents[candidate[0]][1], supported=True))
                        unparsedWords.remove(token.text)
        unparsedWords = [word for word in unparsedWords if not word in self.nlp.Defaults.stop_words] #gets rid of stop words like "the" cause we really dont need that.
        for word in unparsedWords:
            #this is to detect the words with no matches and if it is a noun or verb, it'll be interprted as an unknown type or relation
            for token in doc:
                if token.text == word:
                    if ((token.pos_ == "NOUN") or (token.pos_ == "VERB")):
                        if token.text.lower() in [x.lower() for x in customobjects]: 
                            #if a list of objects is given (customobjects), that list will make it so those objects won't be detected as unknown and will actually be parsed like an object that exists in the provided file do. 
                            components.append(ParsedComponent(token=token, matchedComponentName=token.text, category="Type", supported=True))
                        else:
                            components.append(ParsedComponent(token=token, matchedComponentName=token.text, category="Unknown", supported=False))
        return(components)

    def getComponentsToJSON(self, englishRule):
        convertDict = {'parsedComponents':[json.loads(comp.toJSON()) for comp in self.parseComponents(englishRule)]}
        return json.dumps(convertDict)

    def getComponentsFromFile(self, path):
        #just a helper method to initally read from the given files and append the words to a list.
        componentList = []
        with open(os.path.join(os.getcwd(), path)) as f:
            componentList = f.read().splitlines()
        return componentList

    def parseExistentialClauses(self, parsedComponents):
        #Using the type objs from the parsed components, we can detect the existential clauses
        existentialClauses = {}
        index = 0
        for parsedComponent in parsedComponents:
            if (parsedComponent.category == "Type"):
                occurrenceRule = self.occurrenceRuleCheck(parsedComponent.token)
                keyName = "Object" + str(index)
                existentialClauses[keyName] = ruleLanguage.ExistentialClause(
                    occurrenceRule,
                    ruleLanguage.Characteristic(parsedComponent.matchedComponentName, parsedComponent.token.text)
                )
                index += 1
        return existentialClauses

    def occurrenceRuleCheck(self, token):
        #method just checks if type ends with and s, tpye has a prefix like "any", etc.
        occurrenceRule = ""
        childrenTextSet = {token.text}
        for child in token.children:
            childrenTextSet.add(child.text)
        if (childrenTextSet.intersection({"any", "a", "some", "one", "an"}) != set()):
            return ruleLanguage.Occurrence.ANY
        if (childrenTextSet.intersection({"all", "every", "each", "no"}) != set()):
            return ruleLanguage.Occurrence.ALL
        if token.is_sent_start: # Assumption that the first object is always ALL if unspecified
            return ruleLanguage.Occurrence.ALL
        for child in token.children:
            if child.is_sent_start:
                return ruleLanguage.Occurrence.ALL
        return ruleLanguage.Occurrence.ANY
    
    def parseLogicalExpressions(self, parsedComponents, englishRule, existentialClauses): #doenst do any nested logical expressions
        propertyChecks = self.parsePropertyChecks(parsedComponents, englishRule) #using the helper propertychecks method, we can build logical expressions.
        objectChecks = []
        relationChecks = []
        for propertyCheck in propertyChecks:
            if (propertyCheck.propertyName in self.properties): #object check
                objectChecks.append(self.parseObjectCheck(propertyCheck, existentialClauses, parsedComponents))
            elif (propertyCheck.propertyName in self.relations): #property check
                relationChecks.append(self.parseRelationCheck(propertyCheck, existentialClauses, parsedComponents, englishRule))

        return(ruleLanguage.LogicalExpression(objectChecks, relationChecks, [], 0))
    
    def parseObjectCheck(self, propertyCheck, existentialClauses, parsedComponents):
        #looks for the closest type that is to the left of the property which is also in the exisentialclause.
        tokenSpan = None
        for parsedComponent in parsedComponents:
            if (parsedComponent.matchedComponentName == propertyCheck.propertyName):
                tokenHead = parsedComponent.token.head
                tokenCurrent = tokenHead
                while tokenCurrent.dep_ != 'ROOT':
                    tokenCurrent = tokenCurrent.head
                tokenSpan = [x.text for x in tokenCurrent.children]
        for x in existentialClauses:
            if existentialClauses[x].characteristic.originalText in tokenSpan:
                return ruleLanguage.ObjectCheck(x, 0, propertyCheck, existentialClauses[x].characteristic.type)

    def parseRelationCheck(self, propertyCheck, existentialClauses, parsedComponents, englishRule):
        englishRuleWithoutPunctuation = englishRule.translate(str.maketrans('', '', string.punctuation))
        tokenSpanLeft = None
        tokenSpanRight = None
        tokenLeft = None
        tokenRight = None
        leftObjectType = None
        rightObjectType = None

        for parsedComponent in parsedComponents:
            #using the helper propertychecks method, we find the word and split the sentence - left side and right side and relation in the middle. 
            if (parsedComponent.matchedComponentName == propertyCheck.propertyName):
                indexOfComponent = englishRuleWithoutPunctuation.split().index(parsedComponent.token.text)
                tokenSpanLeft = [x for x in englishRuleWithoutPunctuation.split()[0:indexOfComponent]]
                tokenSpanRight = [x for x in englishRuleWithoutPunctuation.split()[indexOfComponent:]]
        for x in existentialClauses:
            #using info from exisentialclause, we can find the type left and right of the relation.
            if existentialClauses[x].characteristic.originalText in tokenSpanLeft:
                tokenLeft = x
                leftObjectType = existentialClauses[x].characteristic.type
            if existentialClauses[x].characteristic.originalText in tokenSpanRight:
                tokenRight = x
                rightObjectType = existentialClauses[x].characteristic.type
        return ruleLanguage.RelationCheck(tokenLeft, tokenRight, 0, propertyCheck, leftObjectType, rightObjectType)
            
    def parsePropertyChecks(self, parsedComponents, englishRule):
        doc = self.nlp(englishRule)
        # Given a list of parsedComponents, retrieves the elements of all PropertyChecks, initializes the objects, and returns them for use
        propertyChecks = []
        # Pull out our Types, Properties, Relations, and Units
        types = [component for component in parsedComponents if component.category == "Type"]
        properties = [component for component in parsedComponents if component.category == "Property"]
        relations = [component for component in parsedComponents if component.category == "Relation"]
        quantities = [component for component in parsedComponents if component.category == "Unit" and component.token.text.isnumeric()] # They are labeled as units but are really quanitities

        # Start with those who are attatched to any detected quantities
        for quantity in quantities:
            # This will be a PropertyCheckDouble
            # we need to know what we will be measuring in, which is the parent of this token in the dep tree
            unit = quantity.token.head
            # We now need to find which parent this is a part of, which SHOULD be a matter of parsing up the tree until we find a Relation or Property
            propertyName = None
            propertyToken = None
            current = unit
            #this while loop wil use the dependecy tree and look for a match either a relation or a property type
            while current.head.dep_ != 'ROOT':
                current = current.head
                if current in [prop.token for prop in properties]:
                    for x in properties:
                        if x.token == current:
                            propertyName = x.matchedComponentName
                            propertyToken = current
                    break
                elif current in [rel.token for rel in relations]:
                    for x in relations:
                        if x.token == current:
                            propertyName = x.matchedComponentName
                            propertyToken = current
                    break
            # Whichever element we find, we need to find the operation that comes with this PropertyCheckDouble
            propertyCheckSpan = doc[propertyToken.i:unit.i + 1]
            operationDouble = ''
            if 'minimum' in propertyCheckSpan.text.lower() or 'no less than' in propertyCheckSpan.text.lower():
                operationDouble = '>='
            elif 'maximum' in propertyCheckSpan.text.lower() or 'no more than' in propertyCheckSpan.text.lower():
                operationDouble = '<='
            elif 'less than' in propertyCheckSpan.text.lower():
                operationDouble = '<'
            elif 'more than' in propertyCheckSpan.text.lower():
                operationDouble = '>'
            elif 'not' in propertyCheckSpan.text.lower():
                operationDouble = '!='
            else:
                operationDouble = '=='
            # We finally create an instance of the PropertyCheckDouble object, and append it to the list.
            propertyChecks.append(ruleLanguage.PropertyCheckDouble(propertyName = propertyName, operationDouble = operationDouble, value = float(quantity.token.text), unit = unit.text))
            # We also remove the properties and relations we used in this propertycheck
            properties = [component for component in properties if component.token != propertyToken]
            relations = [component for component in relations if component.token != propertyToken]

        # We have found all our PropertyCheckDoubles, which means we are left with PropertyCheckString and PropertyCheckBool
        # Iterate through the rest of our Properties and Relations, and assign them to one or the other
        # We assume they are PropertyCheckBool, as the only example we can see of PropertyCheckString is FunctionOfObj which is not currently supported
        for prop in properties:            
            if "no" in doc[prop.token.i - 1:prop.token.i + 1].text: # Checking for negation
                propertyChecks.append(ruleLanguage.PropertyCheckBool(prop.matchedComponentName, '==', False))
            else:
                propertyChecks.append(ruleLanguage.PropertyCheckBool(prop.matchedComponentName, '==', True))

        for rel in relations:
            if "no" in doc[rel.token.i - 1:rel.token.i + 1].text: # Checking for negation
                propertyChecks.append(ruleLanguage.PropertyCheckBool(rel.matchedComponentName, '==', False))
            else:
                propertyChecks.append(ruleLanguage.PropertyCheckBool(rel.matchedComponentName, '==', True))
        return propertyChecks

    def unknownSuggestions(self, parsedComponents):
        #If a word is marked as an unknown type, the frontend will be provided with a list of 3 types/relations/properties.
        #Matching is based on fuzzywuzzy so not really contextual matching. It'll look for words that are quite close.
        #Also, this will always return 3 matches, no matter if the word is close or not.
        suggestionDict = {}
        for unknown in parsedComponents:
            if unknown.category == "Unknown":
                wordlist = []
                #word suggestion for properties
                if unknown.token.pos_ == "NOUN" and unknown.token.dep_ == "dobj":
                    suggestions = process.extract(unknown.token.text, self.properties, limit=3)
                    for wordin in suggestions:
                        wordlist.append(wordin[0])
                    suggestionDict[unknown.token.text] = wordlist
                #word suggestion for types
                elif unknown.token.pos_ == "NOUN":
                    suggestions = process.extract(unknown.token.text, self.objectTypes, limit=3)
                    for wordin in suggestions:
                        wordlist.append(wordin[0])
                    suggestionDict[unknown.token.text] = wordlist
                #word suggestion for relations
                else:
                    suggestions = process.extract(unknown.token.text, self.relations, limit=3)
                    for wordin in suggestions:
                        wordlist.append(wordin[0])
                    suggestionDict[unknown.token.text] = wordlist
                    #returns a dict with the name of type/relation/property as key and a list of 3 values as the value
        return suggestionDict



class ParsedComponent:
    #holds individual information on each token minus stop words.
    def __init__(self, token, matchedComponentName, category, supported):
        self.token = token
        self.matchedComponentName = matchedComponentName
        self.category = category
        self.supported = supported

    def toJSON(self):
        componentDict = {}
        componentDict['token'] = self.token.text
        componentDict['category'] = self.category
        componentDict['supported'] = self.supported
        return json.dumps(componentDict)