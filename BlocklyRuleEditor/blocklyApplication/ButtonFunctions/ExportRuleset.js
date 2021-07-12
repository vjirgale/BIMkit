/* update color when ruleset is valid */
function UpdateExportColor(isValid){
    if(isValid){
        //turns green if ruleset is valid
        document.getElementById("exportButton").style.backgroundColor = "#4CAF50";
        document.getElementById("exportButton").style.border = "#aea";
    }
    else{
        //turns red if ruleset is not valid
        document.getElementById("exportButton").style.backgroundColor = "#ff195f";
        document.getElementById("exportButton").style.border = "#fcc";
    }
}



//function to export the ruleset into a file
function ExportRuleset() {
    console.log(Blockly.Xml.workspaceToDom(Workspace));
    try {
        //update json for all rules in ruleset
        CurrentRuleSet.updateAll();

        //convert ruleset to JSON
        var text = JSON.stringify(CurrentRuleSet, null, 3);
        
        var filename = document.getElementById("RuleSetName").value;

        //export/download ruleset as text/json file
        var blob = new Blob([text], {
        type: "application/json;charset=utf-8"
        });
        saveAs(blob, filename);
    } catch (e) {
        alert(e);
    }
}
//function to export the ruleset into a file
function ExportRules() {
    try {
        //update json for all rules in ruleset
        CurrentRuleSet.updateAll();

        CurrentRuleSet.Rules.forEach(rule =>{

            //convert ruleset to JSON
            var text = JSON.stringify(rule, null, 3);
            
            var filename = rule.title;

            //export/download ruleset as text/json file
            var blob = new Blob([text], {
            type: "application/json;charset=utf-8"
            });
            saveAs(blob, filename);
        });
        
    } catch (e) {
        alert(e);
    }
}