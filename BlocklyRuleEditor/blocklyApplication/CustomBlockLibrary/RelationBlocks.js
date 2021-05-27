//==============================================RelationTypes====================================================

//initalize relation block methods
let relationBooleanMethods = [["Pending...", "Pending..."],["Next to","Next to"],["Above","Above"]];
let relationDistanceMethod = [["Pending...","Pending..."], ["Center Distance","Center Distance"], ["Center DistanceXY","Center DistanceXY"], ["BoundingBoxDistance","Bounding Box"]];
let relationStringMethods  = [["Pending...", "Pending..."],["FunctionOfObj", "FunctionOfObj"]];
fetchRelationMethods();


//block definitions for relations
Blockly.Blocks['relationboolean'] = {
    init: function() {
      this.appendDummyInput()
          .appendField(new Blockly.FieldDropdown([["Must be","MUST_HAVE"], ["Must not be","MUST_NOT_HAVE"]]), "NEGATION")
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
          .appendField(new Blockly.FieldDropdown([["=",OperatorNum.EQUAL], ["<",OperatorNum.LESS_THAN], ["<=",OperatorNum.LESS_THAN_OR_EQUAL], [">",OperatorNum.GREATER_THAN], [">=",OperatorNum.GREATER_THAN_OR_EQUAL]]), "SIGN")
          .appendField(new Blockly.FieldNumber(0), "VALUE")
          .appendField(new Blockly.FieldDropdown([["MM", UnitEnums.MM], ["CM", UnitEnums.CM], ["M",UnitEnums.M], ["INCH",UnitEnums.INCH], ["FT",UnitEnums.FT], ["DEG",UnitEnums.DEG], ["RAD",UnitEnums.RAD]]), "UNIT")
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
          .appendField(new Blockly.FieldDropdown([["=", OperatorString.EQUAL], ["!=", OperatorString.NOT_EQUAL], ["Contains",OperatorString.CONTAINS]]), "SIGN")
          .appendField(new Blockly.FieldTextInput(""), "STRING");
      this.setInputsInline(false);
      this.setOutput(true, "RELATION");
      this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
   this.setTooltip("");
   this.setHelpUrl("");
    }
  };