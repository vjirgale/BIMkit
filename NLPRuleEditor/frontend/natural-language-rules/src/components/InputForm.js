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
    const debouncedEmit = useCallback(_.debounce(function emitMessage(data) { props.socket.emit("GetParsedComponents", data); }, 200), []);

    useEffect(() => {
        // Each time the activeRule is changed from our props, we update state to be re-rendered.
        //setValue(props.activeRule.description);
        setPriorityLevel(props.activeRule.ErrorLevel);
        setResponse(props.activeRule.translation);
        setRetranslation(props.activeRule.retranslation);
        
    }, [props.activeRule])

    // Callback function, something that can be called from child to set "value" from our state to the contents of the Draft.js textbox
    const setValueFromChild = (childData) => {
        if (childData !== value) {
            setValue(childData);
            debouncedEmit(childData);
        }
    }

    //Function to select priority level of a rule with the optionselect rendered below.
    const selectChange = e => {
        setPriorityLevel(e.target.value)

    }

    function handleSubmit(event) {
        // Runs every time the submit button is pressed, or the user presses enter.
        event.preventDefault();
        setResponse("Processing...");
        setRetranslation("");
        postData('/api/translate', { 'rule': value, 'customobjects': props.customObjects }).then(data => {
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
        });
    }

    return (
        <form onSubmit={handleSubmit}>
            <div id="input-form">
                <InputEditor className="input-editor" parentCallback={setValueFromChild} activeRule={props.activeRule} socket={props.socket} customObjects = {props.customObjects}/>
                <div className="wrapper">
                        <br/>
                        <select className="select-css" onChange={selectChange} value={priorityLevel}>
                            <option value={"Recommended"}>Error Level: Recommended</option>
                            <option value={"Warning"}>Error Level: Warning</option>
                            <option value={"Error"}>Error Level: Error</option>
                        </select>
                </div>
                <Button onClick={handleSubmit} variant="outline-primary" size="lg" block>Submit</Button>
                <div id="response-wrapper">
                    <br/>
                    <div id="response-content">{response}</div>
                    <br/>
                    <div id="retranslation-content">{retranslation}</div>
                </div>
            </div>
        </form>
    )
}