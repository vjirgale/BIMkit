//Colours
Blockly.Msg.RULEBLOCK_COLOUR = 250;
Blockly.Msg.ECSBLOCK_COLOUR = 350;
Blockly.Msg.PROPERTYBLOCK_COLOUR = 320;
Blockly.Msg.CHECKBLOCK_COLOUR = 290;
Blockly.Msg.RELATIONBLOCK_COLOUR = 230;

// Define the colour
Blockly.Msg.OR_COLOUR = 100;
Blockly.Msg.AND_COLOUR = 300;
Blockly.Msg.XOR_COLOUR = 15;
//RuleBlock

Blockly.Blocks['newruleblock'] = {
  init: function(){
    this.ConstID;
    this.Count = 0;
    this.Object_List = [['select...','selectVar']];
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
              "error",
              "ERROR"
            ],
            [
              "warning",
              "WARNING"
            ],
            [
              "recommended",
              "RECOMMENDED"
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
          "max": 20,
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
              "AND"
            ],
            [
              "OR",
              "OR"
            ],
            [
              "XOR",
              "XOR"
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
      "extensions": ["warning_on_change"],
      "mutator": "math_is_divisibleby_mutator_new"
    });
  }
};
Blockly.Extensions.register('warning_on_change', function() {
  // Example validation upon block change:
  this.setOnChange(function(changeEvent) {
    var count = this.getFieldValue('CountECS')
    for(var i =1; i <= count;i++){
      if(!this.getInputTargetBlock('ECS'+i)){
        this.setWarningText('All ECS variables must be set to an ECS.');
        return;
      }
    }
    this.setWarningText(null);

  });
});
Blockly.Constants.Logic.IS_DIVISIBLEBY_MUTATOR_MIXIN_NEW = {
  /**
   * Create XML to represent whether the 'divisorInput' should be present.
   * @return {!Element} XML storage element.
   * @this {Blockly.Block}
   */
  mutationToDom: function() {
    var container = Blockly.utils.xml.createElement('mutation');
    var divisorInput = (this.getFieldValue('CountECS'));
    container.setAttribute('divisor_input', divisorInput);
    return container;
  },
  
  /**
   * Parse XML to restore the 'divisorInput'.
   * @param {!Element} xmlElement XML storage element.
   * @this {Blockly.Block}
   */
  domToMutation: function(xmlElement) {
    var divisorInput = parseInt(xmlElement.getAttribute('divisor_input'), 10) || 0;
    this.addnewOnes_(this.Count, divisorInput);
    this.Count = divisorInput;
    //RuleID_ECSList_Dictionary[this.ConstID] = this.Object_List;
  },
  /**
   * Modify this block to have (or not have) an input for 'is divisible by'.
   * @param {boolean} divisorInput True if this block has a divisor input.
   * @private
   * @this {Blockly.Block}
   */
  addnewOnes_: function(original, divisorInput) {

    var valueConnections = [null];
    var ECSnames = [null];
    this.Object_List = [['select...','var']];

    var i = 1;
    while (this.getInput('ECS' + i)) {
      var ECS_input = this.getInput('ECS' + i);
      var ECS_name = this.getFieldValue("VarName"+i);
      //var inputDo = this.getInput('DO' + i);
      valueConnections.push(ECS_input.connection.targetConnection);
      ECSnames.push(ECS_name);
      i++;
    }


    for(i = 1; i < original+1; i++){
      this.removeInput('ECS' + i);
    }
    for(i = 1; i < divisorInput+1; i++){
      if(i <= original){
        this.appendValueInput('ECS' + i)
          .setCheck('ECS')
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendField("Set")
          .appendField(new Blockly.FieldTextInput(ECSnames[i]), "VarName"+i)
          .appendField("to:");
        this.Object_List.push([ECSnames[i],'ecs'+i]);
      }
      else{
        this.appendValueInput('ECS' + i)
          .setCheck('ECS')
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendField("Set")
          .appendField(new Blockly.FieldTextInput("object..."), "VarName"+i)
          .appendField("to:");
          this.Object_List.push(["object...",'ecs'+i]);
      }
      this.moveInputBefore('ECS'+i, 'SeperationSpace');
      
    }
    if(divisorInput > original){
      this.reconnectChildBlocks_(valueConnections, original);
    }
    else if(divisorInput < original){
      this.reconnectChildBlocks_(valueConnections, divisorInput);
    }
  },
  reconnectChildBlocks_: function(valueConnections, originalCount) {
    for (var i = 0; i <= originalCount; i++) {
      Blockly.Mutator.reconnect(valueConnections[i], this, 'ECS' + i);
    }
  }
};

