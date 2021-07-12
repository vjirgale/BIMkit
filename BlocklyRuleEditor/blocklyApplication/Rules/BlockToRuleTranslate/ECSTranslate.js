//update the json definition for a ecs block.
//paramters: id = blockly id of ruleblock, jsonRuleObject = json definition of a rule object
function UpdateJsonECS(id,jsonRuleObject){
    //the blockly rule block
    var ruleBlock = Workspace.getBlockById(id);
    
    var newExistentialClauses = {};

    //iterate through every ecs in ruleblock
    var ECS_COUNT = ruleBlock.getFieldValue('CountECS');
    for(var i = 1; i <= ECS_COUNT; i++){

        var ecsName = ruleBlock.getFieldValue('VarName'+i);
        //input/ecs block
        var ecsblock = ruleBlock.getInputTargetBlock('ECS'+i);
        
        //create and add an ECS object to newExistentialClauses
        //newExistentialClauses['Object'+(i-1)] = (NEW_ECS(ecsName, ecsblock, jsonRuleObject));
        newExistentialClauses[ecsName] = (NEW_ECS(ecsblock, jsonRuleObject));
    }

    //set ecs of rule object 
    jsonRuleObject.existentialClauses = newExistentialClauses;
}

//creates a new ecs json object
//paramters: name = name of ecs defined by kitchen designer,Block = ecs input block ,jsonRuleObject = rule object
function NEW_ECS(Block, jsonRuleObject){
    //if there is no input block create an ecs object marked as missing
    if(Block == null){
        jsonRuleObject.valid = false;
        return new ExistentialClause('missing', 'missing');
    }

    //get OccuranceRule
    var OccurenceRule =  Block.getFieldValue('ObjectSelection');
    
    //array for property objects
    var properties = [];
    //amount of properties
    var property_Count = Block.getFieldValue('PropertyCount');

    //foreach property input
    for(var i = 1; i <= property_Count; i++){
        //input/property block
        var propertyBlock = Block.getInputTargetBlock('Property'+i);
        //add property object to properties
        properties.push(NEW_Property(propertyBlock, jsonRuleObject));
    }

    //get type of object i.e. chair
    var type = Block.getFieldValue('TypeOfObject');

    var characteristic = new Characteristic(type, properties);
    var newECS = new ExistentialClause(OccurenceRule, characteristic);
    return newECS;
}




//index = position of the ecs from top to bottom of rule block. starting at 1
function getECSXML(ecs,index){
    if(ecs.OccurenceRule == 'missing'){
        return null;
    }
    //ecs element
    var ecsXML = document.createElementNS(XML_NS, 'value');
    
    ecsXML.setAttribute("name", "ECS"+index);
    var ecsblock = document.createElementNS(XML_NS, 'block');
    ecsblock.setAttribute("type", "ecsblock");
    //fields

    var field0 = document.createElementNS(XML_NS, 'mutation');
    field0.setAttribute("PropertyCount", ecs.Characteristic.PropertyChecks.length);
    //OccurenceRule
    var field1 = document.createElementNS(XML_NS, 'field');
    field1.setAttribute("name", "ObjectSelection");
    field1.textContent = ecs.OccurenceRule;

    //type i.e. Chair
    var field2 = document.createElementNS(XML_NS, 'field');
    field2.setAttribute("name", "TypeOfObject");
    field2.textContent = ecs.Characteristic.Type;

    var field3 = document.createElementNS(XML_NS, 'field');
    field3.setAttribute("name", "PropertyCount");
    field3.textContent = ecs.Characteristic.PropertyChecks.length;
    
    //add fields
    ecsblock.appendChild(field0);
    ecsblock.appendChild(field1);
    ecsblock.appendChild(field2);
    ecsblock.appendChild(field3);
    
    var i = 1;
    ecs.Characteristic.PropertyChecks.forEach(property => {
        if(property != 'missing'){
            //add each property
            var propertyXML = getPropertyXML(property, i); //property.getXML(i);
            ecsblock.appendChild(propertyXML);  
        }
        i += 1;
    });   

    ecsXML.appendChild(ecsblock);
    return ecsXML;
}