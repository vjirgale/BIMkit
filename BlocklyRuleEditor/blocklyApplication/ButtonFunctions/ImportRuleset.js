

//function that loads in ruleset from json file
function LoadInRule(){
    //selected file
    var file = document.getElementById("inputfile").files[0];
    if(file){
        var reader = new FileReader();
        reader.readAsText(file, "UTF-8");
        reader.onload = function (evt) {
        //document.getElementById("importRule").textContent = evt.target.result;
        try{
            //create object
            var newOBJ = JSON.parse(evt.target.result);
            //assign and ensure that the imported file is valid
            var importedRule = assignObjectToRule(newOBJ);
            LoadRuleObjectToWorkSpace(importedRule)
            
        }
        catch(e){
            alert(e);
        }
        //reset file selection
        document.getElementById("inputfile").value = "";
        }
        reader.onerror = function (evt) {
        alert("error reading file");
        }
    }
}

//function that loads in ruleset from json file
function LoadInRuleSet(){
    //selected file
    var file = document.getElementById("inputfile").files[0];
    if(file){
        var reader = new FileReader();
        reader.readAsText(file, "UTF-8");
        reader.onload = function (evt) {
        //document.getElementById("importRule").textContent = evt.target.result;
        try{
            //create object
            var newOBJ = JSON.parse(evt.target.result);
            //assign and ensure that the imported file is valid
            var importedRuleset = assignObjectToRuleset(newOBJ);

            importedRuleset.Rules.forEach(rule =>{
                LoadRuleObjectToWorkSpace(rule);
            });

        }
        catch(e){
            alert(e);
        }
        //reset file selection
        document.getElementById("inputfile").value = "";
        }
        reader.onerror = function (evt) {
            alert("error reading file");
        }
    }
}

let KeyToIndex = {}
function LoadRuleObjectToWorkSpace(importedRule){
    //change the default options for check blocks
    //*note this is a workaround to error: Cannot set the dropdown's value to an unavailable option.
    //This is ensures that all check blocks can access the ecs they were assigned to.
    var ECSKeys = Object.keys(importedRule.ExistentialClauses);
    for(var i = 0; i <ECSKeys.length; i++){
        KeyToIndex[ECSKeys[i]] = (i+1).toString();
        DefaultOptions.push([ECSKeys[i],(i+1).toString()]);
    }

    var doc = document.implementation.createDocument (XML_NS, 'xml', null);
    var ruleXML = getRuleXML(importedRule);
    doc.documentElement.appendChild(ruleXML);

    //load blocks into workspace
    var Dom = Blockly.Xml.textToDom(Blockly.Xml.domToText(doc));         
    Blockly.Xml.domToWorkspace(Dom, Workspace);

    //restore DefaultOptions
    DefaultOptions = [['select...','0']];
}

//this function is used to assign an object that has been imported from a json file to a ruleset class.
//throws an error if there is missing information/the json file is not a ruleset.
function assignObjectToRuleset(ObjectToAssign){
    
    //check if it has Rules property
    if(!ObjectToAssign.hasOwnProperty("Rules")){
        throw "ruleset object missing property: Rules";
    }


    //assign object to ruleset
    var newRuleSet = Object.assign(new RuleSet, ObjectToAssign);

    assingedRules = []
    //assign all objects in ObjectToAssign.Rules to ruleblock class
    newRuleSet.Rules.forEach(rule => {
        assingedRules.push(assignObjectToRule(rule));
    });
    newRuleSet.Rules = assingedRules;

    //return assigned object
    return newRuleSet
}

//assigns an object to Rule class.
function assignObjectToRule(ObjectToAssign){
    //all properties of Rule class
    var RuleProperties = Object.getOwnPropertyNames((new Rule).toJSON());

    //check if the ObjectToAssign has all properties of a Rule
    RuleProperties.forEach(property => {
        //if a property is missing throw an error.
        if(!ObjectToAssign.hasOwnProperty(property)){
            throw "rule object missing property: "+ property;
        }
    });

    //assign object to rule
    var newRule = Object.assign(new Rule, ObjectToAssign);
    var assingedECSs = {};
    let ecskeys = Object.keys(newRule.ExistentialClauses);

    // assign all ecs in rule to ExistentialClause
    ecskeys.forEach(ecskey => {
        assingedECSs[ecskey] = assignObjectToECS(newRule.ExistentialClauses[ecskey], ecskey);
    });
    newRule.ExistentialClauses = assingedECSs;
    //check if each logical expression is valid
    //console.log(newRule.LogicalExpression);
    newRule.LogicalExpression = assignObjectToLogicalExpression(newRule.LogicalExpression);

    //return valid assinged rule object
    return newRule;
}

