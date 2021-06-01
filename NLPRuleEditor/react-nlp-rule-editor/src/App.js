import React, { useState } from 'react';
import './App.css';
import { InputForm } from './components/InputForm';
import { ExampleButton, TypeButton, PropertyButton, RelationButton, UnitButton, UnknownButton } from './components/Popovers';
import 'bootstrap/dist/css/bootstrap.min.css';
import Button from 'react-bootstrap/esm/Button';
import Sidebar from "./components/Sidebar";
import { WarningList } from "./components/WarningList";
import { postData } from "./tools/Tools";
//import '../node_modules/draft-js/dist/Draft.css';

function App() {
  // Example page was built based on the following: https://blog.miguelgrinberg.com/post/how-to-create-a-react--flask-project

  // Page itself has some state, as it stores the current ruleset object. It also has an "Active Rule", which it keeps track of which rule is in focus.
  const [ruleset, setRuleset] = useState(new Ruleset("New Ruleset", "Ruleset", [new TempRule("Rule1", "", "Recommended")]));
  const [ruleCount, setRuleCount] = useState(1); // Used for default names of Rules
  const [activeRule, setActiveRule] = useState(ruleset.Rules[0])
  const [customObjects, setCustomObjects] = useState([]) // Words in this list will be passed on to use in the final rule

  // Callback function to update active rule from InputForm
  const updateActiveRule = (childData) => {
    ruleset.update(activeRule, childData);
    setActiveRule(childData);
  }

  const selectActiveRule = (rule) => {
    setActiveRule(rule);
  }
  const deleteActiveRule = () => {
    if (ruleset.Rules.length === 1) {
      // We need to make a new one to focus on
      ruleset.addTempRule("Rule1", "")
    }
    let pos = ruleset.Rules.indexOf(activeRule);
    if (pos === (ruleset.Rules.length - 1)) {
      // Last element in the list, so we set active rule to the previous one
      setActiveRule(ruleset.Rules[pos - 1])
    } else {
      setActiveRule(ruleset.Rules[pos + 1])
    }
    ruleset.Rules.splice(pos, 1);


  }
  const addNewRule = () => {
    // Increment the last number
    let newName = "Rule" + (ruleCount + 1);
    setRuleCount(ruleCount + 1);
    ruleset.addTempRule(
      newName, ""
    )
  }

  const addCustomObject = (word) => {
    customObjects.push(word);
  }

  const exportJson = () => {
    // Generate ruleset JSON file and download it
    let toExport = JSON.parse(JSON.stringify(ruleset));
    let filteredRules = toExport.Rules.filter((rule) => !(rule instanceof TempRule));
    filteredRules.map(rule => {delete rule.translation; delete rule.retranslation;});
    toExport.Rules = filteredRules;
    // from https://stackoverflow.com/a/30727676 
    let dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify([toExport]));
    let a = document.createElement('a');
    a.href = 'data:' + dataStr;
    a.download = ruleset.Name + '.json'
    a.click();
  }

  const uploadHandler = (event) => {
    const reader = new FileReader();
    reader.onload = async (e) => {
      let text = (e.target.result);
      try {
        let importedRulesets = JSON.parse(text);
        // We create new ruleset object using our class
        let newRuleset = new Ruleset(importedRulesets[0].Name, importedRulesets[0].Description, importedRulesets[0].Rules);
        // The translation for a rule is not stored in the JSON of the rule, 
        for (let rule of newRuleset.Rules) {
          postData('/api/translate', { 'rule': rule.Description }).then(data => {
            try {
              rule.translation = data.response;
              rule.retranslation = data.retranslation;
            } catch (err) {
              console.log(err);
            }
          });
        }
        setRuleset(newRuleset);
        setRuleCount(newRuleset.Rules.length);
        if (ruleCount === 0) {
          ruleset.addTempRule("Rule1", "");
        }
        setActiveRule(newRuleset.Rules[0]);
      } catch {
        alert("Parsing failed, please check file and try again.");
      }
    };
    reader.readAsText(event.target.files[0])
  }

  return (
    <div className="App" className="container-fluid">
     <div className="row">
       
        <div id="left-sidebar" className="col-md">
          <Sidebar ruleset={ruleset} activeRule={activeRule} selectActiveRule={selectActiveRule} addNewRule={addNewRule} deleteActiveRule={deleteActiveRule} setRuleset={setRuleset} exportJson={exportJson} uploadHandler = {uploadHandler}/>
        </div>
        
        <div id="main-content" className="col-md-8">
          <div id="App-header">
            <h1>Natural Language Rules</h1>
            <h5>Natural language to JSON rule conversion.</h5>
          </div>
          
          <div id="category-buttons">
            <TypeButton />
            <PropertyButton />
            <RelationButton />
            <UnitButton />
            <UnknownButton />
            <ExampleButton />
          </div>

          <div id="editor">
            <InputForm activeRule={activeRule} updateActiveRule={updateActiveRule}  customObjects = {customObjects}/>
          </div>
          <Button id="delete-button" className="btn btn-danger" onClick={deleteActiveRule}>Delete <i className="fa fa-trash"/></Button>
          <input style={{ visibility: "hidden" }} type="file" name="file" id="import-file" onChange={uploadHandler} />
          
        </div>
        
        <div id="right-sidebar" className="col-md">
          <WarningList addCustomObject = {addCustomObject} customObjects = {customObjects}/>
        </div>
        
      </div>
    </div>
  );
}

// Rulset object class definition
class Ruleset {
  constructor(name = "Name", description = "Description", Rules = [new TempRule()]) {
    this.Name = name;
    this.Description = description;
    this.Rules = Rules;
  }
  addTempRule(name, description) {
    // For adding a rule to the list before replacing it with a translated rule object.
    var tempRule = new TempRule(name, description);
    this.Rules.push(tempRule);
  }
  update(tempRule, rule) {
    // Once we have received a rule object from translation phase, replace it in the list
    let index = this.Rules.indexOf(tempRule);
    this.Rules[index] = rule;
  }
  removeRule(rule) {
    // Remove the rule from the list
    let index = this.Rules.findIndex(rule);
    this.Rules.pop(index);
  }
}

// Class for a mock rule which only contains name and description
class TempRule {
  constructor(name = "Rule", description = "", ErrorLevel = "Recommended") {
    this.Name = name;
    this.Description = description;
    this.ErrorLevel = ErrorLevel;
  }
}


export default App;
