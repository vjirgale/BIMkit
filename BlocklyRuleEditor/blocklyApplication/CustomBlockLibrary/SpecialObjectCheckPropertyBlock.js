
Blockly.Blocks['specialproperty'] = {
    init: function() {
      this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown(PropertyNegationDropdown), "NEGATION")
        .appendField("(")
        .appendField(new Blockly.FieldDropdown(propertyBooleanMethods), "ATTACHMENT")
        .appendField(new Blockly.FieldDropdown(LogicalOperatorDropdown), "LogicalOperator")
        .appendField(new Blockly.FieldDropdown(propertyBooleanMethods), "ATTACHMENT")
        .appendField(")");
      this.setOutput(true, "SPECIAL_PROPERTY");
      this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };