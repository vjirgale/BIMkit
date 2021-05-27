//==============================================Property Blocks====================================================
//numeric property dimension block definition
Blockly.Blocks['propertydimension'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown(propertyDistanceMethods), "DIMENSION")
          .appendField(new Blockly.FieldDropdown([["=",OperatorNum.EQUAL], ["<",OperatorNum.LESS_THAN], ["<=",OperatorNum.LESS_THAN_OR_EQUAL], [">",OperatorNum.GREATER_THAN], [">=",OperatorNum.GREATER_THAN_OR_EQUAL]]), "SIGN")
          .appendField(new Blockly.FieldNumber(0, 0), "VALUE")
          .appendField(new Blockly.FieldDropdown([["MM", UnitEnums.MM], ["CM", UnitEnums.CM], ["M",UnitEnums.M], ["INCH",UnitEnums.INCH], ["FT",UnitEnums.FT], ["DEG",UnitEnums.DEG], ["RAD",UnitEnums.RAD]]), "UNIT");
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
          .appendField(new Blockly.FieldDropdown([["Has",Negation.MUST_HAVE], ["Does not Have",Negation.MUST_NOT_HAVE]]), "NEGATION")
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
          .appendField(new Blockly.FieldDropdown([["=", OperatorString.EQUAL], ["!=", OperatorString.NOT_EQUAL], ["Contains",OperatorString.CONTAINS]]), "SIGN")
          .appendField(new Blockly.FieldTextInput(" "), "STRING");
      this.setOutput(true, "PROPERTY");
      this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };