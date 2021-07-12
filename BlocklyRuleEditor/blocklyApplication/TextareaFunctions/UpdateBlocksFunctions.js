/*This file contains functions used for keeping the dropdowns of object check blocks correct*/


//dictionary for storing list of ecs names to a rule ID 
let RuleID_ECSList_Dictionary = {};

//rerenders dropdowns of all objectcheck and relationcheck blocks attached to ruleblock
function reRenderAllDescendants(ruleblock){
  //get all child blocks
  var descendants = ruleblock.getDescendants();
  //rerender all object checks
  descendants.forEach(element => {
    if(element.type == 'objectcheck'){
      //selected dropdown option
      var selected = element.getFieldValue('Object');

      //if the blocks selected dropdown option doesn't exists
      if(selected != '0' && parseInt(selected.slice(3,selected.length)) > ruleblock.Count){
        //set block selected object to the default "select..."
        element.getField('Object').setValue('0');
      }
      //refreshs the dropdown so that the updated changes are visible
      refreshDynamicDropdownField(element, 'Object');
    }
    //rerenders all relation checks
    else if(element.type == 'relationcheck'){
      var selected = element.getFieldValue('Object1');
      //if the blocks selected dropdown option doesn't exists
      if(selected != '0' && parseInt(selected.slice(3,selected.length)) > ruleblock.Count){
        //set block selected object to the default "select..."
        element.getField('Object1').setValue('0');
      }
      //refreshs the dropdown so that the updated changes are visible
      refreshDynamicDropdownField(element, 'Object1');

      var selected = element.getFieldValue('Object2');
      //if the blocks selected dropdown option doesn't exists
      if(selected != '0' && parseInt(selected.slice(3,selected.length)) > ruleblock.Count){
        //set block selected object to the default "select..."
        element.getField('Object2').setValue('0');
      }
      //refreshs the dropdown so that the updated changes are visible
      refreshDynamicDropdownField(element, 'Object2');
    }
  });

}


//updates all objectcheck and relationcheck blocks attached to ruleblock that have been moved from another ruleblock
function resetAllMoved(ruleBlock){
  var descendants = ruleBlock.getDescendants();
  //iterates through all blocks moved
  descendants.forEach(element => {
    //checks if an objectcheck was moved to a different rule block
    if(element.type == 'objectcheck' && element.ruleID != ruleBlock.id){
      //resets dropdown
      element.getField('Object').setValue('0');
      //refreshs the dropdown so that the updated changes are visible
      refreshDynamicDropdownField(element, 'Object');
      //updates to new ruleblock id
      element.ruleID = ruleBlock.id;
    }
    //checks if an relationcheck was moved to a different rule block
    else if(element.type == 'relationcheck' && element.ruleID != ruleBlock.id){
      //updates to new ruleblock id
      element.ruleID = ruleBlock.id;

       //reset firstdropdown
      element.getField('Object1').setValue('0');
      //refreshs the dropdown so that the updated changes are visible
      refreshDynamicDropdownField(element, 'Object1');
      
      //reset second dropdown
      element.getField('Object2').setValue('0');
      //refreshs the dropdown so that the updated changes are visible
      refreshDynamicDropdownField(element, 'Object2');
    }
  });
}

//refreshes the rendered field (i.e. rerenders a check blocks selected name when the name is changed)
function refreshDynamicDropdownField(block, fieldName) {
  const field = block.getField(fieldName)
  if (field) {
    field.getOptions(false)
    // work around for https://github.com/google/blockly/issues/3553
    field.doValueUpdate_(field.getValue())
    field.forceRerender()
  }
}

//returns current ecs names list of a ruleblock
function updateECSList(ruleBlock){
  var options = [['select...','0']];
  //iterate throguh all ecs name fields in the ruleblock
  for(var i = 1; i <= ruleBlock.getFieldValue('CountECS'); i++){
    //add name to options array
    options.push([ruleBlock.getFieldValue('VarName'+i), i.toString()])
  }
  return options;
}

//function to set the rule ID of all check blocks when a ruleset is imported
function setCheckBlockRuleIDs(ruleBlock){
  var childBlocks = ruleBlock.getDescendants(true);
  //iterates over every check block in a rule block and sets the ruleBlockID.
  childBlocks.forEach(element => {
    if(element.type == "objectcheck"){
      element.ruleID = ruleBlock.id;
    }
    else if(element.type == "relationcheck"){
      element.ruleID = ruleBlock.id;
    }
  });
}