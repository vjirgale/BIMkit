//update the json definition for a ecs block.
//paramters: id = blockly id of ruleblock, jsonRuleObject = json definition of a rule object
function UpdateJsonECS(id,jsonRuleObject){
    //the blockly rule block
    var ruleBlock = Workspace.getBlockById(id);
    
    var newExistentialClauses = [];

    //iterate through every ecs in ruleblock
    var ECS_COUNT = ruleBlock.getFieldValue('CountECS');
    for(var i = 1; i <= ECS_COUNT; i++){

        var ecsName = ruleBlock.getFieldValue('VarName'+i);
        //input/ecs block
        var ecsblock = ruleBlock.getInputTargetBlock('ECS'+i);
        
        //create and add an ECS object to newExistentialClauses
        newExistentialClauses.push(NEW_ECS(ecsName, ecsblock, jsonRuleObject));
    }

    //set ecs of rule object 
    jsonRuleObject.existentialClauses = newExistentialClauses;
}

//creates a new ecs json object
//paramters: name = name of ecs defined by kitchen designer,Block = ecs input block ,jsonRuleObject = rule object
function NEW_ECS(name, Block, jsonRuleObject){
    //if there is no input block create an ecs object marked as missing
    if(Block == null){
        jsonRuleObject.valid = false;
        return new ExistentialClause(name, 'missing', 'missing');
    }

    //get OccuranceRule
    var OccuranceRule =  Block.getFieldValue('ObjectSelection');
    
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
    var newECS = new ExistentialClause(name, OccuranceRule, characteristic);
    return newECS;
}

//creates a new propety object
//paramters: Block = property input block ,jsonRuleObject = rule object
function NEW_Property(Block, jsonRuleObject){
    //if there is no input block create a property object marked as missing
    if(Block == null){
        jsonRuleObject.valid = false;
        return "missing";
    }

    switch (Block.type){
        case "propertyattachments":
            var operation = Block.getFieldValue('NEGATION');
            var name = Block.getFieldValue('ATTACHMENT');
            return new PropertyCheckBool(operation, name);
        case "propertydimension":
            var operation = Block.getFieldValue('SIGN');
            var value = Block.getFieldValue('VALUE');
            var ValueUnit = Block.getFieldValue('UNIT');
            var name = Block.getFieldValue('DIMENSION');
    
            var standardValue = ValueToStandardValue(value, ValueUnit);
            return new PropertyCheckNumeric(operation, value, standardValue, ValueUnit, name);
        case "propertystring":
            var value = Block.getFieldValue('STRING');
            var operation = Block.getFieldValue('SIGN');
            var name = Block.getFieldValue('FUNCTION');
    
            return new PropertyCheckString(value, operation, name);        
    }
    //if the type was none of the defined types
    return 'ERROR';
}

//UnitEnums = {MM:'0', CM:'1', M:'2', INCH:'3', FT:'4', DEG:'5', RAD:'6'}
//takes in a numeric value and a unit and returns the value in standard value
function ValueToStandardValue(value, unit){
    switch (unit){
        case "0":
            return value * 0.01;
        case "1":
            return value * 0.1;
        case "2":
            return value;
        case "3":
            return value * 0.0254;
        case "4":
            return value * 0.3048;
        default:
            return NaN;
    }
}


class ExistentialClause
{
    constructor(name, occuranceRule, characteristic){
        this.Name = name;
        this.OccuranceRule = occuranceRule;
        this.Characteristic = characteristic;
    }
    set name(x){
        this.Name = x; 
    }
    set occuranceRule(x) {
        this.OccuranceRule = x;
    }
    set characteristic(x) {
        this.Characteristic = x;
    }

    //index = position of the ecs from top to bottom of rule block. starting at 1
    getXML(index){
        if(this.OccuranceRule == 'missing'){
            return null;
        }
        //ecs element
        var ecs = document.createElementNS('https://developers.google.com/blockly/xml', 'value');
        
        ecs.setAttribute("name", "ECS"+index);
        var ecsblock = document.createElementNS('https://developers.google.com/blockly/xml', 'block');
        ecsblock.setAttribute("type", "ecsblock");
        //fields

        var field0 = document.createElementNS('https://developers.google.com/blockly/xml', 'mutation');
        field0.setAttribute("PropertyCount", this.Characteristic.PropertyChecks.length);
        //OccuranceRule
        var field1 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field1.setAttribute("name", "ObjectSelection");
        field1.textContent = this.OccuranceRule;

        //type i.e. Chair
        var field2 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field2.setAttribute("name", "TypeOfObject");
        field2.textContent = this.Characteristic.Type;

        var field3 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field3.setAttribute("name", "PropertyCount");
        field3.textContent = this.Characteristic.PropertyChecks.length;
        
        //add fields
        ecsblock.appendChild(field0);
        ecsblock.appendChild(field1);
        ecsblock.appendChild(field2);
        ecsblock.appendChild(field3);
        
        var i = 1;
        this.Characteristic.PropertyChecks.forEach(property => {
            if(property != 'missing'){
                //add each property
                var propertyXML = property.getXML(i);
                ecsblock.appendChild(propertyXML);  
            }
            i += 1;
        });   

        ecs.appendChild(ecsblock);
        return ecs;
    }
}

