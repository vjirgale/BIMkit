import React, { useState, useEffect } from "react";
import "./Sidebar.css";
import Button from 'react-bootstrap/Button';
import Popover from 'react-bootstrap/Popover';
import Accordion from 'react-bootstrap/Accordion';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';

// Based on this example by Michael Burrows https://dev.to/michaelburrows/build-a-react-sidebar-navigation-component-2j4i

function Sidebar(props) {
 
  const [sidebar, setSidebar] = useState(false);
  const [ruleset, setRuleset] = useState(props.ruleset);
  const showSidebar = () => setSidebar(!sidebar);
  const [activeRuleText, setActiveRuleText] = useState(ruleset.Rules[0].Name);

  useEffect(() => {
    console.log(0);
    setRuleset(props.ruleset);
    setActiveRuleText(props.activeRule.Name);
  }, [props.ruleset, props.activeRule]);

  function optionClicked(event) {
    console.log(event.target.innerText);
    props.selectActiveRule(props.ruleset.Rules.find(rule => rule.Name === event.target.innerText));
    setActiveRuleText(event.target.innerText);
  }

  function newButtonClicked(event) {
    props.addNewRule()
    setRuleset(props.ruleset)
  }

  function setNewName(event) {
    let newRuleset = ruleset;
    newRuleset.Name = event.target.value;
    props.setRuleset(newRuleset);
    setRuleset(newRuleset);
  }

  function setNewDescription(event) {
    let newRuleset = ruleset;
    newRuleset.Description = event.target.value;
    props.setRuleset(newRuleset);
    setRuleset(newRuleset);
  }

  function setNewRuleName(event){
    console.log(event.target.value);
    //rule.Name = event.target.value;
    setActiveRuleText(event.target.value)
    props.updateActiveRuleName(event.target.value)
  }

  const listRules = ruleset.Rules.map(function returnRuleDiv(rule, index) {
    index++;
    if (rule === props.activeRule) {
      return (
        <div className="list-option rounded active-rule"
          key={index}
          //onClick={setNewRuleName}
        > <input type="text" value={activeRuleText} onChange={setNewRuleName}/>  </div>
      )
    } else {
      return (
        <div className="list-option rounded"
          key={index}
          onClick={optionClicked}
        >{rule.Name}</div>
      )
    }
  });

  const helpPopover = (
    <Popover id="popover-basic">
      <Popover.Content>
        Press here to create or modify your rulesets.
        </Popover.Content>
    </Popover>
  );

  return (
    <nav className={sidebar ? "sidebar active" : "sidebar"}>
      <OverlayTrigger trigger={["hover", "focus"]} placement="right" defaultShow={true}  overlay={helpPopover}>
        <button outline="none" className="hamburger hamburger--elastic" type="button" onClick={showSidebar}>
          <span outline="none" className="hamburger-box">
            <span outline="none" className="hamburger-inner"></span>
          </span>
        </button>
      </OverlayTrigger>
      <Accordion defaultActiveKey="0">
        <Accordion.Toggle as={"div"} className="rules-accordion" eventKey="0" data-toggle="collapse">
          <label>Rules</label><div className={"fa fa-chevron-down"}/>
        </Accordion.Toggle>
        <Accordion.Collapse eventKey="0">
          <div id="ruleset-sidebar-list">
            <ul>
              {listRules}
              <div className="list-option add-rule-button rounded" onClick={newButtonClicked}>
                <div className="fa fa-plus"/>
              </div>
            </ul>
          </div>
        </Accordion.Collapse>
      </Accordion>
      <hr/>
      <div id="ruleset-naming">
        <label>Ruleset Name</label><input id="ruleset-name-box" className="form-control" type="text" defaultValue={ruleset.name} onChange={setNewName} />
        <br />
        <label>Description</label><textarea id="ruleset-description-box"className="form-control" type="text" defaultValue={ruleset.description} onChange={setNewDescription} />
      </div>
      <div id="json-buttons">
        <Button className="btn "variant="outline-primary" id="import-button" onClick={() => document.getElementById('import-file').click()}>Import JSON</Button>
        <Button className="btn"variant="outline-primary" id="export-button" onClick={props.exportJson}>Export to JSON</Button>
      </div>
    </nav>
  );
}
export default Sidebar;
