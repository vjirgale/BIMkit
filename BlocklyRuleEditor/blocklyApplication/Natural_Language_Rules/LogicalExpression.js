//adds object/relation check objects and logical expressions objects to the ruleblock object as properties
//paramters: id = id of ruleblock, jsonRuleObject = rule object
function UpdateJsonRelations( id,jsonRuleObject){
    var ruleBlock = Workspace.getBlockById(id);
    jsonRuleObject.logicalExpression = NewLogicalExpression(ruleBlock, jsonRuleObject, id);
}

//returns and creates logical expression json object
//paramters: block = logicalexpression block, jsonRuleObject = rule object
function NewLogicalExpression(block, jsonRuleObject, ruleid){
    var ObjectChecks = [];
    var RelationChecks = [];
    var LogicalExpressions = [];
    
    //gets the first child block in the statement input.
    var CurrentRelation =  block.getFirstStatementConnection().targetBlock();
    //iterates through all child blocks of the statement input.
    while(CurrentRelation != null){
        //create and add object corresponding to child block to array

        if(CurrentRelation.type == "objectcheck"){
            ObjectChecks.push(NewObjectCheck(CurrentRelation, jsonRuleObject, ruleid));
        }
        else if(CurrentRelation.type == "relationcheck"){
            RelationChecks.push(NewRelationCheck(CurrentRelation, jsonRuleObject, ruleid));
        }
        else if(CurrentRelation.type == "logicalexpression"){
            LogicalExpressions.push(NewLogicalExpression(CurrentRelation, jsonRuleObject, ruleid));
        }
        
        //get next child block
        CurrentRelation = CurrentRelation.getNextBlock();
    }

    var LogicalOperator = block.getFieldValue('LOGICAL_OPERATOR');

    return new LogicalExpression(ObjectChecks, RelationChecks, LogicalExpressions, LogicalOperator);
}


class LogicalExpression
{
    constructor(objectChecks, relationChecks, logicalExpressions, logicalOperator){
        this.ObjectChecks = objectChecks;
        this.RelationChecks = relationChecks;
        this.LogicalExpressions = logicalExpressions;
        this.LogicalOperator = logicalOperator;
    }
    getXML(){
        var LogicalExpressionblock = document.createElementNS(XML_NS, 'block');
        LogicalExpressionblock.setAttribute("type", "logicalexpression");

        var field0 = document.createElementNS(XML_NS, 'field');
        field0.setAttribute("name", "LOGICAL_OPERATOR");
        field0.textContent = this.LogicalOperator;

        LogicalExpressionblock.appendChild(field0);

        //add statement inputs (relation check, object check, and logical expression blocks that are connected to the statement input)
        LogicalExpressionblock = addLogicalExpressionInputsXML(LogicalExpressionblock, this);

        return LogicalExpressionblock
    }
}
