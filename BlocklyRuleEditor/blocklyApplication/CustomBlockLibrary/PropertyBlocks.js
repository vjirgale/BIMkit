//numeric property dimension block definition
Blockly.Blocks['propertydimension'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown(propertyDistanceMethods), "DIMENSION")
          .appendField(new Blockly.FieldDropdown(OperatorNumDropdown), "SIGN")
          .appendField(new Blockly.FieldNumber(0, 0), "VALUE")
          .appendField(new Blockly.FieldDropdown(UnitDropdown), "UNIT");
      this.setOutput(true, "PROPERTY");
      this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };
  //bool property attachments block definition
  Blockly.Blocks['propertyattachments'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown(PropertyNegationDropdown), "NEGATION")
          .appendField(new Blockly.FieldDropdown(propertyBooleanMethods), "ATTACHMENT");
      this.setOutput(true, "PROPERTY");
      this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };
  //string property string block definition
  Blockly.Blocks['propertystring'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown(propertyStringMethods), "FUNCTION")
          .appendField(new Blockly.FieldDropdown(OperatorStringDropdown), "SIGN")
          .appendField(new Blockly.FieldTextInput(" "), "STRING");
      this.setOutput(true, "PROPERTY");
      this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };