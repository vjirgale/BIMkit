

//called every time a block is moved in the workspace.
//updateAfterRuleID is the id of a rule block of which a block was disconnected from.
//this function is called twice when it is first dragged and when it is dropped.
function BlockMovedEvent(updateAfterRuleID, event){
    var moved_block = Workspace.getBlockById(event.blockId);

    //if moved block was disconnected from a rule block. (after dropped)
    if(updateAfterRuleID != null){
      //update ruleblock that the block was detatched from
      CurrentRuleSet.updateRule(updateAfterRuleID);
      updateAfterRuleID = null;
    }

    //if the moved block is now attached to a ruleblock 
    if(moved_block && moved_block.getRootBlock().type == "ruleblock")
    {
      //resets dropdowns of the check blocks that were moved.
      resetAllMoved(moved_block.getRootBlock());

      //updates json of the ruleblock that the moved block is now attached to.
      CurrentRuleSet.updateRule(moved_block.getRootBlock().id);
    }

    //if the moved block was dettached from a ruleblock (before dropped)
    if(moved_block && event.oldParentId){
      var OldRootBlock = Workspace.getBlockById(event.oldParentId).getRootBlock();
      if(OldRootBlock.type == "ruleblock"){
        //set it to update the json of the rule block on the next move event
        //*note: move event is called twice when a block that had a parent is moved.
        updateAfterRuleID = OldRootBlock.id;
      } 
    }
    return updateAfterRuleID;
}

//called every time a block value is cahnged in the workspace.
function BlockChangedEvent(event){
    //block that had a value changed
    var changed_block = Workspace.getBlockById(event.blockId);

    if(changed_block.type == "ruleblock"){

      //if an ecs name was changed
      if(event.name.slice(0,7) == "VarName"){

        //copy ecs name list from dictionary
        var currentList = RuleID_ECSList_Dictionary[event.blockId];
        //parse name of the field for the position in ecs list.
        var place = parseInt(event.name.slice(7,event.name.length));
        //update the changed name
        currentList[place] = [event.newValue, (place).toString()];
      
        //update the ecs name list in dictionary
        RuleID_ECSList_Dictionary[event.blockId] = currentList;
      }
      //rerender the dropdowns of all check blocks in the rule
      reRenderAllDescendants(changed_block);
    }
    //if the changed block is or is connected to a rule block update the Json
    if(changed_block.getRootBlock().type == "ruleblock"){
      CurrentRuleSet.updateRule(changed_block.getRootBlock().id);
    }
}

//called every time a block is created in the workspace.
function BlockCreatedEvent(event){
     //block that was created
     var newly_created_block = Workspace.getBlockById(event.blockId);
          
     //if newly created block is a ruleblock
     if(newly_created_block.type == "ruleblock"){

       //add the rule to RuleID_ECSList_Dictionary
       RuleID_ECSList_Dictionary[event.blockId] = updateECSList(newly_created_block);
       //create a new rule
       var newRule = new Rule(event.blockId, newly_created_block.getFieldValue('RULENAME'), newly_created_block.getFieldValue('DESCRIPTION'), 
                 newly_created_block.getFieldValue('ERRORLEVEL'));

       //if the rule block has child blocks. ie. if it was imported
       //setup child blocks
       reRenderAllDescendants(newly_created_block);
       setCheckBlockRuleIDs(newly_created_block);

       //update the ruleset object
       CurrentRuleSet.AddRule(newRule);
     }
}