Blockly.Constants.Logic.IS_DIVISIBLE_MUTATOR_EXTENSION_NEW = function() {
  this.getField('CountECS').setValidator(function(option) {
    
    if(option > 20){
      alert("Amount of ECSs Must be less than 20.");
      return;
    }
    if(option != this.getSourceBlock().Count){
      this.getSourceBlock().addnewOnes_(this.getSourceBlock().Count, option);
      this.getSourceBlock().Count = option;
      RuleID_ECSList_Dictionary[this.getSourceBlock().id] = this.getSourceBlock().Object_List;
    }
  });
  
};

Blockly.Extensions.registerMutator('math_is_divisibleby_mutator_new',
    Blockly.Constants.Logic.IS_DIVISIBLEBY_MUTATOR_MIXIN_NEW,
    Blockly.Constants.Logic.IS_DIVISIBLE_MUTATOR_EXTENSION_NEW);

//dictionary for storing ecs
//ruleBlockId --> dropdownfield of ecss
let RuleID_ECSList_Dictionary = {};



//ecs
let EcsOptions = [
  [
    "Chair",
    "Chair"
  ],
  [
    "Table",
    "Table"
  ],
  [
    "Fridge",
    "Fridge"
  ],
  [
    "Counter",
    "Counter"
  ]
];
Blockly.Blocks['ecsblock'] = {
  init: function() {
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
              "ALL"
            ],
            [
              "ANY",
              "ANY"
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
Blockly.Extensions.register('warning_on_change_ECS', function() {
  // Example validation upon block change:
  this.setOnChange(function(changeEvent) {
    var count = this.getFieldValue('PropertyCount')
    for(var i =1; i <= count;i++){
      if(!this.getInputTargetBlock('Property'+i)){
        this.setWarningText('All Properties must have a Property block input.');
        return;
      }
    }
    this.setWarningText(null);

  });
});
Blockly.Constants.Logic.PROPERTY_MUTATOR_MIXIN = {
  mutationToDom: function() {
    var container = Blockly.utils.xml.createElement('mutation');
    var divisorInput = (this.getFieldValue('PropertyCount'));
    container.setAttribute('divisor_input', divisorInput);
    return container;
  },
  domToMutation: function(xmlElement) {
    var divisorInput = parseInt(xmlElement.getAttribute('divisor_input'), 10) || 0;
    this.addnewOnes_(this.Count, divisorInput);
    this.Count = divisorInput;
  },
  addnewOnes_: function(original, divisorInput) {
    var valueConnections = [null];
    var Propertynames = [null];

    var i = 1;
    while (this.getInput('Property' + i)) {
      var ECS_input = this.getInput('Property' + i);
      
      valueConnections.push(ECS_input.connection.targetConnection);    
      i++;
    }

    if(this.getInput('brackets')){
      this.removeInput('brackets');
    }
    

    for(i = 1; i < original+1; i++){
      this.removeInput('Property' + i);
    }

    if(divisorInput == 0){
      this.appendDummyInput('brackets')
        .appendField('[ ]');
    }

    for(i = 1; i < divisorInput+1; i++){
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
      if(i == divisorInput){
        this.appendDummyInput('brackets')
        .appendField(']');
      }
    }

    if(divisorInput > original){
      this.reconnectChildBlocks_(valueConnections, original);
    }
    else if(divisorInput < original){
      this.reconnectChildBlocks_(valueConnections, divisorInput);
    }
  },
  reconnectChildBlocks_: function(valueConnections, originalCount) {
    for (var i = 0; i <= originalCount; i++) {
      Blockly.Mutator.reconnect(valueConnections[i], this, 'Property' + i);
    }
  }
};

Blockly.Constants.Logic.PROPERTY_MUTATOR_EXTENSION = function() {

  this.getField('PropertyCount').setValidator(function(option) {
    if(option > 20){
      alert("Amount of Properties Must be less than 20.");
      return;
    }
    this.getSourceBlock().addnewOnes_(this.getSourceBlock().Count, option);
    this.getSourceBlock().Count = option;
  });
};

Blockly.Extensions.registerMutator('property_mutator',
    Blockly.Constants.Logic.PROPERTY_MUTATOR_MIXIN,
    Blockly.Constants.Logic.PROPERTY_MUTATOR_EXTENSION);




//properties
let PropertyDimensions = [["Width","Width"], ["Height","Height"], ["Length","Length"]];
Blockly.Blocks['revisedpropertydimension'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown(PropertyDimensions), "DIMENSION")
        .appendField(new Blockly.FieldDropdown([["=","Equal"], ["<","Less"], ["<=","LessOrEqual"], [">","Greater"], [">=","GreaterOrEqual"]]), "SIGN")
        .appendField(new Blockly.FieldNumber(0, 0), "VALUE")
        .appendField(new Blockly.FieldDropdown([["MM","0"], ["CM","1"], ["M","2"], ["INCH","3"], ["FT","4"], ["DEG","5"], ["RAD","6"]]), "UNIT");
    this.setOutput(true, "PROPERTY");
    this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

let PropertyAttachments = [["Door","Door"]];
Blockly.Blocks['revisedpropertyattachments'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Has","HAS"], ["Does not Have","NOT_HAS"]]), "NEGATION")
        .appendField(new Blockly.FieldDropdown(PropertyAttachments), "ATTACHMENT");
    this.setOutput(true, "PROPERTY");
    this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['revisedpropertystring'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Function","Function"]]), "FUNCTION")
        .appendField(new Blockly.FieldDropdown([["=","Equal"], ["!=","NotEqual"], ["Contains","Contains"]]), "SIGN")
        .appendField(new Blockly.FieldTextInput(" "), "STRING");
    this.setOutput(true, "PROPERTY");
    this.setColour('%{BKY_PROPERTYBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};


//Logical Expression
Blockly.Blocks['logicalexpression'] = {
  init: function() {
    this.appendStatementInput("LogicalStatement")
        .setCheck(['relationstruct','logicalEX'])
        .appendField(new Blockly.FieldDropdown([["AND","AND"], ["OR","OR"], ["XOR","XOR"]]), "Expression");
    this.setPreviousStatement(true, ['relationstruct','logicalEX']);
    this.setNextStatement(true, ['relationstruct','logicalEX']);
    this.setColour(90);
    this.setTooltip("");
    this.setHelpUrl("");
    this.setOnChange(function(changeEvent) {

      var colourValue = this.colour_;
      if(this.getFieldValue("Expression") == "AND"){
        colourValue = '%{BKY_AND_COLOUR}';
      }
      else if(this.getFieldValue("Expression") == "OR"){
        colourValue = '%{BKY_OR_COLOUR}';
      }
      else{
        colourValue = '%{BKY_XOR_COLOUR}';
      }
      this.setColour(colourValue);
    });
  }
};



//REALTIONS

//objectchecks
Blockly.Blocks['objectcheck'] = {
  init: function() {
    this.ruleID = null;
    this.appendValueInput("PROPERTYBLOCK")
        .setCheck('PROPERTY')
        .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object")
        .appendField(new Blockly.FieldDropdown([["Must Have","Must"], ["Must Not Have","MustNot"]]), "Negation")
        .appendField(" Property:");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour('%{BKY_CHECKBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
 this.setOnChange(function(changeEvent) {
  if (!this.getInputTargetBlock('PROPERTYBLOCK')) {
    this.setWarningText('Must have an input property block.');
    return;
  } 
  if(this.getFieldValue('Object') == 'var'){
    this.setWarningText('Must set object.');
    return;
  }

  this.setWarningText(null);
});
  },
  generateOptions: function() {

    var options = [['select...','var']];
    if(this.getSourceBlock()){
      var rootBlock = this.getSourceBlock().getRootBlock();
      if(rootBlock.type == 'newruleblock'){
        //this.ruleID = rootBlock.id;
        options = RuleID_ECSList_Dictionary[rootBlock.id];
      }
    }
    return options;
  }
};
//relationchecks
Blockly.Blocks['relationcheck'] = {
  init: function() {
    this.ruleID = null;
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object1");
    this.appendValueInput("Relation")
        .setCheck("RELATION");
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown(this.generateOptions), "Object2");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour('%{BKY_CHECKBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
 this.setOnChange(function(changeEvent) {
  if (!this.getInputTargetBlock('Relation')) {
    this.setWarningText('Must have an input relation block.');
    return;
  }
  if(this.getFieldValue('Object1') == 'var'){
    this.setWarningText('Must set object1.');
    return;
  }
  if(this.getFieldValue('Object2') == 'var'){
    this.setWarningText('Must set object2.');
    return;
  }
  this.setWarningText(null);
});
  },
  generateOptions: function() {
    var options = [['select...','var']];
    if(this.getSourceBlock()){
      var rootBlock = this.getSourceBlock().getRootBlock();
      
      if(rootBlock.type == 'newruleblock'){
        //options = rootBlock.Object_List;
        options = RuleID_ECSList_Dictionary[rootBlock.id];
      }
    }
    return options;
  },
};


function reRenderAllDescendants(block){
  var descendants = block.getDescendants();
  descendants.forEach(element => {
    if(element.type == 'objectcheck'){
      var selected = element.getFieldValue('Object');
      if(selected != 'var' && parseInt(selected.slice(3,selected.length)) > block.Count){
        element.getField('Object').setValue('var');
      }
      refreshDynamicDropdownField(element, 'Object');

    }
    else if(element.type == 'relationcheck'){
      var selected = element.getFieldValue('Object1');
      if(selected != 'var' && parseInt(selected.slice(3,selected.length)) > block.Count){
        element.getField('Object1').setValue('var');
      }
      refreshDynamicDropdownField(element, 'Object1');

      var selected = element.getFieldValue('Object2');
      if(selected != 'var' && parseInt(selected.slice(3,selected.length)) > block.Count){
        element.getField('Object2').setValue('var');
      }
      refreshDynamicDropdownField(element, 'Object2');
    }
  });

}

function resetAllMoved(block){
  var descendants = block.getDescendants();
  descendants.forEach(element => {
    if(element.type == 'objectcheck' && element.ruleID != block.id && element.ruleID != null){
      element.getField('Object').setValue('var');
      refreshDynamicDropdownField(element, 'Object');
      element.ruleID = block.id;
    }
    else if(element.type == 'relationcheck' && element.ruleID != block.id){
      element.getField('Object1').setValue('var');
      refreshDynamicDropdownField(element, 'Object1');
      element.ruleID = block.id;

      element.getField('Object2').setValue('var');
      refreshDynamicDropdownField(element, 'Object2');
    }
  });
}

function refreshDynamicDropdownField(block, fieldName) {
  const field = block.getField(fieldName)
  if (field) {
    field.getOptions(false)
    // work around for https://github.com/google/blockly/issues/3553
    field.doValueUpdate_(field.getValue())
    field.forceRerender()
  }
}

function updateECSList(ruleBlock){
  var options = [['select...','var']];
  for(var i = 1; i <= ruleBlock.getFieldValue('CountECS'); i++){
    options.push([ruleBlock.getFieldValue('VarName'+i),'ecs'+i])
  }
  return options;
}

function refreshObjectList(ruleBlock){
  var childBlocks = ruleBlock.getDescendants(true);
  childBlocks.forEach(element => {
    if(element.type == "objectcheck"){
      var FieldDropdown = element.getField('Object');
      FieldDropdown.setValue('ecs'+RestoreCheckNames.shift());
      element.ruleID = ruleBlock.id;
      console.log(FieldDropdown.getValue());
    }
    else if(element.type == "relationcheck"){
      var FieldDropdown1 = element.getField('Object1');
      FieldDropdown1.setValue('ecs'+RestoreRelationNames.shift());

      var FieldDropdown2 = element.getField('Object2');
      FieldDropdown2.setValue('ecs'+RestoreRelationNames.shift());
      element.ruleID = ruleBlock.id;
      
    }
  });
}



//RelationTypes====================================================
let newBooleanRelations = [["Next to","Next to"]];
Blockly.Blocks['newrelationboolean'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Must be","true"], ["Must not be","false"]]), "Negation")
        .appendField(new Blockly.FieldDropdown(newBooleanRelations), "BoolRelation");
    this.setInputsInline(false);
    this.setOutput(true, "RELATION");
    this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};
let newDistanceRelations = [["Center Distance","Center Distance"], ["Center DistanceXY","Center DistanceXY"], ["BoundingBoxDistance","Bounding Box"]];
Blockly.Blocks['newrelationnumeric'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("has")
        .appendField(new Blockly.FieldDropdown(newDistanceRelations), "DistanceType")
        .appendField(new Blockly.FieldDropdown([[">","0"], [">=","1"],["=","2"], ["<","3"], ["<=","4"],["!=","5"]]), "check")
        .appendField(new Blockly.FieldNumber(0), "distance")
        .appendField(new Blockly.FieldDropdown([["MM","0"], ["CM","1"], ["M","2"], ["INCH","3"], ["FT","4"], ["DEG","5"], ["RAD","6"]]), "MeasurementType")
        .appendField("to");
    this.setInputsInline(false);
    this.setOutput(true, "RELATION");
    this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['newrelationstring'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["Function","Function"]]), "Type")
        .appendField(new Blockly.FieldDropdown([["=","0"], ["!=","1"], ["Contains","2"]]), "check")
        .appendField(new Blockly.FieldTextInput(""), "FunctionString");
    this.setInputsInline(false);
    this.setOutput(true, "RELATION");
    this.setColour('%{BKY_RELATIONBLOCK_COLOUR}');
 this.setTooltip("");
 this.setHelpUrl("");
  }
};


