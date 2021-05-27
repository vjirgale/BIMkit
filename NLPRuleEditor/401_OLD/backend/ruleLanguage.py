import types, json
from enum import Enum

class Occurrence(Enum):
    ANY = "ANY"
    ALL = "ALL"

    def __str__(self):
        return str(self.value)

class Ruleset:
    def __init__(self, id, name, description, rules):
        self.id = id
        self.name = name
        self.description = description
        self.rules = rules

    def toJson(self):
        rulesetDict = {
            'Rules': [json.loads(rule.toJson()) for rule in self.rules],
            'Name': self.name,
            "Description": self.description
        }
        return json.dumps(rulesetDict)
    

class LogicalExpression:
    def __init__(
        self,
        objectChecks = [],
        relationChecks = [],
        logicalExpressions = [],
        logicalOperator = "AND"
    ):
        self.objectChecks = objectChecks
        self.relationChecks = relationChecks
        self.logicalExpressions = logicalExpressions
        if logicalOperator == 0:
            self.logicalOperator = 'AND'
        elif logicalOperator == 1:
            self.logicalOperator = 'OR'
        else:
            self.logicalOperator = "XOR"

    def toString(self):
        # Method that returns a string version of the logical expression in DRL terms
        # Can be multiple elements, all separated by one operator
        output = '('
                       
        for objectCheck in self.objectChecks:
            output += objectCheck.toString() + ' ' + self.logicalOperator + ' '
        for relationCheck in self.relationChecks:
            output += relationCheck.toString() + ' ' + self.logicalOperator + ' '
        for logicalExpression in self.logicalExpressions:
            output += logicalExpression.toString() + ' ' + self.logicalOperator + ' '
        
        return output[:(len(self.logicalOperator) + 2)*-1] + ')'
        
    
    def toEnglish(self):
        output = ""

        for objectCheck in self.objectChecks:
            output += objectCheck.toEnglish() + " " + self.logicalOperator.lower() + " "
        for relationCheck in self.relationChecks:
            output += relationCheck.toEnglish() + " " + self.logicalOperator.lower() + " "
        for logicalExpression in self.logicalExpressions:
            output += logicalExpression.toEnglish() + " " + self.logicalOperator.lower() + " "

        return output[:(len(self.logicalOperator) + 2)*-1]
    
    def toJson(self):
        logDict = {
            "ObjectChecks": [json.loads(objectCheck.toJson()) for objectCheck in self.objectChecks],
            "RelationChecks": [json.loads(relationCheck.toJson()) for relationCheck in self.relationChecks],
            "LogicalExpressions": [json.loads(logicalExpression.toJson()) for logicalExpression in self.logicalExpressions],
            "LogicalOperator": self.logicalOperator
        }
        return json.dumps(logDict)
    

class Rule:
    def __init__(
        self,
        id = "",
        title = "",
        description = "",
        existentialClauses = [],
        logicalExpression = LogicalExpression(),
        errorLevel = 0
    ):
        self.id = id
        self.title = title
        self.description = description
        self.existentialClauses = existentialClauses
        self.logicalExpression = logicalExpression
        self.errorLevel = errorLevel

    def toString(self):
        # Function for taking the different elements of a rule, and outputting them as a printable string.
        output = ''
        # First step: Existential Clauses
        for x in self.existentialClauses:
            output += self.existentialClauses[x].toString(x)

        # Next: Logical Expressions
        output += self.logicalExpression.toString()
        return output

    def toEnglish(self):
        output = 'For '

        existentialClauseCount = len(self.existentialClauses)
        i = 0

        for x in self.existentialClauses:
            i += 1
            output += self.existentialClauses[x].toEnglish()
            if i < existentialClauseCount:
                output += " and "

        output += ', '
        output += self.logicalExpression.toEnglish() + "."

        return output

    def toJson(self):
        ruleDict =  {
            "Name": self.title,
            "Description": self.description,
            "ExistentialClauses": {},
            "LogicalExpression": json.loads(self.logicalExpression.toJson()),
            "ErrorLevel": self.errorLevel
        }
        if ruleDict["ErrorLevel"] == "0":
            ruleDict["ErrorLevel"]= "Recommended"
        elif ruleDict["ErrorLevel"] == "1":
            ruleDict["ErrorLevel"] = "Warning"
        elif ruleDict["ErrorLevel"] == "2":
            ruleDict["ErrorLevel"] = "Error"
        for exClause in self.existentialClauses:
            ruleDict["ExistentialClauses"][exClause] = json.loads(self.existentialClauses[exClause].toJson())
        return json.dumps(ruleDict)

