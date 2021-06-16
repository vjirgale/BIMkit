import React from 'react';
import ReactDOM from 'react-dom';

import { Editor, EditorState, CompositeDecorator, ContentState } from 'draft-js';
import { ParseJson } from '../tools/jsonParser';
import _ from "lodash";
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import { popoverSuggestion } from './Popovers.js';
import 'draft-js/dist/Draft.css';
import {fetchData} from '../tools/Fetch';


async function fetchTypes(){
    try{
        console.log('123');
        var result= await fetchData("https://localhost:44370/api/method/type");
        console.log(result);
        var values = Object.values(result);
        var regex = 'Relation';
        for(var i =0; i < values.length; i++){
            regex += '|'+values[i];
        }
        TYPE_MATCH = RegExp(regex, 'gi');
        return;
    }
    catch{
        console.log("Failed to fetch property methods. Reload page to try again or continue with default.");
       //use default
       TYPE_MATCH = RegExp('Type|Chairs*|Sofas*|Oven', 'gi');
    }
    
    return;
}

var OCCURRENCE_MATCH = RegExp('Occurrence|ANY|ALL|NONE', 'gi')
var TYPE_MATCH = RegExp('Type|Chairs*|Sofas*|Oven|Microwave|Windows', 'gi');
fetchTypes();
var NEGATION_MATCH = RegExp('Negation|Not', 'gi')
var PROPERTY_MATCH = RegExp('Property|DISTANCE|FunctionOfObj|Width|Height|Length', 'gi')
var OPERATION_MATCH = RegExp('Operation|less than|more than', 'gi')
var VALUE_MATCH = RegExp('Value|[0-9]','gi')
var UNIT_MATCH = RegExp('Unit|MM|CM|M|meter|INCH|FT|feet|DEG|RAD', 'gi')
var LOGICAL_OPERATOR_MATCH = RegExp('LOGICAL OPERATOR|AND|OR|ANY', 'gi')
var RELATION_MATCH = RegExp('Relation|Next to|Above', 'gi')
var UNKNOWN_MATCH = RegExp('Unknown', 'gi')

// Component for text input field that handles text coloring with the Draft.js library.
// Resides within InputForm
export class InputEditor extends React.Component {

    constructor(props) {
        super(props);
        this.compositeDecorator = new CompositeDecorator([
            {
                strategy: occurrenceStrategy,
                component: occurrenceSpan,
            },
            {
                strategy: typeStrategy,
                component: typeSpan,
            },
            {
                strategy: negationStrategy,
                component: negationSpan,
            },
            {
                strategy: propertyStrategy,
                component: propertySpan,
            },
            {
                strategy: operationStrategy,
                component: operationSpan,
            },
            {
                strategy: valueStrategy,
                component:valueSpan,
            },
            {
                strategy: unitStrategy,
                component: unitSpan,
            },
            {
                strategy: logicalOperatorStrategy,
                component: logicalOperatorSpan,
            },
            {
                strategy: relationStrategy,
                component: relationSpan,
            },
            {
                strategy: unknownStrategy,
                component: unknownSpan,
            },
        ]);

        this.state = {
            editorState: EditorState.createEmpty(this.compositeDecorator),
            categories: { types: [], properties: [], relations: [], units: [], unknowns: [] },
        };
        
        this.editor = React.createRef();

        this.focus = () => this.editor.current.focus();

        //called when editor is changed
        this.onChange = (editorState) => {
            //console.log(11111);
            //console.log(editorState.getCurrentContent().getPlainText() !== this.state.editorState.getCurrentContent().getPlainText());
            // Sets current state, and also sets the value state of our parent component via the callback function passed in props.
            if (editorState.getCurrentContent().getPlainText() !== this.state.editorState.getCurrentContent().getPlainText()) {
                //console.log("parent call back");
                this.props.parentCallback(editorState.getCurrentContent().getPlainText());
            }
            this.setState({ editorState: editorState });
        }
    }

    componentDidUpdate(oldProps) {
        //console.log(12344);
        if (oldProps.activeRule !== this.props.activeRule) {
            // We want to set the text in the rule box to be the description of the rule
            const selectionState = this.state.editorState.getSelection();
            const newContentState = ContentState.createFromText(this.props.activeRule.Description);
            const newEditorState = EditorState.create({ currentContent: newContentState, selection: selectionState, decorator: this.compositeDecorator })
            this.setState({ editorState: newEditorState })
            // Then try and refresh our highlighting information.
            this.props.parentCallback(newEditorState.getCurrentContent().getPlainText())
        }
    }



