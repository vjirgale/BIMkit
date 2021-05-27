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
            "options": ecsOptions
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