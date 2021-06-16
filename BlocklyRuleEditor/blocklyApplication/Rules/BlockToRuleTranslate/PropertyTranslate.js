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


function getPropertyXML(property, index){
    switch (property.PCType){
        case "BOOL":
            return getBoolPropertyXML(property, index);
        case "NUM":
            return getNumericPropertyXML(property, index);
        case "STRING":
            return getStringPropertyXML(property, index);
        default:
            return NaN;
    }
}

function getBoolPropertyXML(property, index){
    //ecs element
    var propertyXML = document.createElementNS(XML_NS, 'value');
    if(index == -1){
        propertyXML.setAttribute("name", "Relation");
    }
    else if(index == 0){
        propertyXML.setAttribute("name", "PROPERTYBLOCK");
    }else{
        propertyXML.setAttribute("name", "Property"+index);
    }
    
    var propertyblock = document.createElementNS(XML_NS, 'block');
    propertyblock.setAttribute("type",  index==-1?"relationboolean":"propertyattachments");
    //fields
    //create element for negation i.e. Has, Has not
    var field0 = document.createElementNS(XML_NS, 'field');
    field0.setAttribute("name", "NEGATION");
    field0.textContent = property.Operation;
    ////create element for attachment i.e. door
    var field1 = document.createElementNS(XML_NS, 'field');
    field1.setAttribute("name", "ATTACHMENT");
    field1.textContent = property.Name;

    //add fields to block
    propertyblock.appendChild(field0);
    propertyblock.appendChild(field1);

    //append block
    propertyXML.appendChild(propertyblock);
    return propertyXML;
}

function getNumericPropertyXML(property, index){
    //ecs element
    var propertyXML = document.createElementNS(XML_NS, 'value');
    if(index == -1){
        propertyXML.setAttribute("name", "Relation");
    }
    else if(index == 0){
        propertyXML.setAttribute("name", "PROPERTYBLOCK");
    }else{
        propertyXML.setAttribute("name", "Property"+index);
    }
    var propertyblock = document.createElementNS(XML_NS, 'block');
    propertyblock.setAttribute("type", index==-1?"relationnumeric":"propertydimension");
    //fields

    //create element for dimension i.e width
    var field0 = document.createElementNS(XML_NS, 'field');
    field0.setAttribute("name", index==-1?"DistanceType":"DIMENSION");
    field0.textContent = property.Name;
    //create element for sign i.e. =, <, > etc.
    var field1 = document.createElementNS(XML_NS, 'field');
    field1.setAttribute("name", "SIGN");
    field1.textContent = property.Operation;
    //create element for numeric value
    var field2 = document.createElementNS(XML_NS, 'field');
    field2.setAttribute("name", "VALUE");
    field2.textContent = property.Value;
    //create element for unit of value
    var field3 = document.createElementNS(XML_NS, 'field');
    field3.setAttribute("name", "UNIT");
    field3.textContent = property.ValueUnit;

    //add fields to block
    propertyblock.appendChild(field0);
    propertyblock.appendChild(field1);
    propertyblock.appendChild(field2);
    propertyblock.appendChild(field3);

    //append block
    propertyXML.appendChild(propertyblock);
    return propertyXML;
}

function getStringPropertyXML(property, index){
    //ecs element
    var propertyXML = document.createElementNS(XML_NS, 'value');
    if(index == -1){
        propertyXML.setAttribute("name", "Relation");
    }
    else if(index == 0){
        propertyXML.setAttribute("name", "PROPERTYBLOCK");
    }else{
        propertyXML.setAttribute("name", "Property"+index);
    }
    var propertyblock = document.createElementNS(XML_NS, 'block');
    propertyblock.setAttribute("type", index==-1?"relationstring":"propertystring");
    //fields
    //create element for function
    var field0 = document.createElementNS(XML_NS, 'field');
    field0.setAttribute("name", "FUNCTION");
    field0.textContent = property.Name;
    //create element for operation i.e. =, <, > etc.
    var field1 = document.createElementNS(XML_NS, 'field');
    field1.setAttribute("name", "SIGN");
    field1.textContent = property.Operation;
    ////create element for string
    var field2 = document.createElementNS(XML_NS, 'field');
    field2.setAttribute("name", "STRING");
    field2.textContent = property.Value;


    //add fields to block
    propertyblock.appendChild(field0);
    propertyblock.appendChild(field1);
    propertyblock.appendChild(field2);

    //append block
    propertyXML.appendChild(propertyblock);
    return propertyXML;
}