class ObjectCheck:
    def __init__(self, objName, negation, propertyCheck, objectType):
        self.objName = objName
        self.negation = negation
        self.propertyCheck = propertyCheck
        self.objectType = objectType

    def toString(self):
        # Returns string representation in DRL terms
        # As of now, no examples with negation -- leaving out of output for the moment
        output = str(self.objName) + ' ' + self.propertyCheck.toString()
        return output

    def toEnglish(self):
        return self.objectType + ' objects must have ' + self.propertyCheck.toEnglish()

    def toJson(self):
        if self.negation == 0:
            self.negation = "MUST_HAVE"
        elif self.negation == 1:
            self.negation = "MUST_NOT_HAVE"
        objDict = {
            "ObjName": self.objName,
            "Negation": self.negation,
            "PropertyCheck": json.loads(self.propertyCheck.toJson())
        }
        return json.dumps(objDict)

class RelationCheck:
    def __init__(self, obj1Name, obj2Name, negation, propertyCheck, objectType1, objectType2):
        self.obj1Name = obj1Name
        self.obj2Name = obj2Name
        self.negation = negation
        self.propertyCheck = propertyCheck
        self.objectType1 = objectType1
        self.objectType2 = objectType2

    def toString(self):
        # Returns string representation in DRL terms
        # Again, no negation in examples given to us
        output = str(self.obj1Name) + ' and ' + str(self.obj2Name) + ' ' + self.propertyCheck.toString()
        return output
    
    def toEnglish(self):
        return self.objectType1 + " and " + self.objectType2 + " must have the \"" + self.propertyCheck.toEnglish() + "\" relation"

    def toJson(self):
        if self.negation == 0:
            self.negation = "MUST_HAVE"
        elif self.negation == 1:
            self.negation = "MUST_NOT_HAVE"
        relDict = {
            "Obj1Name": self.obj1Name,
            "Obj2Name": self.obj2Name,
            "Negation": self.negation,
            "PropertyCheck": json.loads(self.propertyCheck.toJson())
        }
        return json.dumps(relDict)

class ExistentialClause:
    def __init__(self, occurrenceRule, characteristic):
        self.occurrenceRule = occurrenceRule
        self.characteristic = characteristic
        
    def toString(self, objName):
        # Returns string representation in DRL terms
        # Requires the name of the object
        output = str(self.occurrenceRule).upper() + ' ' + str(objName) + ' = ' + str(self.characteristic.type)
        # Add any property requirements
        if self.characteristic.propertyChecks:
            output += '{'
            for propertyCheck in self.characteristic.propertyChecks:
                output += propertyCheck.toString()
                output += ' AND '
            output = output[:-5] # Strip out unnecessary AND
            output += '}'
        output += '\n'
        return output

    def toEnglish(self):
        if self.occurrenceRule == Occurrence.ALL:
            output = 'all ' + str(self.characteristic.type) + ' objects'
        else:
            output = 'any ' + str(self.characteristic.type) + ' object'
        return output

    def toJson(self):
        exDict = {
            "OccurrenceRule": str(self.occurrenceRule),
            "Characteristic": json.loads(self.characteristic.toJson())
        }
        return json.dumps(exDict)



