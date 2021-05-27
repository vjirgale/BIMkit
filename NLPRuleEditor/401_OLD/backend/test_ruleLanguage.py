import unittest
from ruleLanguage import *

class TestLogicalExpressionMethods(unittest.TestCase):
    def setUp(self):
        propertyCheck1 = PropertyCheckString("Color", '==', 'Red')
        propertyCheck2 = PropertyCheckBool('IsNextTo', '==', True)
        propertyCheck3 = PropertyCheckBool('IsBehind', '==', False)
        objectChecks = [ObjectCheck(0, 'MUST', propertyCheck1, 'Color')]
        relationChecks = [RelationCheck(0,1,'MUST', propertyCheck2, 'Wall', 'CounterTop')]
        logicalExpressions = [
            LogicalExpression([], [RelationCheck(0,1,'MUST', propertyCheck3, 'Wall', 'CounterTop'), RelationCheck(1,0,'MUST', propertyCheck3, 'Wall', 'CounterTop')],[],'OR')
        ]
        self.logicalExpression = LogicalExpression(objectChecks, relationChecks, logicalExpressions, 'AND')

    def testToString(self):
        logStr = self.logicalExpression.toString()
        self.assertEqual(logStr.count('('), 2)
        self.assertEqual(logStr.count('and'), 3)
        self.assertEqual(logStr.count('XOR'), 3)
        self.assertTrue('0' in logStr)
        self.assertTrue('1' in logStr)
        self.assertTrue('Color' in logStr)
        self.assertTrue('IsNextTo' in logStr)
        self.assertTrue('IsBehind' in logStr)
        self.assertEqual(logStr.count(')'),2)


class TestRuleMethods(unittest.TestCase):
    def setUp(self):
        characteristic1 = Characteristic('CounterTop','', [])
        propertyCheck1 = PropertyCheckString('HasThing', '==', 'Valid')
        propertyCheck2 = PropertyCheckBool('IsWall', '!=', False)
        characteristic2 = Characteristic('Wall', '', [propertyCheck1, propertyCheck2])
        existentialClause1 = ExistentialClause('ANY', characteristic1)
        existentialClause2 = ExistentialClause('ALL', characteristic2)
        propertyCheck1 = PropertyCheckString("Color", '==', 'Red')
        propertyCheck2 = PropertyCheckBool('IsNextTo', '==', True)
        propertyCheck3 = PropertyCheckBool('IsBehind', '==', False)
        objectChecks = [ObjectCheck(0, 'MUST', propertyCheck1, 'Wall' )]
        relationChecks = [RelationCheck(0,1,'MUST', propertyCheck2, 'CounterTop', 'Wall')]
        logicalExpressions = [
            LogicalExpression([], [RelationCheck(0,1,'MUST', propertyCheck3, 'CounterTop', 'Wall'), RelationCheck(1,0,'MUST', propertyCheck3, 'CounterTop', 'Wall')],[],'OR')
        ]
        logicalExpression = LogicalExpression(objectChecks, relationChecks, logicalExpressions, 'AND')
        self.rule = Rule(existentialClauses=[existentialClause1, existentialClause2],logicalExpression=logicalExpression,)
    
    def testToString(self):
        ruleStr = self.rule.toString()
        # This is just calling two different toString functions, so we just check the final output
        self.assertEqual(ruleStr, 'ANY CounterTop = CounterTop\nALL Wall = Wall{HasThing EQUAL Valid AND IsWall NOT-EQUAL False}\n(0 Color EQUAL Red XOR 0 and 1 IsNextTo EQUAL True XOR (0 and 1 IsBehind EQUAL False XOR 1 and 0 IsBehind EQUAL False))')


class TestObjectCheckMethods(unittest.TestCase):
    def setUp(self):       
        propertyCheck = PropertyCheckBool('HasDoor', '==', False)
        self.objectCheck = ObjectCheck(1, 'MUST', propertyCheck, 'HasDoor')
    
    def testToString(self):
        objcheckStr = self.objectCheck.toString()
        self.assertTrue('1' in objcheckStr)
        self.assertTrue('HasDoor' in objcheckStr)
        self.assertTrue('EQUAL' in objcheckStr)
        self.assertTrue('False' in objcheckStr)

