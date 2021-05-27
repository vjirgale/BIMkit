//print current ruleset object as json string into text box
function PrintIntoTextArea(){
    if(CurrentRuleSet == null){
        //if there is no current rule set return nothing
        document.getElementById("JsonPrint").textContent="";
        alert("An error has occured. Please refresh the browser.")
        return;
    }
    //update the Json
    document.getElementById("JsonPrint").textContent=JSON.stringify(CurrentRuleSet, null, 3);
}

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
    jsonRuleBlock.ecsCount = block.getFieldValue('CountECS');
    //update the ECS inputs of the object
    UpdateJsonECS(id,jsonRuleBlock);
    //update the relation inputs of the object
    UpdateJsonRelations(id,jsonRuleBlock);
}

class RuleSet{
    constructor(ID, name){
        this._ID = ID
        this.Name = name;
        this.Rules = []
        this.XmlString ='';
    }
    set name(x){
        this.Name = x;
    }
    set rules(x){
        this.Rules = x;
    }
    set xmlstring(x){
        this.xmlstring = x;
    }
    //adds rule to ruleset
    AddRule(rule){
        this.Rules.push(rule);
        PrintIntoTextArea();
    }
    //input id of a rule block returns corresponding rule object
    GetRule(ruleID){
        var rule = null;
        this.Rules.forEach(element => {
            if(element.ID == ruleID){
                rule = element;
            }
        });
        return rule;
    }
    updateRule(id){
        var rule = this.GetRule(id);
        if(rule){
            UpdateRuleBlock(rule.ID, rule);
        }
        PrintIntoTextArea();
    }
    deleteRule(id){
        var i = 0;
        this.Rules.forEach(element => {
            //if the id matches delete rule from ruleset
            if(element.ID == id){
                this.Rules.splice(i,1);
            }
            else{
                //update rule
                this.updateRule(element.ID);
            }
            i+= 1;
        });
        PrintIntoTextArea();
    }
    //update all rules in 
    updateAll(){
        var i = 0;
        this.Rules.forEach(element => {
            this.updateRule(element.ID);
            i+= 1;
        });
        PrintIntoTextArea();
    }
    //checks if the current rule set is valid
    isValid(){
        var is_valid = true;
        //iterates every rule insided the set and checks if it is valid
        this.Rules.forEach(element => {
            if(!element.valid){
                is_valid = false;
                return false;
            }
        });
          
        return is_valid;
    }
    getXML(){
        var doc = document.implementation.createDocument ('https://developers.google.com/blockly/xml', 'xml', null);
        this.Rules.forEach(rule => {
            //add each rule
            var ruleXML = rule.getXML();
            doc.documentElement.appendChild(ruleXML);    
        });
        return doc;
    }
}

//taken from https://stackoverflow.com/questions/105034/how-to-create-guid-uuid
function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = (performance && performance.now && (performance.now()*1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if(d > 0){//Use timestamp until depleted
            r = (d + r)%16 | 0;
            d = Math.floor(d/16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r)%16 | 0;
            d2 = Math.floor(d2/16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}

//gloabal variable that references the ruleset object
//*note: there is only 1 ruleset
var CurrentRuleSet = new RuleSet(generateUUID(),"");

class RuleBlock{
    constructor(ID, title, description, errorLevel, ecsCount){
        this._ID = ID;
        this.Valid = true;
        this.Title = title;
        this.Description = description;
        this.ErrorLevel = errorLevel;
        this.ECS_Count = ecsCount
        this.ExistentialClauses = [];
        this.LogicalExpression = {};
    }
    get ID(){
        return this._ID;
    }

    get valid(){
        return this.Valid;
    }
    set valid(x){
        this.Valid = x;
    }

    get title() {
        return this.Title;
    }
    set title(x) {
        this.Title = x;
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
    getXML(){
        var rule = document.createElementNS('https://developers.google.com/blockly/xml', 'block');
        rule.setAttribute("type", "ruleblock");
        rule.setAttribute("id", this._ID);

        //fields
        //mutation ecs count
        var field0 = document.createElementNS('https://developers.google.com/blockly/xml', 'mutation');
        field0.setAttribute("ecs_count", this.ECS_Count);
        //name
        var field1 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field1.setAttribute("name", "RULENAME");
        field1.textContent = this.Title;
        //errorlevel
        var field2 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field2.setAttribute("name", "ERRORLEVEL");
        field2.textContent = this.ErrorLevel;
        //description
        var field3 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field3.setAttribute("name", "DESCRIPTION");
        field3.textContent = this.Description;
        //ecs count
        var field4 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field4.setAttribute("name", "CountECS");
        field4.textContent = this.ECS_Count;
        //logical expression
        var field5 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field5.setAttribute("name", "LOGICAL_OPERATOR");
        field5.textContent = this.LogicalExpression.LogicalOperator;
        //append fields
        rule.appendChild(field0);
        rule.appendChild(field1);
        rule.appendChild(field2);
        rule.appendChild(field3);
        rule.appendChild(field4);
        rule.appendChild(field5);
        
        //add ecs blocks
        var i = 1;
        this.ExistentialClauses.forEach(ecs => {
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
    var statementField = document.createElementNS('https://developers.google.com/blockly/xml', 'statement');
    statementField.setAttribute("name", "LOGICALEXPRESSION");
    parent.appendChild(statementField);

    //bool to check if a next element is needed. if firststatement > 0 then next statement is needed
    var firststatement = 0;
    let lastElement = statementField;
    //add object check blocks
    LogicalExpression.ObjectChecks.forEach(objectcheck => {
        if(firststatement > 0){
            //add next
            var nextfield= document.createElementNS('https://developers.google.com/blockly/xml', 'next');
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
            var nextfield= document.createElementNS('https://developers.google.com/blockly/xml', 'next');
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
            var nextfield= document.createElementNS('https://developers.google.com/blockly/xml', 'next');
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

