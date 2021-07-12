
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