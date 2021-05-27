//returns and creates a new json relationcheck object
//paramters: block = relation check block, jsonRuleObject = rule object
function NewRelationCheck(block, jsonRuleObject,ruleid){
    //first dropdown
    var dropdown_object1 = block.getFieldValue('Object1');
    var index1;
    //set index1
    //if dropdown has default selected ('select...')
    if(dropdown_object1 == '0'){
        index1 = "missing";
        jsonRuleObject.valid = false;
    }
    else{
        //all options in the dropdown are referenced by a string: 'ecs1' or 'ecs2' or 'ecs3' etc.
        //gets the index of the object by parsing the string of the selected by removing the first three characters 'ecs' and then converting the rest to an int.
        index1 = RuleID_ECSList_Dictionary[ruleid][dropdown_object1][0];
    }

    //second dropdown
    var dropdown_object2 = block.getFieldValue('Object2');
    var index2;
    //set index2
    //if dropdown has default selected ('select...')
    if(dropdown_object2 == '0'){
        index2 = "missing";
        jsonRuleObject.valid = false;
    }
    else{
        //all options in the dropdown are referenced by a string: 'ecs1' or 'ecs2' or 'ecs3' etc.
        //gets the index of the object by parsing the string of the selected by removing the first three characters 'ecs' and then converting the rest to an int.
        index2 = RuleID_ECSList_Dictionary[ruleid][dropdown_object2][0];
    }

    //relation block input
    var ChildBlock = block.getChildren(true)[0];
    var PropertyCheck = {};

    //create relation json object
    if(ChildBlock != null){
        if(ChildBlock.type == "relationboolean"){
            var operation = ChildBlock.getFieldValue('NEGATION');
            var name = ChildBlock.getFieldValue('ATTACHMENT');

            PropertyCheck = new PropertyCheckBool(operation, name);
        }
        else if(ChildBlock.type == "relationnumeric"){
            var operation = ChildBlock.getFieldValue('SIGN');
            var value = ChildBlock.getFieldValue('VALUE');
            var ValueUnit = ChildBlock.getFieldValue('UNIT');
            var name = ChildBlock.getFieldValue('DistanceType');

            var standardValue = ValueToStandardValue(value, ValueUnit);
            PropertyCheck = new PropertyCheckNumeric(operation, value, standardValue,ValueUnit, name);
        }
        else if(ChildBlock.type == "relationstring"){
            var value = ChildBlock.getFieldValue('STRING');
            var operation = ChildBlock.getFieldValue('SIGN');
            var name = ChildBlock.getFieldValue('FUNCTION');

            PropertyCheck = new PropertyCheckString(value, operation, name);
        }
        else{
            PropertyCheck = "missing";
        }
    }
    else{
        jsonRuleObject.valid = false;
        PropertyCheck = "missing";
    }

    return new DoubleRelation(index1, index2, PropertyCheck);
}

class DoubleRelation
{
    constructor(index1, index2, propertyCheck){
        this.Obj1Name = index1;
        this.Obj2Name = index2;
        this.Negation = "MUST_HAVE";
        this.PropertyCheck = propertyCheck;
    }
    getXML(){
        //object check block
        var objectcheckblock = document.createElementNS(XML_NS, 'block');
        objectcheckblock.setAttribute("type", "relationcheck");
        //fields

        var field0 = document.createElementNS(XML_NS, 'field');
        field0.setAttribute("name", "Object1");
        field0.textContent = (this.Obj1Name in KeyToIndex)? KeyToIndex[this.Obj1Name]: "0";

        var field1 = document.createElementNS(XML_NS, 'field');
        field1.setAttribute("name", "Object2");
        field1.textContent = (this.Obj1Name in KeyToIndex)? KeyToIndex[this.Obj1Name]: "0";

        //add fields to block
        objectcheckblock.appendChild(field0);
        objectcheckblock.appendChild(field1);

        //if has proerpty check add property check element
        if(this.PropertyCheck != "missing"){
            var propertyXML = this.PropertyCheck.getXML(-1);
            objectcheckblock.appendChild(propertyXML);
        }

        return objectcheckblock;
    }
}