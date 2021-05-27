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
    alert("Error, invalid property type.");
    return 'ERROR';
}

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
        this.Value = operation == Negation.MUST_HAVE;
        this.Operation = operation;
        this.Name = name;
        this.PCType = "BOOL";
    }
    getXML(index){
        //ecs element
        var property = document.createElementNS(XML_NS, 'value');
        if(index == -1){
            property.setAttribute("name", "Relation");
        }
        else if(index == 0){
            property.setAttribute("name", "PROPERTYBLOCK");
        }else{
            property.setAttribute("name", "Property"+index);
        }
        
        var propertyblock = document.createElementNS(XML_NS, 'block');
        propertyblock.setAttribute("type",  index==-1?"relationboolean":"propertyattachments");
        //fields
        //create element for negation i.e. Has, Has not
        var field0 = document.createElementNS(XML_NS, 'field');
        field0.setAttribute("name", "NEGATION");
        field0.textContent = this.Operation;
        ////create element for attachment i.e. door
        var field1 = document.createElementNS(XML_NS, 'field');
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
        
        this.Operation = operation;
        this.Value = value;
        this.ValueInStandardUnit = ValueInStandardUnit;
        this.ValueUnit = ValueUnit;
        this.Name = name;
        this.PCType = "NUM";
    }
    getXML(index){
        //ecs element
        var property = document.createElementNS(XML_NS, 'value');
        if(index == -1){
            property.setAttribute("name", "Relation");
        }
        else if(index == 0){
            property.setAttribute("name", "PROPERTYBLOCK");
        }else{
            property.setAttribute("name", "Property"+index);
        }
        var propertyblock = document.createElementNS(XML_NS, 'block');
        propertyblock.setAttribute("type", index==-1?"relationnumeric":"propertydimension");
        //fields

        //create element for dimension i.e width
        var field0 = document.createElementNS(XML_NS, 'field');
        field0.setAttribute("name", index==-1?"DistanceType":"DIMENSION");
        field0.textContent = this.Name;
        //create element for sign i.e. =, <, > etc.
        var field1 = document.createElementNS(XML_NS, 'field');
        field1.setAttribute("name", "SIGN");
        field1.textContent = this.Operation;
        //create element for numeric value
        var field2 = document.createElementNS(XML_NS, 'field');
        field2.setAttribute("name", "VALUE");
        field2.textContent = this.Value;
        //create element for unit of value
        var field3 = document.createElementNS(XML_NS, 'field');
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
        this.Value = value;
        this.Operation = operation;
        this.Name = name;
        this.PCType = "STRING";
    }
    getXML(index){
        //ecs element
        var property = document.createElementNS(XML_NS, 'value');
        if(index == -1){
            property.setAttribute("name", "Relation");
        }
        else if(index == 0){
            property.setAttribute("name", "PROPERTYBLOCK");
        }else{
            property.setAttribute("name", "Property"+index);
        }
        var propertyblock = document.createElementNS(XML_NS, 'block');
        propertyblock.setAttribute("type", index==-1?"relationstring":"propertystring");
        //fields
        //create element for function
        var field0 = document.createElementNS(XML_NS, 'field');
        field0.setAttribute("name", "FUNCTION");
        field0.textContent = this.Name;
        //create element for operation i.e. =, <, > etc.
        var field1 = document.createElementNS(XML_NS, 'field');
        field1.setAttribute("name", "SIGN");
        field1.textContent = this.Operation;
        ////create element for string
        var field2 = document.createElementNS(XML_NS, 'field');
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