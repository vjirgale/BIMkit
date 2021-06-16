

//UnitEnums = {MM:'0', CM:'1', M:'2', INCH:'3', FT:'4', DEG:'5', RAD:'6'}
//takes in a numeric value and a unit and returns the value in standard value
function ValueToStandardValue(value, unit){
    switch (unit){
        case "MM":
            return value * 0.001;
        case "CM":
            return value * 0.01;
        case "M":
            return value;
        case "INCH":
            return value * 0.0254;
        case "FT":
            return value * 0.3048;
        case ("DEG"):
            return value * Math.PI / 180;
        case "RAD":
            return value;
        default:
            return NaN;
    }
}



class PropertyCheckBool
{
    constructor(operation, name){
        this.Value = operation == PropertyNegation.MUST_HAVE;
        this.Operation = operation;
        this.Name = name;
        this.PCType = "BOOL";
    }
    
}

class PropertyCheckNumeric
{
    constructor(operation, value, ValueInStandardUnit, ValueUnit, name){
        
        this.Operation = operation;
        this.Value = value;
        this.ValueInStandardUnit = ValueInStandardUnit;
        this.ValueUnit = ValueUnit;
        this.Name = name;
        this.PCType = "NUM";
    }
}

class PropertyCheckString
{
    constructor(value, operation, name){
        this.Value = value;
        this.Operation = operation;
        this.Name = name;
        this.PCType = "STRING";
    }

}

