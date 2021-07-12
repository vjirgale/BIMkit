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


function getLogicalExpressionXML(logicalExpression){
    var LogicalExpressionXML = document.createElementNS(XML_NS, 'block');
    LogicalExpressionXML.setAttribute("type", "logicalexpression");

    var field0 = document.createElementNS(XML_NS, 'field');
    field0.setAttribute("name", "LOGICAL_OPERATOR");
    field0.textContent = logicalExpression.LogicalOperator;

    LogicalExpressionXML.appendChild(field0);

    //add statement inputs (relation check, object check, and logical expression blocks that are connected to the statement input)
    LogicalExpressionXML = addLogicalExpressionInputsXML(LogicalExpressionXML, logicalExpression);

    return LogicalExpressionXML
}


//add statement inputs (relation check, object check, and logical expression blocks that are connected to the statement input)
//parent = parent xml element
function addLogicalExpressionInputsXML(parent, LogicalExpression){
    var statementField = document.createElementNS(XML_NS, 'statement');
    statementField.setAttribute("name", "LOGICALEXPRESSION");
    parent.appendChild(statementField);

    //bool to check if a next element is needed. if firststatement > 0 then next statement is needed
    var firststatement = 0;
    let lastElement = statementField;
    //add object check blocks
    LogicalExpression.ObjectChecks.forEach(objectcheck => {
        if(firststatement > 0){
            //add next
            var nextfield= document.createElementNS(XML_NS, 'next');
            lastElement.appendChild(nextfield);
            lastElement = nextfield;
        }
        //add each ecs
        var objectCheckXML = getObjectCheckXML(objectcheck);//objectcheck.getXML();
        if(objectCheckXML != null){
            lastElement.appendChild(objectCheckXML);
            lastElement = objectCheckXML;
            firststatement = 1;
        }
    });

    //add relation checks
    LogicalExpression.RelationChecks.forEach(relationcheck => {
        if(firststatement > 0){
            //add next
            var nextfield= document.createElementNS(XML_NS, 'next');
            lastElement.appendChild(nextfield);
            lastElement = nextfield;
        }
        //add each ecs
        var relationcheckXML = getRelationCheckXML(relationcheck);//relationcheck.getXML();
        if(relationcheckXML != null){
            lastElement.appendChild(relationcheckXML);
            lastElement = relationcheckXML;
            firststatement = 1;
        }
    });
    //add logical expressions
    LogicalExpression.LogicalExpressions.forEach(logicalexpression => {
        if(firststatement > 0){
            //add next
            var nextfield= document.createElementNS(XML_NS, 'next');
            lastElement.appendChild(nextfield);
            lastElement = nextfield;
        }
        //add each ecs
        var logicalexpressionXML = getLogicalExpressionXML(logicalexpression);//logicalexpression.getXML();
        if(logicalexpressionXML != null){
            lastElement.appendChild(logicalexpressionXML);
            lastElement = logicalexpressionXML;
            firststatement = 1;
        }
    });

    return parent;
}