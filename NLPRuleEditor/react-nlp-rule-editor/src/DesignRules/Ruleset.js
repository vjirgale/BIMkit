
class RuleSet{
    constructor(ID, name){
        this.ID = ID
        this.Name = name;
        this.Rules = [];
    }
    set name(x){
        this.Name = x;
    }
    set rules(x){
        this.Rules = x;
    }
    set xmlstring(x){
        this.xmlstring = x;
    }
    //adds rule to ruleset
    AddRule(rule){
        this.Rules.push(rule);
    }
    //input id of a rule block returns corresponding rule object
    GetRule(ruleID){
        var rule = null;
        this.Rules.forEach(element => {
            if(element.id == ruleID){
                rule = element;
            }
        });
        return rule;
    }
    updateRule(id){
        var rule = this.GetRule(id);
        if(rule){
            UpdateRuleBlock(rule.id, rule);
        }
    }
    deleteRule(id){
        var i = 0;
        this.Rules.forEach(element => {
            //if the id matches delete rule from ruleset
            if(element.id == id){
                this.Rules.splice(i,1);
            }
            else{
                //update rule
                this.updateRule(element.id);
            }
            i+= 1;
        });
    }
    //update all rules in 
    updateAll(){
        var i = 0;
        this.Rules.forEach(element => {
            this.updateRule(element.id);
            i+= 1;
        });
    }
    //checks if the current rule set is valid
    isValid(){
        var is_valid = true;
        //iterates every rule insided the set and checks if it is valid
        this.Rules.forEach(element => {
            if(!element.valid){
                is_valid = false;
                return false;
            }
        });
          
        return is_valid;
    }
    toJSON(){
        return {
            Rules: this.Rules,
            Name: this.Name,
            Description: ""};
    }
}

