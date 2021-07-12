import { useState, useCallback, useEffect } from 'react';
import { postData } from '../tools/Tools.js';
import _ from "lodash";
import Button from 'react-bootstrap/Button';
import { InputEditor } from './InputEditor';
import './InputForm.css'



//Input form is the component that sends user input to the backend through a socket.io endpoint. It receives text coloring from InputEditor.
export function InputForm(props) {
    // functions that let us access and change state
    const [value, setValue] = useState(props.activeRule.description);
    const [response, setResponse] = useState(props.activeRule.translation);
    const [retranslation, setRetranslation] = useState(props.activeRule.retranslation);
    const [priorityLevel, setPriorityLevel] = useState(props.activeRule.ErrorLevel)

    // useCallback prevents from initializing new debounce function when re-rendered, which breaks the functionality of debouncing.
    //const debouncedEmit = useCallback(_.debounce(function emitMessage(data) { props.socket.emit("GetParsedComponents", data); }, 200), []);

    useEffect(() => {
        console.log(100);
        console.log(props.activeRule);
        // Each time the activeRule is changed from our props, we update state to be re-rendered.
        setValue(props.activeRule.description);
        setPriorityLevel(props.activeRule.ErrorLevel);
        setResponse(props.activeRule.translation);
        setRetranslation(props.activeRule.retranslation);
        
    }, [props.activeRule])

    // Callback function, something that can be called from child to set "value" from our state to the contents of the Draft.js textbox
    const setValueFromChild = (childData) => {
        //console.log(childData); childData = text from editor
        //console.log("went to call back");
        //console.log(childData !== value);
        if (childData !== value) {
            setValue(childData);
            props.updateActiveRuleDescription(childData);
            //debouncedEmit(childData);
            //props.updateActiveRuleDescription(childData);
            //console.log(value);
        }
        
    }

    //Function to select priority level of a rule with the optionselect rendered below.
    const selectChange = e => {
        setPriorityLevel(e.target.value)
        props.updateActiveRuleErrorLevel(e.target.value);
    }

    function handleSubmit(event) {
        // Runs every time the submit button is pressed, or the user presses enter.
        event.preventDefault();
        //todo: translate data

        //set state to translated data
        var obj = {
            Name: "New Ruleset",
            Description: "Ruleset",
            Rules: [{
                    Name: "Rule1",
                    Description: "All windows should have a width of no less than 15 inch and a height of no more than 2 feet and is above a sink.",
                    ExistentialClauses: {
                        Object0: {
                            OccurenceRule: "ALL",
                            Characteristic: {
                                Type: "Window",
                                PropertyChecks: []
                            }
                        },
                        Object1: {
                            OccurenceRule: "ANY",
                            Characteristic: {
                                Type: "Sink",
                                PropertyChecks: []
                            }
                        }
                    },
                    LogicalExpression: {
                        ObjectChecks: [
                            {
                                ObjName: "Object0",
                                Negation: "MUST_HAVE",
                                PropertyCheck: {
                                    Operation: "GREATER-THAN-OR-EQUAL",
                                    Value: 15,
                                    ValueUnit: "inch",
                                    Name: "Width",
                                    PCType: "NUM"
                                }
                            },
                            {
                                ObjName: "Object0",
                                Negation: "MUST_HAVE",
                                PropertyCheck: {
                                    Operation: "LESS-THAN-OR-EQUAL",
                                    Value: 2,
                                    ValueUnit: "feet",
                                    Name: "Height",
                                    PCType: "NUM"
                                }
                            }
                        ],
                        RelationChecks: [
                            {
                                Obj1Name: "Object0",
                                Obj2Name: "Object1",
                                Negation: "MUST_HAVE",
                                PropertyCheck: {
                                    Operation: "EQUAL",
                                    Value: true,
                                    Name: "IsAbove",
                                    PCType: "BOOL"
                                }
                            }
                        ],
                        LogicalExpressions: [],
                        LogicalOperator: "AND"
                    },
                    ErrorLevel: "Recommended"
                }
            ]
        }
        setResponse(JSON.stringify(obj, null));
        setRetranslation(value);
        //let rule = {description: value, translation: "1212", retranslation: retranslation, Name: "Rule 1", ErrorLevel: priorityLevel}
        //props.updateActiveRule(rule);

        console.log(value);
        console.log(props.customObjects);

        /*postData('/api/translate', { 'rule': value, 'customobjects': props.customObjects }).then(data => {
            try {
                setResponse(data.response);
                let rule = JSON.parse(data.rule);
                rule.translation = data.response;
                rule.retranslation = data.retranslation;
                rule.Name = props.activeRule.Name;
                rule.ErrorLevel = priorityLevel;
                props.updateActiveRule(rule);
            } catch (err) {
                console.log(err);
            }
        });*/
    }
    
    return (
        <form id='editor-input-output' onSubmit={handleSubmit}>
            <div id="response-wrapper"style={{display:'none'}}>
                    <div id="response-content" style={{visibility:'hidden'}}>{response}</div>
                    <br/>
                    <div id="retranslation-content" style={{visibility:'hidden'}}>{retranslation}</div>
            </div>
            <div id="input-form">
                <InputEditor className="input-editor" parentCallback={setValueFromChild} activeRule={props.activeRule} customObjects = {props.customObjects}/>
                <div className="wrapper">
                        <br/>
                        <select className="select-css" onChange={selectChange} value={priorityLevel}>
                            <option value={"Recommended"}>Error Level: Recommended</option>
                            <option value={"Warning"}>Error Level: Warning</option>
                            <option value={"Error"}>Error Level: Error</option>
                        </select>
                </div>
                <Button onClick={handleSubmit} variant="outline-primary" size="lg" block>Submit</Button>
                <div id="response-wrapper" style={{display:'none'}}>
                    <br/>
                    <div id="response-content" >{response}</div>
                    <br/>
                    <div id="retranslation-content">{retranslation}</div>
                </div>
            </div>
            <div id="response-wrapper" style={{display:'none'}}>
                    <div id="response-content">{response}</div>
                    <br/>
                    <div id="retranslation-content">{retranslation}</div>
            </div>
        </form>
    )
}