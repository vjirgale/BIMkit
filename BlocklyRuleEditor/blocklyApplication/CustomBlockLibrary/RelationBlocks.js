//block definitions for relations
Blockly.Blocks['relationboolean'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown(RelationNegationDropdown), "NEGATION")
          .appendField(new Blockly.FieldDropdown(relationBooleanMethods), "ATTACHMENT");
      this.setInputsInline(false);
      this.setOutput(true, "RELATION");
      this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };
  
  Blockly.Blocks['relationnumeric'] = {
    init: function() {
      this.appendDummyInput()
          .appendField("has")
          .appendField(new Blockly.FieldDropdown(relationDistanceMethod), "DistanceType")
          .appendField(new Blockly.FieldDropdown(OperatorNumDropdown), "SIGN")
          .appendField(new Blockly.FieldNumber(0), "VALUE")
          .appendField(new Blockly.FieldDropdown(UnitDropdown), "UNIT")
          .appendField("to");
      this.setInputsInline(false);
      this.setOutput(true, "RELATION");
      this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };
  
  Blockly.Blocks['relationstring'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown(relationStringMethods), "FUNCTION")
          .appendField(new Blockly.FieldDropdown(OperatorStringDropdown), "SIGN")
          .appendField(new Blockly.FieldTextInput(""), "STRING");
      this.setInputsInline(false);
      this.setOutput(true, "RELATION");
      this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };