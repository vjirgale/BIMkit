//input a rule block id and the corresponding json object 
//parameters: id = id of a rule block, jsonRuleBlock = the corresponding json object of the rule block
function UpdateRuleBlock(id, jsonRuleBlock){
    //get the rule block using the id.
    block = Workspace.getBlockById(id);
    if(block == null){
        return null;
    }
    //reset valid
    jsonRuleBlock.valid = true;
    //set values names, description, errorlevel and the amount of ECS inputs.
    jsonRuleBlock.name = block.getFieldValue('RULENAME');
    jsonRuleBlock.errorLevel = block.getFieldValue('ERRORLEVEL');
    jsonRuleBlock.description = block.getFieldValue('DESCRIPTION');
    //jsonRuleBlock.ecsCount = block.getFieldValue('CountECS');
    //update the ECS inputs of the object
    UpdateJsonECS(id,jsonRuleBlock);
    //update the relation inputs of the object
    UpdateJsonRelations(id,jsonRuleBlock);
}



function getRuleXML(rule){
    var rulexml = document.createElementNS(XML_NS, 'block');
    rulexml.setAttribute("type", "ruleblock");
    rulexml.setAttribute("id", rule.ID);

    //fields
    //mutation ecs count
    var field0 = document.createElementNS(XML_NS, 'mutation');
    field0.setAttribute("ecs_count", Object.keys(rule.ExistentialClauses).length);
    //name
    var field1 = document.createElementNS(XML_NS, 'field');
    field1.setAttribute("name", "RULENAME");
    field1.textContent = rule.Name;
    //errorlevel
    var field2 = document.createElementNS(XML_NS, 'field');
    field2.setAttribute("name", "ERRORLEVEL");
    field2.textContent = rule.ErrorLevel;
    //description
    var field3 = document.createElementNS(XML_NS, 'field');
    field3.setAttribute("name", "DESCRIPTION");
    field3.textContent = rule.Description;
    //ecs count
    var field4 = document.createElementNS(XML_NS, 'field');
    field4.setAttribute("name", "CountECS");
    field4.textContent = Object.keys(rule.ExistentialClauses).length;

    //append fields
    rulexml.appendChild(field0);
    rulexml.appendChild(field1);
    rulexml.appendChild(field2);
    rulexml.appendChild(field3);
    rulexml.appendChild(field4);

    var i = 1;
    Object.keys(rule.ExistentialClauses).forEach(ecskey => {
        //add each ecs name
        var field = document.createElementNS(XML_NS, 'field');
        field.setAttribute("name", "VarName" + i);
        field.textContent = ecskey;
        rulexml.appendChild(field);
        i += 1;
    });
    //logical expression
    var field5 = document.createElementNS(XML_NS, 'field');
    field5.setAttribute("name", "LOGICAL_OPERATOR");
    field5.textContent = rule.LogicalExpression.LogicalOperator;
    
    rulexml.appendChild(field5);
    
    //add ecs blocks
    var i = 1;
    Object.keys(rule.ExistentialClauses).forEach(ecskey => {
        var ecs = rule.ExistentialClauses[ecskey];
        //add each ecs
        var ecsXML = getECSXML(ecs, i);//ecs.getXML(i);
        if(ecsXML != null){
            rulexml.appendChild(ecsXML);  
        }
        i += 1;
    });
    //add statement inputs (relation check, object check, and logical expression blocks that are connected to the statement input)
    rulexml = addLogicalExpressionInputsXML(rulexml, rule.LogicalExpression);
    return rulexml;
}