function assignObjectToECS(ObjectToAssign, ecskey){
    var ECSProperties = Object.getOwnPropertyNames(new ExistentialClause);
    //check if object to assign has all properties of a ECS.
    ECSProperties.forEach(property => {
        //check if the object is missing a property
        if(!ObjectToAssign.hasOwnProperty(property)){
            throw "ecs object missing property: "+ property;
        }
    });

    //assign object to ECS
    var newECS = Object.assign(new ExistentialClause, ObjectToAssign);
    //if a ecs was not filled in.
    if(newECS.Characteristic == "missing"){
        return newECS;
    }

    //check characteristic
    var CharacteristicProperties = Object.getOwnPropertyNames(new Characteristic);
    //check if object to assign has all properties of a ECS.
    CharacteristicProperties.forEach(property => {
        //check if the object is missing a property
        if(!newECS.Characteristic.hasOwnProperty(property)){
            throw "Characteristic object missing property: "+ property;
        }
    });
    newECS.Characteristic = Object.assign(new Characteristic, newECS.Characteristic);
    //assign all propertyChecks
    assingedPropertyChecks = [];
    newECS.Characteristic.PropertyChecks.forEach(propertyCheck => {
        //assign property check if it is not missing
        if(propertyCheck != "missing"){
            assingedPropertyChecks.push(assignObjectToProperty(propertyCheck));
        }else{
            assingedPropertyChecks.push(propertyCheck);
        }
    });
    newECS.Characteristic.PropertyChecks = assingedPropertyChecks;

    return newECS;
}

function assignObjectToProperty(ObjectToAssign){
    //get type
    if(!ObjectToAssign.hasOwnProperty("PCType")){
        throw "property object missing property: PCType";
    }

    switch(ObjectToAssign.PCType) {
        //bool property check
        case "BOOL":
            var boolProperties = Object.getOwnPropertyNames(new PropertyCheckBool);

            boolProperties.forEach(property => {
                //check if the object is missing a property
                if(!ObjectToAssign.hasOwnProperty(property)){
                    throw "boolpropertycheck object missing property: "+ property;
                }
            });
            return  Object.assign(new PropertyCheckBool, ObjectToAssign);

        //numeric property check
        case "NUM":
            var NumericProperties = Object.getOwnPropertyNames(new PropertyCheckNumeric);

            NumericProperties.forEach(property => {
                //check if the object is missing a property
                if(!ObjectToAssign.hasOwnProperty(property)){
                    throw "numericpropertycheck object missing property: "+ property;
                }
            });
            return  Object.assign(new PropertyCheckNumeric, ObjectToAssign);

        //string property check
        case "STRING":
            var Stringroperties = Object.getOwnPropertyNames(new PropertyCheckString);

            Stringroperties.forEach(property => {
                //check if the object is missing a property
                if(!ObjectToAssign.hasOwnProperty(property)){
                    throw "stringpropertycheck object missing property: "+ property;
                }
            });
            return  Object.assign(new PropertyCheckString, ObjectToAssign);
    }
    throw console.error("Not Valid Property.");
}

function assignObjectToLogicalExpression(ObjectToAssign){
    //check has logical expression properties
    var LogicalExpressionProperties = Object.getOwnPropertyNames(new LogicalExpression);

    LogicalExpressionProperties.forEach(property => {
        //check if the object is missing a property
        if(!ObjectToAssign.hasOwnProperty(property)){
            throw "stringpropertycheck object missing property: "+ property;
        }
    });
    


    //assign all object checks
    assingedObjectChecks = [];
    ObjectToAssign.ObjectChecks.forEach(objectCheck => {
        //check has object check properties
        var ObjectCheckProperties = Object.getOwnPropertyNames(new SingleRelation);

        ObjectCheckProperties.forEach(property => {
            //check if the object is missing a property
            if(!objectCheck.hasOwnProperty(property)){
                throw "objectcheck object missing property: "+ property;
            }
        });
        //assign object check
        var newObjectCheck = Object.assign(new SingleRelation, objectCheck);

        //assign property check if it is not missing
        if(newObjectCheck.PropertyCheck != "missing"){
            newObjectCheck.PropertyCheck = assignObjectToProperty(objectCheck.PropertyCheck);
        }
        assingedObjectChecks.push(newObjectCheck);
    });
    ObjectToAssign.ObjectChecks = assingedObjectChecks;


    //assign all relation checks
    assingedRelationChecks = [];
    ObjectToAssign.RelationChecks.forEach(relationCheck => {
        //check has object check properties
        var RelationCheckProperties = Object.getOwnPropertyNames(new DoubleRelation);

        RelationCheckProperties.forEach(property => {
            //check if the object is missing a property
            if(!relationCheck.hasOwnProperty(property)){
                throw "relationcheck object missing property: "+ property;
            }
        });
        //assign object check
        var newRelationCheck = Object.assign(new DoubleRelation, relationCheck);

        //assign property check if it is not missing
        if(newRelationCheck.PropertyCheck != "missing"){
            newRelationCheck.PropertyCheck = assignObjectToProperty(relationCheck.PropertyCheck);
        }
        assingedRelationChecks.push(newRelationCheck);
    });
    ObjectToAssign.RelationChecks = assingedRelationChecks;

    var assingedLogicalExpressions = [];
    var i = 0;
    for(; i < ObjectToAssign.LogicalExpressions.length;i++){
        var assignedLE = assignObjectToLogicalExpression(ObjectToAssign.LogicalExpressions[i]);
        assingedLogicalExpressions.push(assignedLE);
    }
    console.log(assingedLogicalExpressions);
    ObjectToAssign.LogicalExpressions = assingedLogicalExpressions;

    //assign object to logical expression
    var newLogicalExpression = Object.assign(new LogicalExpression, ObjectToAssign);

    return newLogicalExpression;
}