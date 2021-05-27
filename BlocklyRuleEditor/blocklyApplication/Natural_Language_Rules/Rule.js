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
    jsonRuleBlock.title = block.getFieldValue('RULENAME');
    jsonRuleBlock.errorLevel = block.getFieldValue('ERRORLEVEL');
    jsonRuleBlock.description = block.getFieldValue('DESCRIPTION');
    //jsonRuleBlock.ecsCount = block.getFieldValue('CountECS');
    //update the ECS inputs of the object
    UpdateJsonECS(id,jsonRuleBlock);
    //update the relation inputs of the object
    UpdateJsonRelations(id,jsonRuleBlock);
}

class Rule{
    constructor(ID, name, description, errorLevel){//, ecsCount){
        this.ID = ID;
        this.Valid = true;

        this.Name = name;
        this.Description = description;
        this.ErrorLevel = errorLevel;
        this.ExistentialClauses = {};
        this.LogicalExpression = {};
    }
    get id(){
        return this.ID;
    }
    set id(x){
        this.ID = x;
    }
    get valid(){
        return this.Valid;
    }
    set valid(x){
        this.Valid = x;
    }
    set name(x) {
        this.Name = x;
    }
    set description(x) {
        this.Description = x;
    }
    set errorLevel(x) {
        this.ErrorLevel = x;
    }
    set ecsCount(x){
        this.ECS_Count = x;
    }

    get existentialClauses(){
        return this.ExistentialClauses;
    }
    set existentialClauses(x) {
        this.ExistentialClauses = x;
    }
    set logicalExpression(x) {
        this.LogicalExpression = x;
    }
    toJSON(){
        return {
            ErrorLevel: this.ErrorLevel,
            ExistentialClauses: this.ExistentialClauses,
            LogicalExpression: this.LogicalExpression,
            Name: this.Name,
            Description: this.Description};
    }
    getXML(){
        var rule = document.createElementNS(XML_NS, 'block');
        rule.setAttribute("type", "ruleblock");
        rule.setAttribute("id", this._ID);

        //fields
        //mutation ecs count
        var field0 = document.createElementNS(XML_NS, 'mutation');
        field0.setAttribute("ecs_count", Object.keys(this.ExistentialClauses).length);
        //name
        var field1 = document.createElementNS(XML_NS, 'field');
        field1.setAttribute("name", "RULENAME");
        field1.textContent = this.Name;
        //errorlevel
        var field2 = document.createElementNS(XML_NS, 'field');
        field2.setAttribute("name", "ERRORLEVEL");
        field2.textContent = this.ErrorLevel;
        //description
        var field3 = document.createElementNS(XML_NS, 'field');
        field3.setAttribute("name", "DESCRIPTION");
        field3.textContent = this.Description;
        //ecs count
        var field4 = document.createElementNS(XML_NS, 'field');
        field4.setAttribute("name", "CountECS");
        field4.textContent = Object.keys(this.ExistentialClauses).length;

        //append fields
        rule.appendChild(field0);
        rule.appendChild(field1);
        rule.appendChild(field2);
        rule.appendChild(field3);
        rule.appendChild(field4);
        console.log(this.ExistentialClauses);
        var i = 1;
        Object.keys(this.ExistentialClauses).forEach(ecskey => {
            //add each ecs name
            var field = document.createElementNS(XML_NS, 'field');
            field.setAttribute("name", "VarName" + i);
            field.textContent = ecskey;
            console.log(ecskey);
            rule.appendChild(field);
            i += 1;
        });
        //logical expression
        var field5 = document.createElementNS(XML_NS, 'field');
        field5.setAttribute("name", "LOGICAL_OPERATOR");
        field5.textContent = this.LogicalExpression.LogicalOperator;
        
        rule.appendChild(field5);
        
        //add ecs blocks
        var i = 1;
        Object.keys(this.ExistentialClauses).forEach(ecskey => {
            var ecs = this.ExistentialClauses[ecskey];
            //add each ecs
            var ecsXML = ecs.getXML(i);
            if(ecsXML != null){
                rule.appendChild(ecsXML);  
            }
            i += 1;
        });
        //add statement inputs (relation check, object check, and logical expression blocks that are connected to the statement input)
        rule = addLogicalExpressionInputsXML(rule, this.LogicalExpression);
        return rule;
    }
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
        var objectCheckXML = objectcheck.getXML();
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
        var relationcheckXML = relationcheck.getXML();
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
        var logicalexpressionXML = logicalexpression.getXML();
        if(logicalexpressionXML != null){
            lastElement.appendChild(logicalexpressionXML);
            lastElement = logicalexpressionXML;
            firststatement = 1;
        }
    });

    return parent;
}