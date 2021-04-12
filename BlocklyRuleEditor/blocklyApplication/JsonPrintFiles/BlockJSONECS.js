//update the ecs of a rule object
//paramters: id = id of ruleblock, jsonRuleObject = rule object
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
        return 'missing';
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
}

class PropertyCheckString
{
    constructor(value, operation, name){
        this.$type = "RulesPackage.PropertyCheckString, RulesPackage";
        this.Value = value;
        this.Operation = operation;
        this.Name = name;
    }
}