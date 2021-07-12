//relationchecks block definition
Blockly.Blocks['relationcheck'] = {
    init: function() {
      this.ruleID = null;
      this.appendDummyInput()
          //dynamically generate dropdown options
          .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object1");
      this.appendValueInput("Relation")
          .setCheck("RELATION");
      this.appendDummyInput()
          //dynamically generate dropdown options
          .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object2");
      this.setInputsInline(true);
      this.setPreviousStatement(true, null);
      this.setNextStatement(true, null);
      this.setColour('%{BKY_CHECKBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
   //set warning if a dropdown is not selected or relation input is not filled.
   this.setOnChange(function(changeEvent) {
     //if relation input is not filled
    if (!this.getInputTargetBlock('Relation')) {
      this.setWarningText('All Relation Checks must have a Relation block as input.');
      return;
    }
    //if dropdown is not selected
    if(this.getFieldValue('Object1') == '0'){
      this.setWarningText('Must set first dropdown to an object.');
      return;
    }
    //if dropdown is not selected
    if(this.getFieldValue('Object2') == '0'){
      this.setWarningText('Must set second dropdown to an object.');
      return;
    }
    this.setWarningText(null);
  });
    },
    //dynamically generate options for dropdown
    generateOptions: function() {
      var options = DefaultOptions;
      if(this.getSourceBlock()){
        var rootBlock = this.getSourceBlock().getRootBlock();
        //if it is attached to a rule block  then set dropdown to the rule block's list of ecs names
        if(rootBlock.type == 'ruleblock'){
          options = RuleID_ECSList_Dictionary[rootBlock.id];
        }
      }
      return options;
    },
};