import unittest
import parsing
import ruleLanguage

parser = parsing.Parser()
class TestParsingMethods(unittest.TestCase):

    def testParseComponentsDetectsObjectTypesSimple(self):
        
        components = parser.parseComponents("There is a corner and a room in this sentence.")
        self.assertEqual(components[0].token.text, "corner")
        self.assertEqual(components[0].category, "Type")
        self.assertEqual(components[1].token.text, "room")
        self.assertEqual(components[1].category, "Type")
        self.assertEqual([x.category == "Type" for x in components].count(True), 2)
    
    def testParseComponentsDetectsObjectTypesComplex(self):
        
        components = parser.parseComponents("Windows should have a width of no less than 15 inch and a height of maximum 2 feet and is above sink.")
        self.assertEqual(components[0].token.text, "Windows")
        self.assertEqual(components[0].category, "Type")
        self.assertEqual(components[-2].token.text, "sink")
        self.assertEqual(components[-2].category, "Type")
        self.assertEqual([x.category == "Type" for x in components].count(True), 2)

    def testParseComponentsDetectsPropertiesSimple(self):
        
        components = parser.parseComponents("There is depth and width in this sentence.")
        self.assertEqual(components[0].token.text, "depth")
        self.assertEqual(components[0].category, "Property")
        self.assertEqual(components[1].token.text, "width")
        self.assertEqual(components[1].category, "Property")
        self.assertEqual([x.category == "Property" for x in components].count(True), 2)
    
    def testParseComponentsDetectsPropertiesComplex(self):
        
        components = parser.parseComponents("Tables should have a width of no more than 40 inch and a height of minimum 2 feet and is above floor.")
        self.assertEqual(components[1].token.text, "width")
        self.assertEqual(components[1].category, "Property")
        self.assertEqual(components[-5].token.text, "height")
        self.assertEqual(components[-5].category, "Property")
        self.assertEqual([x.category == "Property" for x in components].count(True), 2)

    def testParseComponentsDetectsRelationsSimple(self):
        
        components = parser.parseComponents("There is above and is below in this sentence.")
        self.assertEqual(components[0].token.text, "above")
        self.assertEqual(components[0].category, "Relation")
        self.assertEqual(components[1].token.text, "below")
        self.assertEqual(components[1].category, "Relation")
        self.assertEqual([x.category == "Relation" for x in components].count(True), 2)

    def testParseComponentsDetectsRelationsComplex(self):
        #relation will always be the last element in the components list.
        
        components = parser.parseComponents("Tables should have a width of no more than 40 inch and a height of minimum 2 feet and is above floor.")
        self.assertEqual(components[-1].token.text, "above")
        self.assertEqual(components[-1].category, "Relation")
        self.assertEqual([x.category == "Relation" for x in components].count(True), 1)

    def testParseExistentialClausesCreatesOneClausePerObjectType(self):
        
        components = parser.parseComponents("There is a corner and a room and depth in this sentence.")
        existentialClauses = parser.parseExistentialClauses(components)
        self.assertEqual(len(existentialClauses), 2)

    def testParseExistentialClausesCorrectlyAssignsOccurrenceRule(self):
        
        components = parser.parseComponents("All corners must be in a room.")
        existentialClauses = parser.parseExistentialClauses(components)

        self.assertEqual(existentialClauses['Object0'].occurrenceRule, ruleLanguage.Occurrence.ALL)
        self.assertEqual(existentialClauses['Object0'].characteristic.type, "Corner")
        self.assertEqual(existentialClauses['Object1'].occurrenceRule, ruleLanguage.Occurrence.ANY)
        self.assertEqual(existentialClauses['Object1'].characteristic.type, "Room")

    def testParsePropertyChecksNoLess(self):
        
        englishRule = "Windows should have a width of no less than 15 inch and a height of maximum 2 feet and is above sink."
        components = parser.parseComponents(englishRule)
        propertyChecks = parser.parsePropertyChecks(components, englishRule)
        self.assertEqual(len(propertyChecks), 3)
        self.assertEqual(propertyChecks[0].operationDouble, '>=')
        self.assertEqual(propertyChecks[0].value, 15)
        self.assertEqual(propertyChecks[0].unit, 'inch')
        self.assertEqual(propertyChecks[2].value, True)

    def testParsePropertyChecksNoMore(self):
        
        englishRule = "Windows should have a width of no more than 15 inch and a height of maximum 2 feet and is above sink."
        components = parser.parseComponents(englishRule)
        propertyChecks = parser.parsePropertyChecks(components, englishRule)
        self.assertEqual(len(propertyChecks), 3)
        self.assertEqual(propertyChecks[0].operationDouble, '<=')
        self.assertEqual(propertyChecks[0].value, 15)
        self.assertEqual(propertyChecks[0].unit, 'inch')
        self.assertEqual(propertyChecks[2].value, True)

    def testParsePropertyChecksMore(self):
        
        englishRule = "Window should have a width of more than 15 inch and a height of maximum 2 feet and is above sink."
        components = parser.parseComponents(englishRule)
        propertyChecks = parser.parsePropertyChecks(components, englishRule)
        self.assertEqual(len(propertyChecks), 3)
        self.assertEqual(propertyChecks[0].operationDouble, '>')
        self.assertEqual(propertyChecks[0].value, 15)
        self.assertEqual(propertyChecks[0].unit, 'inch')
        self.assertEqual(propertyChecks[2].value, True)
    
    def testParsePropertyChecksLess(self):
        
        englishRule = "Window should have a width of less than 15 inch and a height of maximum 2 feet and is above sink."
        components = parser.parseComponents(englishRule)
        propertyChecks = parser.parsePropertyChecks(components, englishRule)
        self.assertEqual(len(propertyChecks), 3)
        self.assertEqual(propertyChecks[0].operationDouble, '<')
        self.assertEqual(propertyChecks[0].value, 15)
        self.assertEqual(propertyChecks[0].unit, 'inch')
        self.assertEqual(propertyChecks[2].value, True)
    
    def testParsePropertyChecksComplex(self):
        
        englishRule = "Window should have a width of more than 15 inch and a height of maximum 2 feet and a depth of minimum 2 centimeters is above sink."
        components = parser.parseComponents(englishRule)
        propertyChecks = parser.parsePropertyChecks(components, englishRule)
        self.assertEqual(len(propertyChecks), 4)
        self.assertEqual(propertyChecks[0].operationDouble, '>')
        self.assertEqual(propertyChecks[0].value, 15)
        self.assertEqual(propertyChecks[0].unit, 'inch')

        self.assertEqual(propertyChecks[1].operationDouble, '<=')
        self.assertEqual(propertyChecks[1].value, 2.0)
        self.assertEqual(propertyChecks[1].unit, 'feet')

        self.assertEqual(propertyChecks[2].operationDouble, '>=')
        self.assertEqual(propertyChecks[2].value, 2.0)
        self.assertEqual(propertyChecks[2].unit, 'centimeters')
        
        self.assertEqual(propertyChecks[3].operationBool, '==')
        self.assertEqual(propertyChecks[3].value, True)

    def testParseLogicalExpressions(self):
        
        englishRule = "All Windows should have a width of no less than 15 inches and a height of maximum 2 feet and is above sinks"
        components = parser.parseComponents(englishRule)
        existentialClauses = parser.parseExistentialClauses(components)
        logicalExpressions = parser.parseLogicalExpressions(components, englishRule, existentialClauses)
        self.assertEqual(len(logicalExpressions.objectChecks), 2)
        self.assertEqual(len(logicalExpressions.relationChecks), 1)
        self.assertEqual(logicalExpressions.relationChecks[0].propertyCheck.propertyName, "IsAbove")
        self.assertEqual(logicalExpressions.objectChecks[0].propertyCheck.propertyName, "Width")
        self.assertEqual(logicalExpressions.objectChecks[1].propertyCheck.propertyName, "Height")
        self.assertEqual(logicalExpressions.objectChecks[0].objName, 'Object0')
        self.assertEqual(logicalExpressions.objectChecks[1].objName, 'Object0')
        self.assertEqual(logicalExpressions.relationChecks[0].obj1Name, 'Object0')
        self.assertEqual(logicalExpressions.relationChecks[0].obj2Name, 'Object1')
    
    def testUnknownSuggestions(self):
        
        englishRule = "All basins should have a width of no less than 15 feet and is above carpet."
        components = parser.parseComponents(englishRule)
        keys = parser.unknownSuggestions(components)
        self.assertEqual("basins" in keys, True)
        self.assertEqual("carpet" in keys, True)
        self.assertEqual(len(keys), 2)