    render() {
        return (
            <div id="editor-container" style={styles.root}>
                <div id="editor" style={styles.editor} onClick={this.focus}>
                    <Editor
                        editorState={this.state.editorState}
                        onChange={this.onChange}
                        placeholder="Enter your rule here, then press Submit."
                        ref={this.editor}
                        spellCheck={true}
                    />
                </div>
            </div>
        );
    }
}




function occurrenceStrategy(contentBlock, callback, contentState) {
    findCategory(OCCURRENCE_MATCH, contentBlock, callback);
}

function typeStrategy(contentBlock, callback, contentState) {
    findCategory(TYPE_MATCH, contentBlock, callback);
}

function negationStrategy(contentBlock, callback, contentState) {
    findCategory(NEGATION_MATCH, contentBlock, callback);
}

function propertyStrategy(contentBlock, callback, contentState) {
    findCategory(PROPERTY_MATCH, contentBlock, callback);
}

function operationStrategy(contentBlock, callback, contentState) {
    findCategory(OPERATION_MATCH, contentBlock, callback);
}

function valueStrategy(contentBlock, callback, contentState) {
    findCategory(VALUE_MATCH, contentBlock, callback);
}

function logicalOperatorStrategy(contentBlock, callback, contentState) {
    findCategory(LOGICAL_OPERATOR_MATCH, contentBlock, callback);
}

function unitStrategy(contentBlock, callback, contentState) {
    findCategory(UNIT_MATCH, contentBlock, callback);
}

function relationStrategy(contentBlock, callback, contentState) {
    findCategory(RELATION_MATCH, contentBlock, callback);
}

function unknownStrategy(contentBlock, callback, contentState) {
    findCategory(UNKNOWN_MATCH, contentBlock, callback);
}




function findCategory(regex, contentBlock, callback) {
    const text = contentBlock.getText();
    let matchArr, start;
    while ((matchArr = regex.exec(text)) !== null) {
        start = matchArr.index;
        callback(start, start + matchArr[0].length);
    }
}


const occurrenceSpan = (props) => {
    return (
        <span
            style={styles.occurrence}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const typeSpan = (props) => {
    return (
        <span
            style={styles.type}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const negationSpan = (props) => {
    return (
        <span
            style={styles.negation}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const propertySpan = (props) => {
    return (
        <span
            style={styles.property}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const operationSpan = (props) => {
    return (
        <span
            style={styles.operation}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const valueSpan = (props) => {
    return (
        <span
            style={styles.value}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const unitSpan = (props) => {
    return (
        <span
            style={styles.unit}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const logicalOperatorSpan = (props) => {
    return (
        <span
            style={styles.logicalOperator}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const relationSpan = (props) => {
    return (
        <span
            style={styles.relation}
            data-offset-key={props.offsetKey}
        >
            {props.children}
        </span>
    );
};

const unknownSpan = (props) => {
    return (
        <OverlayTrigger trigger="focus" placement="top" overlay={popoverSuggestion}>
            <span
                style={styles.unknown}
                data-offset-key={props.offsetKey}
            >
                {props.children}
            </span>
        </OverlayTrigger>
    );
};



const styles = {
    root: {
        fontFamily: '\'Helvetica\', sans-serif',
        paddingBottom: 10,
        width: "100%"
    },
    editor: {
        border: '1px solid #ddd',
        cursor: 'text',
        fontSize: 16,
        minHeight: 275,
        padding: 10,
    },
    button: {
        marginTop: 10,
        textAlign: 'center',
    },
    occurrence: {
        color: 'rgba(255, 146, 45, 1.0)',
        //orange
    },
    type: {
        color: 'rgba(255, 193, 7, 1.0)',
        //yellow
    },
    negation: {
        color: 'rgba(67,205,49,1.0)',
        //apple
    },
    
    property: {
        color: 'rgba(40, 167, 69, 1.0)',
        //green
    },
    operation: {
        color: 'rgba(240,253,199, 1.0)',
        //mimosa
    },
    value: {
        color: 'rgba(20, 99, 112, 1.0)',
        //darker cyan
    },
    unit: {
        color: 'rgba(252,153,163, 1.0)',
        //Sweet Pink
    },
    logicalOperator: {
        color: 'rgba(198, 198, 255, 1.0)',
        //purple
    },
    relation: {
        color: 'rgba(0, 123, 255, 1.0)',
        //blue
    },
    
    unknown: {
        color: 'rgba(220, 53, 69, 1.0)',
        //red
    },
    suggestions: {
        float: 'right',
        width: '1%',
        right: '-100px'

    }
};