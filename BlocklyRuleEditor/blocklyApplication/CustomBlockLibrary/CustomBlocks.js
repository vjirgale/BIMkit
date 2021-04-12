//Blockly Custom Blocks page: https://developers.google.com/blockly/guides/create-custom-blocks/overview

//Colours For Blocks
Blockly.Msg.RULEBLOCK_COLOUR = 250;
Blockly.Msg.ECSBLOCK_COLOUR = 350;
Blockly.Msg.PROPERTYBLOCK_COLOUR = 320;
Blockly.Msg.CHECKBLOCK_COLOUR = 290;
Blockly.Msg.RELATIONBLOCK_COLOUR = 230;

//Colours for Logical Expression
Blockly.Msg.OR_COLOUR = 100;
Blockly.Msg.AND_COLOUR = 300;
Blockly.Msg.XOR_COLOUR = 15;


//ENUMS
const ErrorLevel = {Recommended : '0', Warning : '1', Error : '2'}
const OccuranceRule = {ALL : 'ALL', ANY : 'ANY', NONE : 'NONE'}
const LogicalOperator = {AND : 'AND', OR : 'OR', XOR : 'XOR'}
const Negation = {MUST_HAVE : '0', MUST_NOT_HAVE : '1'}

const UnitEnums = {MM:'0', CM:'1', M:'2', INCH:'3', FT:'4', DEG:'5', RAD:'6'}
const OperatorNum = {GREATER_THAN : '0', GREATER_THAN_OR_EQUAL : '1', EQUAL : '2', LESS_THAN : '3', LESS_THAN_OR_EQUAL : '4', NOT_EQUAL : '5'}
const OperatorString = {EQUAL:'0', NOT_EQUAL:'1', CONTAINS:'2'}



//==============================================Rule Block====================================================
//rule block definition
Blockly.Blocks['ruleblock'] = {
  init: function(){
    //count to save previous ecs count
    this.Count = 0;
    this.jsonInit({
      "type": "newecs",
      "message0": "Rule Name: %1 %2 ErrorLevel: %3 %4 Description:  %5 %6 ECS COUNT  %7 %8 %9 Relations: %10 %11",
      "args0": [
        {
          "type": "field_input",
          "name": "RULENAME",
          "text": "name..."
        },
        {
          "type": "input_dummy"
        },
        {
          "type": "field_dropdown",
          "name": "ERRORLEVEL",
          "options": [
            [
              "recommended",
              ErrorLevel.Recommended
            ],
            [
              "warning",
              ErrorLevel.Warning
            ],
            [
              "error",
              ErrorLevel.Error
            ]
          ]
        },
        {
          "type": "input_dummy"
        },
        {
          "type": "field_input",
          "name": "DESCRIPTION",
          "text": "..."
        },
        {
          "type": "input_dummy"
        },
        {
          "type": "field_number",
          "name": "CountECS",
          "value": 0,
          "min": 0,
          "max": 100,
          "precision": 1
        },
        {
          "type": "input_dummy"
        },
        {
          "type": "input_dummy",
          "name": "SeperationSpace"
        },
        {
          "type": "field_dropdown",
          "name": "LOGICAL_OPERATOR",
          "options": [
            [
              "AND",
              LogicalOperator.AND
            ],
            [
              "OR",
              LogicalOperator.OR
            ],
            [
              "XOR",
              LogicalOperator.XOR
            ]
          ]
        },
        {
          "type": "input_statement",
          "name": "LOGICALEXPRESSION"
        }
      ],
      "inputsInline": false,
      "colour": '%{BKY_RULEBLOCK_COLOUR}',
      "tooltip": "",
      "extensions": ["ruleblock_warning"],
      "mutator": "ruleblock_ecs_mutator"
    });
    //edit the value to change the maxdisplaylength of the 2 text fields.
    this.getField("RULENAME").maxDisplayLength = 20;
    this.getField("DESCRIPTION").maxDisplayLength = 25;
  }
};

//rule block warning *when the ecs inputs are not filled*
Blockly.Extensions.register('ruleblock_warning', function() {
  //function called when the ruleblock is changed
  this.setOnChange(function(changeEvent) {
    //count = amount of ecs input fields
    var count = this.getFieldValue('CountECS');
    for(var i =1; i <= count;i++){
      //if an ecs input field is missing an input ecs block
      if(!this.getInputTargetBlock('ECS'+i)){
        this.setWarningText('All ECS variables must have an ECS block as input.');
        return;
      }
    }
    this.setWarningText(null);
  });
});

