/*
This program fetchs methods for properties, relations, and ecs' from METHODS_URL_PROPERTY, METHODS_URL_RELATION, METHODS_URL_ECS
and sets the dropdowns of property, relation and ecs blocks to the fetched methods.
*/

//URLs where methods are fetched from
const METHODS_URL_PROPERTY = "https://localhost:44370/api/method/property";
const METHODS_URL_RELATION = "https://localhost:44370/api/method/relation";
const METHODS_URL_ECS = "https://localhost:44370/api/method/type";

//formats json values into a format that blockly can use
//data = json data of property, relation or type methods.
//type = an array of strings of data types. i.e ['Double', 'Boolean']
function formatData(data, type){
    returnArray = [];
    var keys = Object.keys(data); //methods
    var values = Object.values(data);
    //iterate through methods
    for(var i =0; i < keys.length; i++){
      //iterate through wanted data types
      for(var j = 0; j < type.length; j++){
        //if the methods type is acceptable add to return array
        if(values[i].toLowerCase().includes(type[j].toLowerCase())){
          returnArray.push([keys[i], keys[i]]);
          break;
        }
      }
    }
    return returnArray;
}

//set property methods to defaults
let propertyDistanceMethods = [["Pending...","Pending..."], ["Width","Width"],["Height","Height"], ["Length","Length"]];
let propertyBooleanMethods = [["Pending...","Pending..."],["Light", "Light"]];
let propertyStringMethods   = [["Pending...", "Pending..."],["FunctionOfObj", "FunctionOfObj"]];


//initalize ECS block methods
let ecsOptions = [["Pending...","Pending..."],["Table","Table"],["Fridge","Fridge"],["Lamp","Lamp"],["Oven","Oven"]];




async function fetchPropertyMethods(){
  try{
    var result= await fetchData(METHODS_URL_PROPERTY);
    propertyDistanceMethods  = formatData(result, ['Double']);
    propertyBooleanMethods = formatData(result, ['Boolean']);
    propertyStringMethods  = formatData(result, ['String']);
  }
  catch{
    //TODO: check cache
    console.log("Failed to fetch property methods. Reload page to try again or continue with default.");
    return null;
  }
}
async function fetchRelationMethods(){
  try{
    var result= await fetchData(METHODS_URL_RELATION);
    relationDistanceMethod  = formatData(result, ['Double', 'String']);
    relationBooleanMethods = formatData(result, ['Boolean']);
    relationStringMethods  = formatData(result, ['String']);
  }
  catch{
    console.log("Failed to fetch retlation methods");
  }
}
async function fetchECSMethods(){
  try{
    var result= await fetchData(METHODS_URL_ECS);
    var values = Object.values(result);
    methods = [];
    for(var i =0; i < values.length; i++){
      methods.push([values[i], values[i]]);
    }
    ecsOptions  = methods;
  }
  catch{
    console.log("Failed to fetch ecs methods");
  }
}

//fetch property methods
fetchPropertyMethods();

fetchECSMethods();