class TestTranslation(unittest.TestCase):
    # Tests a few full translations to ensure that they function properly.
    def setUp(self):
        self.parser = parsing.Parser()

    def testRule1(self):
        # Test rule 1: "Any television should be in front of a couch"
        ruleResultString = "ALL Object0 = Television\nANY Object1 = Couch\n(Object0 and Object1 IsInFrontOf EQUAL True)"
        generatedRule = self.parser.englishToDRL('Any television should be in front of a couch').rule.toString()
        self.assertEqual(ruleResultString, generatedRule)

    def testRule2(self):
        # Test rule 2: "All windows must have a width of 15 inches and be next to a sink"
        ruleResultString = 'ALL Object0 = Window\nANY Object1 = Sink\n(Object0 Width EQUAL 15.0INCHES AND Object0 and Object1 NextTo EQUAL True)'
        generatedRule = self.parser.englishToDRL("All windows must have a width of 15 inches and be next to a sink").rule.toString()
        self.assertEqual(ruleResultString, generatedRule)

    def testRule3(self):
        # Test rule 3: "All toilets must have a height of no more than 2 feet"
        ruleResultString = 'ALL Object0 = Toilet\n(Object0 Height LESS-THAN-OR-EQUAL 2.0FEET)'
        generatedRule = self.parser.englishToDRL("All toilets must have a height of no more than 2 feet").rule.toString()
        self.assertEqual(ruleResultString, generatedRule)

    def testReTranslation1(self):
        # Test rule 1: "Any television should be in front of a couch"
        englishResultString = 'For all Television objects and any Couch object, Television and Couch must have the "IsInFrontOf that is equal to True" relation.'
        generatedEnglish = self.parser.englishToDRL('Any television should be in front of a couch').rule.toEnglish()
        self.assertEqual(englishResultString, generatedEnglish)

    def testReTranslation2(self):
        # Test rule 2: "All windows must have a width of 15 inches and be next to a sink"
        englishResultString = 'For all Window objects and any Sink object, Window objects must have Width that equals 15.0 inches and Window and Sink must have the "NextTo that is equal to True" relation.'
        generatedEnglish = self.parser.englishToDRL("All windows must have a width of 15 inches and be next to a sink").rule.toEnglish()
        self.assertEqual(englishResultString, generatedEnglish)

    def testReTranslation3(self):
        englishResultString = 'For all Toilet objects, Toilet objects must have Height that is less than or equal to 2.0 feet.'
        generatedEnglish = self.parser.englishToDRL("All toilets must have a height of no more than 2 feet").rule.toEnglish()
        self.assertEqual(englishResultString, generatedEnglish)

if __name__ == '__main__':
    unittest.main()