//mutator that creates input fields for ecs
Blockly.RULEBLOCK_ECS_MUTATOR_MIXIN = {

  mutationToDom: function() {
    var container = Blockly.utils.xml.createElement('mutation');
    var ecsCount = (this.getFieldValue('CountECS'));
    //save ecs count
    container.setAttribute('ecs_count', ecsCount);
    return container;
  },
  
  domToMutation: function(xmlElement) {
    //get ecs count
    var ecsCount = parseInt(xmlElement.getAttribute('ecs_count'), 10) || 0;
    //update ruleblock shape
    this.updateECSinputs(this.Count, ecsCount);
    this.Count = ecsCount;
  },

  //updates the amount of ecs input values in the rule block
  updateECSinputs: function(originalCount, newCount) {

    var valueConnections = [null];
    var ECSnames = [null];
    //list to store updated dropdown array of ecs names
    var Object_List = [['select...','var']];

    var i = 1;
    //iterate over every current ecs input field
    while (this.getInput('ECS' + i)) {
      //save name and connection
      var ECS_input = this.getInput('ECS' + i);
      var ECS_name = this.getFieldValue("VarName"+i);
      valueConnections.push(ECS_input.connection.targetConnection);
      ECSnames.push(ECS_name);
      i++;
    }

    //remove all inputs
    for(i = 1; i < originalCount+1; i++){
      this.removeInput('ECS' + i);
    }
    //create new inputs
    for(i = 1; i <= newCount; i++){
      //if input field should have name restored
      if(i <= originalCount){
        this.appendValueInput('ECS' + i)
          .setCheck('ECS')
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendField("Set")
          //restore name
          .appendField(new Blockly.FieldTextInput(ECSnames[i]), "VarName"+i)
          .appendField("to:");
        this.getField("VarName"+i).maxDisplayLength = 20;
        //add original name to ecs dropdown array
        Object_List.push([ECSnames[i],'ecs'+i]);
      }
      else{
        this.appendValueInput('ECS' + i)
          .setCheck('ECS')
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendField("Set")
          .appendField(new Blockly.FieldTextInput("object..."), "VarName"+i)
          .appendField("to:");
        this.getField("VarName"+i).maxDisplayLength = 20;
        //add new/default name to ecs dropdown array
        Object_List.push(["object...",'ecs'+i]);
      }
      //move input field in ruleblock so it comes before relations
      this.moveInputBefore('ECS'+i, 'SeperationSpace');
      //add a validator. THe kitchen designer must name the objects starting with a lowercase letter.
      this.getField("VarName"+i).setValidator(function(option) {
        //while the first character is not a letter
        while(option.length > 0 && !option[0].match(/[a-z]/i)){
          //delete character
          option = option.substring(1);
        }
        //change first character to lowercase
        if(option.length > 0 && option[0] == option[0].toUpperCase()){
          option = option[0].toLowerCase() + option.slice(1);
        }
        
        return option;
      });
    }
    //reconnect input blocks
    if(newCount > originalCount){
      this.reconnectChildBlocks_(valueConnections, originalCount);
    }
    else if(newCount < originalCount){
      this.reconnectChildBlocks_(valueConnections, newCount);
    }
    //update in dictionary
    RuleID_ECSList_Dictionary[this.id] = Object_List;
  },
  reconnectChildBlocks_: function(valueConnections, originalCount) {
    for (var i = 0; i <= originalCount; i++) {
      Blockly.Mutator.reconnect(valueConnections[i], this, 'ECS' + i);
    }
  }
};

//updates block shape when ECSCOUNT changes
Blockly.RULEBLOCK_ECS_MUTATOR_EXTENSION = function() {
  //option = input ecs count
  this.getField('CountECS').setValidator(function(option) {
    //checks if ecs count has changed
    if(option != this.getSourceBlock().Count){
      //update ruleblock shape
      this.getSourceBlock().updateECSinputs(this.getSourceBlock().Count, option);
      this.getSourceBlock().Count = option;
    }
  });
};
//register mutator SEE mutator page: https://developers.google.com/blockly/guides/create-custom-blocks/extensions
Blockly.Extensions.registerMutator('ruleblock_ecs_mutator',
    Blockly.RULEBLOCK_ECS_MUTATOR_MIXIN,
    Blockly.RULEBLOCK_ECS_MUTATOR_EXTENSION);




