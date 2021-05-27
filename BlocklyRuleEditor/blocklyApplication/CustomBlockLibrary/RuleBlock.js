//Blockly Custom Blocks page: https://developers.google.com/blockly/guides/create-custom-blocks/overview

//Colours For Blocks
Blockly.Msg.RULEBLOCK_COLOUR = 290;
Blockly.Msg.ECSBLOCK_COLOUR = 350;
Blockly.Msg.PROPERTYBLOCK_COLOUR = 320;
Blockly.Msg.CHECKBLOCK_COLOUR = 220;
Blockly.Msg.RELATIONBLOCK_COLOUR = 240;

//Colours for Logical Expression
Blockly.Msg.OR_COLOUR = 100;
Blockly.Msg.AND_COLOUR = 300;
Blockly.Msg.XOR_COLOUR = 15;


//ENUMS
const ErrorLevel = {Recommended : 'Recommended', Warning : 'Warning', Error : 'Error'}
const OccuranceRule = {ALL : 'ALL', ANY : 'ANY', NONE : 'NONE'}
const LogicalOperator = {AND : 'AND', OR : 'OR', XOR : 'XOR'}
const Negation = {MUST_HAVE : 'EQUAL', MUST_NOT_HAVE : 'NOT_EQUAL'}

const UnitEnums = {MM:'MM', CM:'CM', M:'M', INCH:'INCH', FT:'FT', DEG:'DEG', RAD:'RAD'}
const OperatorNum = {GREATER_THAN : 'GREATER_THAN', GREATER_THAN_OR_EQUAL : 'GREATER_THAN_OR_EQUAL', EQUAL : 'EQUAL', LESS_THAN : 'LESS_THAN', LESS_THAN_OR_EQUAL : 'LESS_THAN_OR_EQUAL', NOT_EQUAL : 'NOT_EQUAL'}
const OperatorString = {EQUAL:'EQUAL', NOT_EQUAL:'NOT_EQUAL', CONTAINS:'CONTAINS'}



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
              "Error",
              ErrorLevel.Error
            ],
            [
              "Warning",
              ErrorLevel.Warning
            ],
            [
              "Recommended",
              ErrorLevel.Recommended
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
    for(var i =1; i < count;i++){
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
    var Object_List = [['select...','0']];

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
        Object_List.push([ECSnames[i],i.toString()]);
      }
      else{
        this.appendValueInput('ECS' + i)
          .setCheck('ECS')
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendField("Set")
          .appendField(new Blockly.FieldTextInput('Object'+(i-1)), "VarName"+i)
          .appendField("to:");
        this.getField("VarName"+i).maxDisplayLength = 20;
        //add new/default name to ecs dropdown array
        Object_List.push(['Object'+(i-1), i.toString()]);
      }
      //move input field in ruleblock so it comes before relations
      this.moveInputBefore('ECS'+i, 'SeperationSpace');
      //add a validator. THe kitchen designer must name the objects starting with a letter.
      this.getField("VarName"+i).setValidator(function(option) {
        //while the first character is not a letter
        while(option.length > 0 && !option[0].match(/[a-z]/i)){
          //delete character
          option = option.substring(1);
        }
        /*//change first character to lowercase
        if(option.length > 0 && option[0] == option[0].toUpperCase()){
          option = option[0].toLowerCase() + option.slice(1);
        }*/
        
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





















