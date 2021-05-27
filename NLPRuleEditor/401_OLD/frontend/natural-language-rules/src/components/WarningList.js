import React, { useState, useEffect } from 'react';
import _ from "lodash";
import Button from 'react-bootstrap/esm/Button';


//Function to catch and display unrecognized components in a sidebar on the right side of the screen.
export function WarningList(props){
    const [content, setContent] = useState({});
  
    function getSuggestionList(content, foo){
      // Maps list of suggested alternatives to an <li> object for rendering
      return (
        content[foo].map((suggestion) => <li key={suggestion}>{suggestion}</li>)
      )
    }
    
    const onClick = (warning) => {
      props.addCustomObject(warning);
      let newContent = _.clone(content);
      delete newContent[warning];
      setContent(newContent);
    }

    const listWarnings = Object.keys(content).map((warning) => {
      // Maps unknown suggestions object returned from SocketIO message to HTML toast elements to render.
      if(!props.customObjects.includes(warning)){
       return (
          <div className="warning-card">
            <h5>Token "{warning}" was not recognized. Most similar:</h5>
            <ul>
              {getSuggestionList(content, warning)}
            </ul>
            <Button className="btn btn-outline-danger" variant="outline" onClick={() => onClick(warning)}>Use Anyways</Button>
          </div>
       )
      }
      return "";
    });
  
    useEffect(() => {
      props.socket.on("ReturnUnknownRecommendations", _.debounce(function newToast(data){
        // Retrieving unknown word recommendations from the server, sent as a simple dict
        let suggestions = JSON.parse(data);
        setContent(suggestions)
      }, 200));
    }, [props.socket])
  
    return(
      <div id='toast-container'>{listWarnings}</div>
    )
  }