//==============================================ECS Block====================================================
//ECS block definition
Blockly.Blocks['ecsblock'] = {
  init: function() {
    //count to save previous property count
    this.Count = 0;
    this.jsonInit({
      "type": "ecsblock",
      "message0": "%1 %2  With %3 Properties: ",
      "args0": [
        {
          "type": "field_dropdown",
          "name": "ObjectSelection",
          "options": [
            [
              "ALL",
              OccuranceRule.ALL
            ],
            [
              "ANY",
              OccuranceRule.ANY
            ],
            [
              "NONE",
              OccuranceRule.NONE
            ]
          ]
        },
        {
          "type": "field_dropdown",
          "name": "TypeOfObject",
          "options": EcsOptions
        },
        {
          "type": "field_number",
          "name": "PropertyCount",
          "value": 0,
          "min": 0
        }
      ],
      "inputsInline": true,
      "output": "ECS",
      "colour": '%{BKY_ECSBLOCK_COLOUR}',
      "tooltip": "",
      "extensions": ["warning_on_change_ECS"],
      "mutator": "property_mutator"
    });
  }
};
//ecs block warning *when the properties are not filled*
Blockly.Extensions.register('warning_on_change_ECS', function() {
  this.setOnChange(function(changeEvent) {
    //count of property input fields
    var count = this.getFieldValue('PropertyCount')
    for(var i =1; i <= count;i++){
      //if a property input field is not filled with a propety block
      if(!this.getInputTargetBlock('Property'+i)){
        this.setWarningText('All Properties must have a Property block as input.');
        return;
      }
    }
    this.setWarningText(null);

  });
});

//mutator that creates inputs for properties
Blockly.PROPERTY_MUTATOR_MIXIN = {

  mutationToDom: function() {
    //save property count
    var container = Blockly.utils.xml.createElement('mutation');
    var PropertyCount = (this.getFieldValue('PropertyCount'));
    container.setAttribute('PropertyCount', PropertyCount);
    return container;
  },
  domToMutation: function(xmlElement) {
    //get property count
    var PropertyCount = parseInt(xmlElement.getAttribute('PropertyCount'), 10) || 0;
    //update ecs block shape
    this.updatePropertyInputs(this.Count, PropertyCount);
    this.Count = PropertyCount;
  },

  updatePropertyInputs: function(oldPropertyCount, newPropertyCount) {
    var valueConnections = [null];

    var i = 1;
    //iterate over every property input fields
    while (this.getInput('Property' + i)) {
      var ECS_input = this.getInput('Property' + i);
      //save connection
      valueConnections.push(ECS_input.connection.targetConnection);    
      i++;
    }
    
    if(this.getInput('brackets')){
      this.removeInput('brackets');
    }
    //remove all property input fields
    for(i = 1; i < oldPropertyCount+1; i++){
      this.removeInput('Property' + i);
    }

    if(newPropertyCount == 0){
      this.appendDummyInput('brackets')
        .appendField('[ ]');
    }

    //create new property input fields
    for(i = 1; i < newPropertyCount+1; i++){
      if(i != 1){
        this.appendValueInput('Property' + i)
        .setCheck('PROPERTY')
        .appendField(',');
      }
      else{
        this.appendValueInput('Property' + i)
        .setCheck('PROPERTY')
        .appendField('[');
      }
      if(i == newPropertyCount){
        this.appendDummyInput('brackets')
        .appendField(']');
      }
    }
    //restore connections
    if(newPropertyCount > oldPropertyCount){
      this.reconnectChildBlocks_(valueConnections, oldPropertyCount);
    }
    else if(newPropertyCount < oldPropertyCount){
      this.reconnectChildBlocks_(valueConnections, newPropertyCount);
    }
  },
  reconnectChildBlocks_: function(valueConnections, originalCount) {
    for (var i = 0; i <= originalCount; i++) {
      Blockly.Mutator.reconnect(valueConnections[i], this, 'Property' + i);
    }
  }
};
//updates ecs shape when property count changes
Blockly.PROPERTY_MUTATOR_EXTENSION = function() {
  //option = new property count
  this.getField('PropertyCount').setValidator(function(option) {
    //if the count has changed
    if(this.getSourceBlock().Count != option){
      //update ecs block shape
      this.getSourceBlock().updatePropertyInputs(this.getSourceBlock().Count, option);
      this.getSourceBlock().Count = option;
    }
    
  });
};

Blockly.Extensions.registerMutator('property_mutator',
    Blockly.PROPERTY_MUTATOR_MIXIN,
    Blockly.PROPERTY_MUTATOR_EXTENSION);



