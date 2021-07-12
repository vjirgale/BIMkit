import React, { useState, useCallback, useEffect } from 'react';
import io from "socket.io-client";
import {postData} from '../tools/Tools.js';
import {ParseJson} from '../tools/jsonParser';
import {Editor, EditorState, CompositeDecorator, ContentState, SelectionState } from 'draft-js';
import 'draft-js/dist/Draft.css';
import _ from "lodash";
import Button from 'react-bootstrap/Button';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import { popoverSuggestion } from './Popovers.js';
import Toast from "react-bootstrap/Toast"
import errorIcon from "../img/error_icon.png"


var socket = io({path: "/socket.io"});
var RELATION_MATCH = RegExp('Relation', 'g')
var TYPE_MATCH = RegExp('Type', 'g')
var PROPERTY_MATCH = RegExp('Property', 'g')
var UNIT_MATCH = RegExp('Unit', 'g')
var UNKNOWN_MATCH = RegExp('Unknown', 'g')

export function InputForm(props) {
  // functions that let us access and change state
  const [value, setValue] = useState(props.activeRule.Description);
  const [response, setResponse] = useState(props.activeRule.translation);

  // useCallback prevents from initializing new debounce function when re-rendered, which breaks the functionality of debouncing.
  const debouncedEmit = useCallback(_.debounce(function emitMessage(data){socket.emit("GetParsedComponents", data);}, 200), []);

  useEffect(() => {
    // Each time the activeRule is changed from our props, we update state to be re-rendered.
    setValue(props.activeRule.description);
    setResponse(props.activeRule.translation);
  }, [props.activeRule])

  // Callback function, something that can be called from child to set "value" from our state to the contents of the Draft.js textbox
  const setValueFromChild = (childData) => {
    if(childData !== value){
      setValue(childData);
      debouncedEmit(childData);
    }
  }

  function handleSubmit(event){
    // Runs every time the submit button is pressed, or the user presses enter.
    event.preventDefault();
    setResponse("Processing...");
    postData('/api/translate', {'rule': value}).then(data => {
      try{
        setResponse(data.response);
        let rule = JSON.parse(data.rule);
        rule.translation = data.response;
        rule.Name = props.activeRule.Name;
        props.updateActiveRule(rule);
      }catch(err){
        console.log(err);
      }
    });
  }

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <InputEditor parentCallback = {setValueFromChild} activeRule = {props.activeRule}/>
        <Button onClick={handleSubmit} variant="outline-primary" size="lg" block>Submit</Button>
        <div id="response-wrapper">
          <div id="response-content">{response}</div>
        </div>
      </div>
    </form>
  )
}



// Component for text input field
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
      categories: {types: [], properties: [], relations: [], units: [], unknowns: []},
    };

    this.focus = () => this.refs.editor.focus();
    

    this.onChange = (editorState) => {
      // Sets current state, and also sets the value state of our parent component via the callback function passed in props.
      if(editorState.getCurrentContent().getPlainText() !== this.state.editorState.getCurrentContent().getPlainText()){
        this.props.parentCallback(editorState.getCurrentContent().getPlainText());
      }
      this.setState({editorState: editorState});

      
    } 

  }
  
  

  componentDidUpdate(oldProps){
    if(oldProps.activeRule !== this.props.activeRule){
      // We want to set the text in the rule box to be the description of the rule
      const selectionState = this.state.editorState.getSelection();
      const newContentState = ContentState.createFromText(this.props.activeRule.Description);
      const newEditorState = EditorState.create({currentContent: newContentState, selection: selectionState, decorator:this.compositeDecorator})
      this.setState({editorState: newEditorState})
      // Then try and refresh our highlighting information.
      this.props.parentCallback(newEditorState.getCurrentContent().getPlainText())
    } 
  }

  componentDidMount(){
    socket.on("ReturnParsedComponents", data => {
      // We receive a json formatted string of parsedComponents. We want to pass this new data on to our draftjs component for use in regex matching.
      this.setState({categories: ParseJson(data)});
      if(this.state.categories.relations.length > 0){RELATION_MATCH = RegExp(this.state.categories.relations.join('|'), 'g');}else{RELATION_MATCH = RegExp('Relation', 'g')}
      if(this.state.categories.types.length > 0){TYPE_MATCH = RegExp(this.state.categories.types.join('|'), 'g');}else{TYPE_MATCH = RegExp('Type', 'g')}
      if(this.state.categories.properties.length > 0){PROPERTY_MATCH = RegExp(this.state.categories.properties.join('|'), 'g');}else{PROPERTY_MATCH = RegExp('Property', 'g')}
      if(this.state.categories.units.length > 0){UNIT_MATCH = RegExp(this.state.categories.units.join('|'), 'g');}else{UNIT_MATCH = RegExp('Unit', 'g')}
      if(this.state.categories.unknowns.length > 0){UNKNOWN_MATCH = RegExp(this.state.categories.unknowns.join('|'), 'g');}else{UNKNOWN_MATCH = RegExp('Unknown', 'g')}

      this.compositeDecorator = _.clone(this.compositeDecorator);
      this.setState({editorState: EditorState.set(this.state.editorState, {decorator: this.compositeDecorator})})
    });
  }
  
  render() {
    return (
      <div id="editor-container" style={styles.root}>
        <div style={styles.editor} onClick={this.focus}>
          <Editor
            editorState={this.state.editorState}
            onChange={this.onChange}
            placeholder="Enter your rule here, then press Submit."
            ref="editor"
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
    const text = contentBlock.getText().toLowerCase();
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

  
export function Toaster(props){
  const [display, setDisplay] = useState(false);
  const [content, setContent] = useState({});
  

  function getSuggestionList(content, key){
    // Maps list of suggested alternatives to an <li> object for rendering
    return (
      content[key].map((suggestion) => <li>{suggestion}</li>)
    )
  }

  const listWarnings = Object.keys(content).map((warning) =>
    // Maps unknown suggestions object returned from SocketIO message to HTML toast elements to render.
    // Icon obtained from https://www.iconfinder.com/icons/381599/error_icon

    <div
    aria-live="polite"
    aria-atomic="true"
    style={{
      position: 'relative',
      minHeight: '200px',
      minWidth: '300px'
    }}>
      <div
      className='toast-style' 
      style={{
      position: 'absolute',
      top: 0,
      right: 0,
      }}>
      <Toast onClose = {() => setDisplay(false)} show= {display}>
        <Toast.Header>
          <img src={errorIcon} alt="Error"/>
          <strong className="mr-auto">&nbsp;Error</strong>
        </Toast.Header>
        <Toast.Body>
          Token "{warning}" was not recognized. <br/> Most similar:
          <ul>
            {getSuggestionList(content, warning)}        
          </ul>
        </Toast.Body>
      </Toast>
    </div>
    </div>
  );

  useEffect(() => {
    socket.on("ReturnUnknownRecommendations", _.debounce(function newToast(data){
      // Retrieving unknown word recommendations from the server, sent as a simple dict
      let suggestions = JSON.parse(data);
      setContent(suggestions)
      setDisplay(true);
    }, 1000));
  })

  return(
    <div id='toast-container'>{listWarnings}</div>
  )
}

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