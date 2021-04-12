

//adds object/relation check objects and logical expressions objects to the ruleblock object as properties
//paramters: id = id of ruleblock, jsonRuleObject = rule object
function UpdateJsonRelations( id,jsonRuleObject){
    var ruleBlock = Workspace.getBlockById(id);
    jsonRuleObject.logicalExpression = NewLogicalExpression(ruleBlock, jsonRuleObject);
}

//returns and creates a new json objectcheck object
//paramters: block = object check block, jsonRuleObject = rule object
function NewObjectCheck(block, jsonRuleObject){
    //gets current selected object
    var dropdown_var = block.getFieldValue('Object');
    var index;
    
    //if it is the default dropdown ('select...')
    if(dropdown_var == 'var'){
        index = "missing";
        jsonRuleObject.valid = false;
    }
    else{
        
        //all options in the dropdown are referenced by a string: 'ecs1' or 'ecs2' or 'ecs3' etc.
        //gets the index of the object by parsing the string of the selected by removing the first three characters 'ecs' and then converting the rest to an int.
        index = parseInt(dropdown_var.slice(3,dropdown_var.length));
    }

    //negation ie. has, does not have
    var negation = block.getFieldValue('Negation');

    //property input block
    var propertyBlock =  block.getInputTargetBlock('PROPERTYBLOCK');
    //creates property json object
    var property = NEW_Property(propertyBlock, jsonRuleObject);
    
    return new SingleRelation(index, negation, property);
}

//returns and creates a new json relationcheck object
//paramters: block = relation check block, jsonRuleObject = rule object
function NewRelationCheck(block, jsonRuleObject){
    //first dropdown
    var dropdown_object1 = block.getFieldValue('Object1');
    var index1;
    //set index1
    //if dropdown has default selected ('select...')
    if(dropdown_object1 == 'var'){
        index1 = "missing";
        jsonRuleObject.valid = false;
    }
    else{
        //all options in the dropdown are referenced by a string: 'ecs1' or 'ecs2' or 'ecs3' etc.
        //gets the index of the object by parsing the string of the selected by removing the first three characters 'ecs' and then converting the rest to an int.
        index1 = parseInt(dropdown_object1.slice(3,dropdown_object1.length));
    }

    //second dropdown
    var dropdown_object2 = block.getFieldValue('Object2');
    var index2;
    //set index2
    //if dropdown has default selected ('select...')
    if(dropdown_object2 == 'var'){
        index2 = "missing";
        jsonRuleObject.valid = false;
    }
    else{
        //all options in the dropdown are referenced by a string: 'ecs1' or 'ecs2' or 'ecs3' etc.
        //gets the index of the object by parsing the string of the selected by removing the first three characters 'ecs' and then converting the rest to an int.
        index2 = parseInt(dropdown_object2.slice(3,dropdown_object2.length));
    }

    //relation block input
    var ChildBlock = block.getChildren(true)[0];
    var PropertyCheck = {};

    //create relation json object
    if(ChildBlock != null){
        if(ChildBlock.type == "relationboolean"){
            var operation = ChildBlock.getFieldValue('Negation');
            var name = ChildBlock.getFieldValue('BoolRelation');

            PropertyCheck = new PropertyCheckBool(operation, name);
        }
        else if(ChildBlock.type == "relationnumeric"){
            var operation = ChildBlock.getFieldValue('check');
            var value = ChildBlock.getFieldValue('distance');
            var ValueUnit = ChildBlock.getFieldValue('MeasurementType');
            var name = ChildBlock.getFieldValue('DistanceType');

            var standardValue = ValueToStandardValue(value, ValueUnit);
            PropertyCheck = new PropertyCheckNumeric(operation, value, standardValue,ValueUnit, name);
        }
        else if(ChildBlock.type == "relationstring"){
            var value = ChildBlock.getFieldValue('FunctionString');
            var operation = ChildBlock.getFieldValue('check');
            var name = ChildBlock.getFieldValue('Type');

            PropertyCheck = new PropertyCheckString(value, operation, name);
        }
    }
    else{
        jsonRuleObject.valid = false;
    }

    return new DoubleRelation(index1, index2, PropertyCheck);
}

//returns and creates logical expression json object
//paramters: block = logicalexpression block, jsonRuleObject = rule object
function NewLogicalExpression(block, jsonRuleObject){
    var ObjectChecks = [];
    var RelationChecks = [];
    var LogicalExpressions = [];
    
    //gets the first child block in the statement input.
    var CurrentRelation =  block.getFirstStatementConnection().targetBlock();
    //iterates through all child blocks of the statement input.
    while(CurrentRelation != null){
        //create and add object corresponding to child block to array

        if(CurrentRelation.type == "objectcheck"){
            ObjectChecks.push(NewObjectCheck(CurrentRelation, jsonRuleObject));
        }
        else if(CurrentRelation.type == "relationcheck"){
            RelationChecks.push(NewRelationCheck(CurrentRelation, jsonRuleObject));
        }
        else if(CurrentRelation.type == "logicalexpression"){
            LogicalExpressions.push(NewLogicalExpression(CurrentRelation, jsonRuleObject));
        }
        
        //get next child block
        CurrentRelation = CurrentRelation.getNextBlock();
    }

    var LogicalOperator = block.getFieldValue('LOGICAL_OPERATOR');

    return new LogicalExpression(ObjectChecks, RelationChecks, LogicalExpressions, LogicalOperator);
}


class SingleRelation
{
    constructor(index, negation, propertyCheck){
        this.ObjIndex = index;
        this.Negation = negation;
        this.PropertyCheck = propertyCheck;
    }
}

class DoubleRelation
{
    constructor(index1, index2, propertyCheck){
        this.Obj1Index = index1;
        this.Obj2Index = index2;
        this.PropertyCheck = propertyCheck;
    }
}


class LogicalExpression
{
    constructor(objectChecks, relationChecks, logicalExpressions, logicalOperator){
        this.ObjectChecks = objectChecks;
        this.RelationChecks = relationChecks;
        this.LogicalExpressions = logicalExpressions;
        this.LogicalOperator = logicalOperator;
    }
}
