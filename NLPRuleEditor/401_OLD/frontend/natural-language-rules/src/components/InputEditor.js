import React from 'react';
import { Editor, EditorState, CompositeDecorator, ContentState } from 'draft-js';
import { ParseJson } from '../tools/jsonParser';
import _ from "lodash";
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import { popoverSuggestion } from './Popovers.js';
import 'draft-js/dist/Draft.css';


var RELATION_MATCH = RegExp('Relation', 'g')
var TYPE_MATCH = RegExp('Type', 'g')
var PROPERTY_MATCH = RegExp('Property', 'g')
var UNIT_MATCH = RegExp('Unit', 'g')
var UNKNOWN_MATCH = RegExp('Unknown', 'g')

// Component for text input field that handles text coloring with the Draft.js library.
// Resides within InputForm
export class InputEditor extends React.Component {
    constructor(props) {
        super(props);
        this.compositeDecorator = new CompositeDecorator([
            {
                strategy: relationStrategy,
                component: relationSpan,
            },
            {
                strategy: typeStrategy,
                component: typeSpan,
            },
            {
                strategy: propertyStrategy,
                component: propertySpan,
            },
            {
                strategy: unitStrategy,
                component: unitSpan,
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


        this.onChange = (editorState) => {
            // Sets current state, and also sets the value state of our parent component via the callback function passed in props.
            if (editorState.getCurrentContent().getPlainText() !== this.state.editorState.getCurrentContent().getPlainText()) {
                this.props.parentCallback(editorState.getCurrentContent().getPlainText());
            }
            this.setState({ editorState: editorState });


        }

    }

    componentDidUpdate(oldProps) {
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

    componentDidMount() {
        this.props.socket.on("ReturnParsedComponents", data => {
            // We receive a json formatted string of parsedComponents. We want to pass this new data on to our draftjs component for use in regex matching.
            let newCategories = ParseJson(data);
            // we strip out any custom objects
            this.setState({ categories: newCategories });
            if (this.state.categories.relations.length > 0) { RELATION_MATCH = RegExp(this.state.categories.relations.join('|'), 'g'); } else { RELATION_MATCH = RegExp('Relation', 'g') }
            if (this.state.categories.types.length > 0) { TYPE_MATCH = RegExp(this.state.categories.types.join('|'), 'g'); } else { TYPE_MATCH = RegExp('Type', 'g') }
            if (this.state.categories.properties.length > 0) { PROPERTY_MATCH = RegExp(this.state.categories.properties.join('|'), 'g'); } else { PROPERTY_MATCH = RegExp('Property', 'g') }
            if (this.state.categories.units.length > 0) { UNIT_MATCH = RegExp(this.state.categories.units.join('|'), 'g'); } else { UNIT_MATCH = RegExp('Unit', 'g') }
            if (this.state.categories.unknowns.length > 0) { UNKNOWN_MATCH = RegExp(this.state.categories.unknowns.join('|'), 'g'); } else { UNKNOWN_MATCH = RegExp('Unknown', 'g') }

            this.compositeDecorator = _.clone(this.compositeDecorator);
            this.setState({ editorState: EditorState.set(this.state.editorState, { decorator: this.compositeDecorator }) })
        });
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




function relationStrategy(contentBlock, callback, contentState) {
    findCategory(RELATION_MATCH, contentBlock, callback);
}

function typeStrategy(contentBlock, callback, contentState) {
    findCategory(TYPE_MATCH, contentBlock, callback);
}

function propertyStrategy(contentBlock, callback, contentState) {
    findCategory(PROPERTY_MATCH, contentBlock, callback);
}

function unitStrategy(contentBlock, callback, contentState) {
    findCategory(UNIT_MATCH, contentBlock, callback);
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
        width: 600,
    },
    editor: {
        border: '1px solid #ddd',
        cursor: 'text',
        fontSize: 16,
        minHeight: 40,
        padding: 10,
    },
    button: {
        marginTop: 10,
        textAlign: 'center',
    },
    relation: {
        color: 'rgba(0, 123, 255, 1.0)',
        //blue
    },
    type: {
        color: 'rgba(255, 193, 7, 1.0)',
        //yellow
    },
    property: {
        color: 'rgba(40, 167, 69, 1.0)',
        //green
    },
    unit: {
        color: 'rgba(23, 162, 184, 1.0)',
        //cyan
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