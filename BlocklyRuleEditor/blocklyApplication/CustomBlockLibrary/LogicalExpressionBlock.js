//==============================================LogicalExpression====================================================
//Logical Expression block definition
Blockly.Blocks['logicalexpression'] = {
    init: function() {
      this.appendStatementInput("LOGICALEXPRESSION")
          .setCheck(['relationstruct','logicalEX'])
          .appendField(new Blockly.FieldDropdown([["AND",LogicalOperator.AND], ["OR",LogicalOperator.OR], ["XOR",LogicalOperator.XOR]]), "LOGICAL_OPERATOR");
      this.setPreviousStatement(true, ['relationstruct','logicalEX']);
      this.setNextStatement(true, ['relationstruct','logicalEX']);
      this.setColour(90);
      this.setTooltip("");
      this.setHelpUrl("");
      //change color depending on value selected (AND, OR, XOR)
      this.setOnChange(function(changeEvent) {
        var colourValue = this.colour_;
        if(this.getFieldValue("Expression") == LogicalOperator.AND){
          colourValue = '%{BKY_AND_COLOUR}';
        }
        else if(this.getFieldValue("Expression") == LogicalOperator.OR){
          colourValue = '%{BKY_OR_COLOUR}';
        }
        else{
          colourValue = '%{BKY_XOR_COLOUR}';
        }
        this.setColour(colourValue);
      });
    }
  };