class Characteristic:
    def __init__(self, type, originalText, propertyChecks = []):
        self.type = type
        self.originalText = originalText
        self.propertyChecks = propertyChecks
    
    def __eq__(self, other):
        if isinstance(other, self.__class__):
            if self.type == other.type and self.propertyChecks == other.propertyChecks:
                return True
            return False
    def toJson(self):
        charDict = {
            "Type": self.type,
            "PropertyChecks": [json.loads(propertyCheck.toJson()) for propertyCheck in self.propertyChecks]
        }
        return json.dumps(charDict)

class PropertyCheck:
    def __init__(self, propertyName):
        self.propertyName = propertyName


class PropertyCheckDouble(PropertyCheck):
    def __init__(self, propertyName, operationDouble, value, unit):
        PropertyCheck.__init__(self, propertyName)
        self.operationDouble = operationDouble # Can be { >, >=, ==, <=, <, != }
        self.value = value
        self.unit = unit
        self.operationDoubleOutput = {
            '>': 'GREATER-THAN',
            '>=': 'GREATER-THAN-OR-EQUAL',
            '==': "EQUAL",
            '<=': 'LESS-THAN-OR-EQUAL',
            '<': 'LESS-THAN',
            '!=': 'NOT-EQUAL'
        }
        self.operationDoubleEnglish = {
            '>': 'is greater than',
            '>=': 'is greater than or equal to',
            '==': "equals",
            '<=': 'is less than or equal to',
            '<': 'is less than',
            '!=': 'does not equal'
        }
    
    def toString(self):
        # Return string representation
        output = self.propertyName + ' ' + self.operationDoubleOutput[self.operationDouble] + ' ' + str(self.value) + str(self.unit).upper()
        return output 
    
    def toEnglish(self):
        return self.propertyName + " that " + self.operationDoubleEnglish[self.operationDouble] + " " + str(self.value) + " " + str(self.unit).lower()

    def toJson(self):
        propDict = {
            "Operation": self.operationDoubleOutput[self.operationDouble],
            "Value": self.value,
            "ValueUnit": self.unit,
            "Name": self.propertyName,
            "PCType": "NUM"
        }
        return json.dumps(propDict)

class PropertyCheckString(PropertyCheck):
    def __init__(self, propertyName, operationString, value):
        PropertyCheck.__init__(self, propertyName)
        self.operationString = operationString
        self.value = value
        self.operationStringOutput = {
            '==': 'EQUAL',
            '!=': 'NOT-EQUAL',
            'CONTAINS': 'CONTAINS'
        }
        self.operationStringEnglish = {
            '==': 'is equal to',
            '!=': 'is not equal to',
            'CONTAINS': 'contains'
        }

    def toString(self):
        output = self.propertyName + ' ' + self.operationStringOutput[self.operationString] + ' ' + self.value
        return output

    def toEnglish(self):
        return self.propertyName + " that " + self.operationStringEnglish[self.operationString]  + self.value

    def toJson(self):
        propDict = {
            "Operation": self.operationStringOutput[self.operationString],
            "Value": self.value,
            "Name": self.propertyName,
            "PCType": "STRING"
        }
        return json.dumps(propDict)

class PropertyCheckBool(PropertyCheck):
    def __init__(self, propertyName, operationBool, value):
        PropertyCheck.__init__(self, propertyName)
        self.operationBool = operationBool
        self.value = value
        self.operationBoolOutput = {
            '==': 'EQUAL',
            '!=': 'NOT-EQUAL'
        }
        self.operationBoolEnglish = {
            '==': "equal to",
            "!=": "not equal to",
        }

    def toString(self):
        output = self.propertyName + ' ' + self.operationBoolOutput[self.operationBool] + ' ' + str(self.value)
        return output

    def toEnglish(self):
        return self.propertyName + " that is " + self.operationBoolEnglish[self.operationBool] + " " + str(self.value)

    def toJson(self):
        propDict = {
            "Operation": self.operationBoolOutput[self.operationBool],
            "Value": self.value,
            "Name": self.propertyName,
            "PCType": "BOOL"
        }
        return json.dumps(propDict)
