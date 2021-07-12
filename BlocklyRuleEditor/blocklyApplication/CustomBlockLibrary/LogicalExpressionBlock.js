//==============================================LogicalExpression====================================================
//Logical Expression block definition
Blockly.Blocks['logicalexpression'] = {
    init: function() {
      this.appendStatementInput("LOGICALEXPRESSION")
          .setCheck(['relationstruct','logicalEX'])
          .appendField(new Blockly.FieldDropdown(LogicalOperatorDropdown), "LOGICAL_OPERATOR");
      this.setPreviousStatement(true, ['relationstruct','logicalEX']);
      this.setNextStatement(true, ['relationstruct','logicalEX']);
      this.setColour('%{BKY_LOGICALEXPRESSION_COLOUR}');
      this.setTooltip("");
      this.setHelpUrl("");
    }
  };