//==============================================Property Blocks====================================================
//property dimension block definition
Blockly.Blocks['propertydimension'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown(PropertyDimensions), "DIMENSION")
        .appendField(new Blockly.FieldDropdown([["=",OperatorNum.EQUAL], ["<",OperatorNum.LESS_THAN], ["<=",OperatorNum.LESS_THAN_OR_EQUAL], [">",OperatorNum.GREATER_THAN], [">=",OperatorNum.GREATER_THAN_OR_EQUAL]]), "SIGN")
        .appendField(new Blockly.FieldNumber(0, 0), "VALUE")
        .appendField(new Blockly.FieldDropdown([["MM", UnitEnums.MM], ["CM", UnitEnums.CM], ["M",UnitEnums.M], ["INCH",UnitEnums.INCH], ["FT",UnitEnums.FT], ["DEG",UnitEnums.DEG], ["RAD",UnitEnums.RAD]]), "UNIT");
    this.setOutput(true, "PROPERTY");
    this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};
//property attachments block definition
Blockly.Blocks['propertyattachments'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Has",Negation.MUST_HAVE], ["Does not Have",Negation.MUST_NOT_HAVE]]), "NEGATION")
        .appendField(new Blockly.FieldDropdown(PropertyAttachments), "ATTACHMENT");
    this.setOutput(true, "PROPERTY");
    this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};
//property string block definition
Blockly.Blocks['propertystring'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Function","Function"]]), "FUNCTION")
        .appendField(new Blockly.FieldDropdown([["=", OperatorString.EQUAL], ["!=", OperatorString.NOT_EQUAL], ["Contains",OperatorString.CONTAINS]]), "SIGN")
        .appendField(new Blockly.FieldTextInput(" "), "STRING");
    this.setOutput(true, "PROPERTY");
    this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};


//==============================================LogicalExpression====================================================
//Logical Expression block definition
Blockly.Blocks['logicalexpression'] = {
  init: function() {
    this.appendStatementInput("LogicalStatement")
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


//==============================================CheckBlocks====================================================
//is the default options for a dropdown of a relationcheck/objectcheck block. i.e. when they are not attached to a ruleblock)
var DefaultOptions = [['select...','var']];
//objectchecks block definition
Blockly.Blocks['objectcheck'] = {
  init: function() {
    this.ruleID = null;
    this.appendValueInput("PROPERTYBLOCK")
        .setCheck('PROPERTY')
        //dynamically generate dropdown options
        .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object")
        .appendField(new Blockly.FieldDropdown([["Must Have",Negation.MUST_HAVE], ["Must Not Have",Negation.MUST_NOT_HAVE]]), "Negation")
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
  if(this.getFieldValue('Object') == 'var'){
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
  if(this.getFieldValue('Object1') == 'var'){
    this.setWarningText('Must set first dropdown to an object.');
    return;
  }
  //if dropdown is not selected
  if(this.getFieldValue('Object2') == 'var'){
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


//==============================================RelationTypes====================================================
//block definitions for relations
Blockly.Blocks['relationboolean'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Must be",Negation.MUST_HAVE], ["Must not be",Negation.MUST_NOT_HAVE]]), "Negation")
        .appendField(new Blockly.FieldDropdown(newBooleanRelations), "BoolRelation");
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
        .appendField(new Blockly.FieldDropdown(newDistanceRelations), "DistanceType")
        .appendField(new Blockly.FieldDropdown([["=",OperatorNum.EQUAL], ["<",OperatorNum.LESS_THAN], ["<=",OperatorNum.LESS_THAN_OR_EQUAL], [">",OperatorNum.GREATER_THAN], [">=",OperatorNum.GREATER_THAN_OR_EQUAL]]), "check")
        .appendField(new Blockly.FieldNumber(0), "distance")
        .appendField(new Blockly.FieldDropdown([["MM", UnitEnums.MM], ["CM", UnitEnums.CM], ["M",UnitEnums.M], ["INCH",UnitEnums.INCH], ["FT",UnitEnums.FT], ["DEG",UnitEnums.DEG], ["RAD",UnitEnums.RAD]]), "MeasurementType")
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
        .appendField(new Blockly.FieldDropdown([["Function","Function"]]), "Type")
        .appendField(new Blockly.FieldDropdown([["=", OperatorString.EQUAL], ["!=", OperatorString.NOT_EQUAL], ["Contains",OperatorString.CONTAINS]]), "check")
        .appendField(new Blockly.FieldTextInput(""), "FunctionString");
    this.setInputsInline(false);
    this.setOutput(true, "RELATION");
    this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};


