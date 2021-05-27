//==============================================CheckBlocks====================================================
//is the default options for a dropdown of a relationcheck/objectcheck block. i.e. when they are not attached to a ruleblock)
var DefaultOptions = [['select...','0']];
//objectchecks block definition
Blockly.Blocks['objectcheck'] = {
  init: function() {
    this.ruleID = null;
    this.appendValueInput("PROPERTYBLOCK")
        .setCheck('PROPERTY')
        //dynamically generate dropdown options
        .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object")
        .appendField(new Blockly.FieldDropdown([["Must Have","MUST_HAVE"], ["Must Not Have","MUST_NOT_HAVE"]]), "Negation")
        .appendField(" Property:");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour('%{BKY_CHECKBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
 //set warning if dropdown is not selected or property input is not filled.
 this.setOnChange(function(changeEvent) {
   //if the property input is not filled
  if (!this.getInputTargetBlock('PROPERTYBLOCK')) {
    this.setWarningText('All Object Checks must have a Property block as input.');
    return;
  } 
  //if the dropdown is not seleceted
  if(this.getFieldValue('Object') == '0'){
    this.setWarningText('Must set dropdown to an object.');
    return;
  }
  this.setWarningText(null);
});
  },
  //dynamically generate options for dropdown
  generateOptions: function() {
    //set to default
    var options = DefaultOptions;

    //if the block exists
    if(this.getSourceBlock()){
      var rootBlock = this.getSourceBlock().getRootBlock();
      //if it is attached to a rule block  then set dropdown to the rule block's list of ecs names
      if(rootBlock.type == 'ruleblock'){
        options = RuleID_ECSList_Dictionary[rootBlock.id];
      }
    }
    return options;
  }
};