class TestRelationCheckMethods(unittest.TestCase):
    def setUp(self):
        propertyCheck = PropertyCheckBool('Below', '==', False)
        self.relationCheck = RelationCheck(1, 0, "MUST", propertyCheck, 'Wall', 'Floor')
    def testToString(self):
        rcheckStr = self.relationCheck.toString()
        self.assertTrue('0' in rcheckStr)
        self.assertTrue('1' in rcheckStr)
        self.assertTrue('and' in rcheckStr)
        self.assertTrue('Below' in rcheckStr)
        self.assertTrue('EQUAL' in rcheckStr)
        self.assertTrue('False' in rcheckStr)

class TestExistentialCauseMethods(unittest.TestCase):
    def setUp(self):
        characteristic1 = Characteristic('CounterTop', '', [])
        propertyCheck1 = PropertyCheckString('HasThing', '==', 'Valid')
        propertyCheck2 = PropertyCheckBool('IsWall', '!=', False)
        characteristic2 = Characteristic('Wall', '', [propertyCheck1, propertyCheck2])
        self.existentialClause1 = ExistentialClause(Occurrence('ANY'), characteristic1)
        self.existentialClause2 = ExistentialClause(Occurrence('ALL'), characteristic2)

    def testToString(self):
        exclauseStr = self.existentialClause1.toString(0)
        self.assertTrue("ANY" in exclauseStr)
        self.assertTrue('0' in exclauseStr)
        self.assertTrue('=' in exclauseStr)
        self.assertTrue('CounterTop' in exclauseStr)

        # Test propertychecks
        exclauseStr = self.existentialClause2.toString(1)
        self.assertTrue("ALL" in exclauseStr)
        self.assertTrue('1' in exclauseStr)
        self.assertTrue('=' in exclauseStr)
        self.assertTrue('{' in exclauseStr)
        self.assertTrue('HasThing EQUAL Valid' in exclauseStr)
        self.assertTrue('IsWall NOT-EQUAL False' in exclauseStr)
        self.assertTrue('AND' in exclauseStr)
        self.assertTrue('}' in exclauseStr)

class TestPropertyCheckDoubleMethods(unittest.TestCase):
    def testToString(self):
        pcd = PropertyCheckDouble("Test", '>', 12, 'inch')
        pcdStr = pcd.toString()
        self.assertTrue('Test' in pcdStr)
        self.assertTrue('GREATER-THAN' in pcdStr)
        self.assertTrue('12INCH' in pcdStr)
        self.assertEqual(len(pcdStr), 24)
        # Testing other operationDouble options
        pcd = PropertyCheckDouble("Test", '>=', 12, 'inch')
        self.assertTrue('GREATER-THAN-OR-EQUAL' in pcd.toString())
        pcd = PropertyCheckDouble("Test", '==', 12, 'inch')
        self.assertTrue('EQUAL' in pcd.toString())
        pcd = PropertyCheckDouble("Test", '<=', 12, 'inch')
        self.assertTrue('LESS-THAN-OR-EQUAL' in pcd.toString())
        pcd = PropertyCheckDouble("Test", '<', 12, 'inch')
        self.assertTrue('LESS-THAN' in pcd.toString())
        pcd = PropertyCheckDouble("Test", '!=', 12, 'inch')
        self.assertTrue('NOT-EQUAL' in pcd.toString())

class TestPropertyCheckStringMethods(unittest.TestCase):
    def testToString(self):
        pcs = PropertyCheckString("Test", '==', "valid")
        pcsStr = pcs.toString()
        self.assertTrue('Test' in pcsStr)
        self.assertTrue('EQUAL' in pcsStr)
        self.assertTrue('valid' in pcsStr)
        self.assertEqual(len(pcsStr), 16)
        # Testing other operationString options
        pcs = PropertyCheckString("Test", '!=', 'valid')
        self.assertTrue('NOT-EQUAL' in pcs.toString())
        pcs = PropertyCheckString("Test", 'CONTAINS', 'valid')
        self.assertTrue('CONTAINS' in pcs.toString())


class TestPropertyCheckBoolMethods(unittest.TestCase):
    def testToString(self):
        pcb = PropertyCheckBool("Test", '==', True)
        pcbStr = pcb.toString()
        self.assertTrue('Test' in pcbStr)
        self.assertTrue('EQUAL' in pcbStr)
        self.assertTrue("True" in pcbStr)
        # Testing other operationBool option
        pcb = PropertyCheckBool("Test", '!=', False)
        self.assertTrue('NOT-EQUAL' in pcb.toString())

if __name__ == '__main__':
    unittest.main()