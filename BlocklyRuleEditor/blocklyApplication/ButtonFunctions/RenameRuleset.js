   //update ruleset name
   function RenameRuleset() {
    var x = document.getElementById("RuleSetName").value;
    CurrentRuleSet.name = x;
    PrintIntoTextArea();
 }