class Characteristic
{
    constructor(type, propertyChecks){
        this.Type = type;
        this.PropertyChecks = propertyChecks;
    }
    set type(x) {
        this.Type = x;
    }
    set propertyChecks(x) {
        this.PropertyChecks = x;
    }
}

class PropertyCheckBool
{
    constructor(operation, name){
        this.$type = "RulesPackage.PropertyCheckBool, RulesPackage";
        this.Operation = operation;
        this.Name = name;
    }
    getXML(index){
        //ecs element
        var property = document.createElementNS('https://developers.google.com/blockly/xml', 'value');
        if(index == -1){
            property.setAttribute("name", "Relation");
        }
        else if(index == 0){
            property.setAttribute("name", "PROPERTYBLOCK");
        }else{
            property.setAttribute("name", "Property"+index);
        }
        
        var propertyblock = document.createElementNS('https://developers.google.com/blockly/xml', 'block');
        propertyblock.setAttribute("type",  index==-1?"relationboolean":"propertyattachments");
        //fields
        //create element for negation i.e. Has, Has not
        var field0 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field0.setAttribute("name", "NEGATION");
        field0.textContent = this.Operation;
        ////create element for attachment i.e. door
        var field1 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field1.setAttribute("name", "ATTACHMENT");
        field1.textContent = this.Name;

        //add fields to block
        propertyblock.appendChild(field0);
        propertyblock.appendChild(field1);

        //append block
        property.appendChild(propertyblock);
        return property;
    }
}

class PropertyCheckNumeric
{
    constructor(operation, value, ValueInStandardUnit, ValueUnit, name){
        this.$type = "RulesPackage.PropertyCheckNum, RulesPackage";
        this.Operation = operation;
        this.Value = value;
        this.ValueInStandardUnit = ValueInStandardUnit;
        this.ValueUnit = ValueUnit;
        this.Name = name;
    }
    getXML(index){
        //ecs element
        var property = document.createElementNS('https://developers.google.com/blockly/xml', 'value');
        if(index == -1){
            property.setAttribute("name", "Relation");
        }
        else if(index == 0){
            property.setAttribute("name", "PROPERTYBLOCK");
        }else{
            property.setAttribute("name", "Property"+index);
        }
        var propertyblock = document.createElementNS('https://developers.google.com/blockly/xml', 'block');
        propertyblock.setAttribute("type", index==-1?"relationnumeric":"propertydimension");
        //fields

        //create element for dimension i.e width
        var field0 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field0.setAttribute("name", index==-1?"DistanceType":"DIMENSION");
        field0.textContent = this.Name;
        //create element for sign i.e. =, <, > etc.
        var field1 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field1.setAttribute("name", "SIGN");
        field1.textContent = this.Operation;
        //create element for numeric value
        var field2 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field2.setAttribute("name", "VALUE");
        field2.textContent = this.Value;
        //create element for unit of value
        var field3 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field3.setAttribute("name", "UNIT");
        field3.textContent = this.ValueUnit;

        //add fields to block
        propertyblock.appendChild(field0);
        propertyblock.appendChild(field1);
        propertyblock.appendChild(field2);
        propertyblock.appendChild(field3);

        //append block
        property.appendChild(propertyblock);
        return property;
    }
}

class PropertyCheckString
{
    constructor(value, operation, name){
        this.$type = "RulesPackage.PropertyCheckString, RulesPackage";
        this.Value = value;
        this.Operation = operation;
        this.Name = name;
    }
    getXML(index){
        //ecs element
        var property = document.createElementNS('https://developers.google.com/blockly/xml', 'value');
        if(index == -1){
            property.setAttribute("name", "Relation");
        }
        else if(index == 0){
            property.setAttribute("name", "PROPERTYBLOCK");
        }else{
            property.setAttribute("name", "Property"+index);
        }
        var propertyblock = document.createElementNS('https://developers.google.com/blockly/xml', 'block');
        propertyblock.setAttribute("type", index==-1?"relationstring":"propertystring");
        //fields
        //create element for function
        var field0 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field0.setAttribute("name", "FUNCTION");
        field0.textContent = this.Name;
        //create element for operation i.e. =, <, > etc.
        var field1 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field1.setAttribute("name", "SIGN");
        field1.textContent = this.Operation;
        ////create element for string
        var field2 = document.createElementNS('https://developers.google.com/blockly/xml', 'field');
        field2.setAttribute("name", "STRING");
        field2.textContent = this.Value;


        //add fields to block
        propertyblock.appendChild(field0);
        propertyblock.appendChild(field1);
        propertyblock.appendChild(field2);

        //append block
        property.appendChild(propertyblock);
        return property;
    }
}