//Function to parse the JSON file received from the socket.io connection
export function ParseJson(parsedComponentsString){
  // Input: JSON string received from server with event "ReturnParsedComponents"
  // Output: Returns an object with all tokens sorted into each category, for colouring
  let parsedComponents = JSON.parse(parsedComponentsString);
  let categories = {
    types: [],
    properties: [],
    relations: [],
    units: [],
    unknowns: []
  }
  Object.values(parsedComponents).forEach(weirdArrayThing => {
    Object.values(weirdArrayThing).forEach(parsedComponent => {
      try{
        switch(parsedComponent.category){
          case "Type":
            categories.types.push(parsedComponent.token);
            break;
          case "Property":
            categories.properties.push(parsedComponent.token);
            break;
          case "Relation":
            categories.relations.push(parsedComponent.token);
            break;
          case "Unit":
            categories.units.push(parsedComponent.token);
            break;
          case "Unknown":
            categories.unknowns.push(parsedComponent.token);
            break;
          default:
            break;
        }
      }catch(err){
        console.log(err);
      }
    });
    
  });
  return categories;
}

