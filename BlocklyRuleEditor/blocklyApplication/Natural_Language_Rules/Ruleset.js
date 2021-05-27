
XML_NS = 'https://developers.google.com/blockly/xml'

//print current ruleset object as json string into text box
function PrintIntoTextArea(){
    if(CurrentRuleSet == null){
        //if there is no current rule set return nothing
        document.getElementById("JsonPrint").textContent="";
        alert("An error has occured. Please refresh the browser.")
        return;
    }
    //update the Json
    document.getElementById("JsonPrint").textContent=JSON.stringify(CurrentRuleSet, null, 3);
}

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
        PrintIntoTextArea();
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
        PrintIntoTextArea();
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
        PrintIntoTextArea();
    }
    //update all rules in 
    updateAll(){
        var i = 0;
        this.Rules.forEach(element => {
            this.updateRule(element.id);
            i+= 1;
        });
        PrintIntoTextArea();
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
    getXML(){
        var doc = document.implementation.createDocument (XML_NS, 'xml', null);
        this.Rules.forEach(rule => {
            //add each rule
            var ruleXML = rule.getXML();
            doc.documentElement.appendChild(ruleXML);    
        });
        return doc;
    }
}
//taken from https://stackoverflow.com/questions/105034/how-to-create-guid-uuid
function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = (performance && performance.now && (performance.now()*1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if(d > 0){//Use timestamp until depleted
            r = (d + r)%16 | 0;
            d = Math.floor(d/16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r)%16 | 0;
            d2 = Math.floor(d2/16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}
//gloabal variable that references the ruleset object
//*note: there is only 1 ruleset
var CurrentRuleSet = new RuleSet(generateUUID(),"");
