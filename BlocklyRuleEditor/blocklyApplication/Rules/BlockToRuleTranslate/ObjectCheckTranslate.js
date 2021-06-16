//returns and creates a new json objectcheck object
//paramters: block = object check block, jsonRuleObject = rule object
function NewObjectCheck(block, jsonRuleObject, ruleid){
    //gets current selected object
    var dropdown_var = block.getFieldValue('Object');
    var index;
    
    //if it is the default dropdown ('select...')
    if(dropdown_var == '0'){
        index = "missing";
        jsonRuleObject.valid = false;
    }
    else{
        
        //all options in the dropdown are referenced by a string: 'ecs1' or 'ecs2' or 'ecs3' etc.
        //gets the index of the object by parsing the string of the selected by removing the first three characters 'ecs' and then converting the rest to an int.
        index = RuleID_ECSList_Dictionary[ruleid][dropdown_var][0];
    }

    //negation ie. has, does not have
    var negation = block.getFieldValue('Negation');

    //property input block
    var propertyBlock =  block.getInputTargetBlock('PROPERTYBLOCK');
    //creates property json object
    var property = NEW_Property(propertyBlock, jsonRuleObject);
    
    return new SingleRelation(index, negation, property);
}


function getObjectCheckXML(objectcheck){
    //object check block
    var objectcheckXML = document.createElementNS(XML_NS, 'block');
    objectcheckXML.setAttribute("type", "objectcheck");
    //fields

    var field0 = document.createElementNS(XML_NS, 'field');
    field0.setAttribute("name", "Object");
     //if the object index is a number than set it to that ecs index otherwise set it to default var (select...)
    field0.textContent = (objectcheck.ObjName in KeyToIndex)? KeyToIndex[objectcheck.ObjName]: "0";
    

    var field1 = document.createElementNS(XML_NS, 'field');
    field1.setAttribute("name", "Negation");
    field1.textContent = objectcheck.Negation;

    //add fields to block
    objectcheckXML.appendChild(field0);
    objectcheckXML.appendChild(field1);

    if(objectcheck.PropertyCheck != 'missing'){
        var propertyXML = getPropertyXML(objectcheck.PropertyCheck, 0);  //PropertyCheck.getXML(0);
        objectcheckXML.appendChild(propertyXML);
    }

    return objectcheckXML;
}

