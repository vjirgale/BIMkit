//Dropdowns
//Used by Rule
const ErrorLevel = {Recommended : 'Recommended', Warning : 'Warning', Error : 'Error'}
//Used by Rule, LogicalExpression
const LogicalOperator = {AND : 'AND', OR : 'OR', XOR : 'XOR'}
//Used by ECS
const OccuranceRule = {ALL : 'ALL', ANY : 'ANY', NONE : 'NONE'}
//Used by Property
const PropertyNegation = {MUST_HAVE : 'EQUAL', MUST_NOT_HAVE : 'NOT_EQUAL'}
//Used by Relation
const RelationNegation = {MUST_HAVE : 'MUST_HAVE', MUST_NOT_HAVE : 'MUST_NOT_HAVE'}
//Used by Property, Relation
const Unit = {MM:'MM', CM:'CM', M:'M', INCH:'INCH', FT:'FT', DEG:'DEG', RAD:'RAD'}
const OperatorNum = {GREATER_THAN : 'GREATER_THAN', GREATER_THAN_OR_EQUAL : 'GREATER_THAN_OR_EQUAL', EQUAL : 'EQUAL', LESS_THAN : 'LESS_THAN', LESS_THAN_OR_EQUAL : 'LESS_THAN_OR_EQUAL', NOT_EQUAL : 'NOT_EQUAL'}
const OperatorString = {EQUAL:'EQUAL', NOT_EQUAL:'NOT_EQUAL', CONTAINS:'CONTAINS'}



//dropdowns
/* Each dropdown is an array of arrays where the inner arrays consist of two elements that represents each dropdown option.
The first is what will be displayed on the screen when the dropdown is clicked.
The second is what is returned from getValue() */

//Rule
const ErrorLevelDropdown = [[ErrorLevel.Error, ErrorLevel.Error], [ErrorLevel.Warning, ErrorLevel.Warning],[ErrorLevel.Recommended,ErrorLevel.Recommended]];
//Rule, LogicalExpression
const LogicalOperatorDropdown = [[LogicalOperator.AND, LogicalOperator.AND], [LogicalOperator.OR,LogicalOperator.OR],[LogicalOperator.XOR, LogicalOperator.XOR]];
//ECS
const OccuranceRuleDropdown = [[OccuranceRule.ALL, OccuranceRule.ALL],[OccuranceRule.ANY,OccuranceRule.ANY],[OccuranceRule.NONE,OccuranceRule.NONE]];
//Property, Relation
const OperatorNumDropdown = [["=",OperatorNum.EQUAL], ["<",OperatorNum.LESS_THAN], ["<=",OperatorNum.LESS_THAN_OR_EQUAL], [">",OperatorNum.GREATER_THAN], [">=",OperatorNum.GREATER_THAN_OR_EQUAL]];
//Property, Relation
const UnitDropdown = [[Unit.MM, Unit.MM], [Unit.CM, Unit.CM], [Unit.M,Unit.M], [Unit.INCH, Unit.INCH], [Unit.FT, Unit.FT], [Unit.DEG, Unit.DEG], [Unit.RAD, Unit.RAD]];
//Property
const PropertyNegationDropdown = [["Has", PropertyNegation.MUST_HAVE], ["Does not Have", PropertyNegation.MUST_NOT_HAVE]];
//Relation
const RelationNegationDropdown = [["Must be",RelationNegation.MUST_HAVE], ["Must not be",RelationNegation.MUST_NOT_HAVE]];
//Property, Relation
const OperatorStringDropdown = [["=", OperatorString.EQUAL], ["!=", OperatorString.NOT_EQUAL], ["Contains",OperatorString.CONTAINS]];

//ObjectCheck
const ObjectCheckNegationDropdown = [["Must Have",RelationNegation.MUST_HAVE], ["Must Not Have",RelationNegation.MUST_NOT_HAVE]];

//RelationCheck, ObjectCheck 
//when connected to a rule block the check blocks dropdown is dynamically generated.
// See the custom block definitions for relationcheck/objectcheck.
//This array is temporarily updated when importing blocks. see ..\ButtonFunctions\ImportRuleset.js
let DefaultOptions = [['select...','0']];


//ECS dropdowns that are fetched from database
let ECSOptions = [["Pending...","Pending..."],["Table","Table"],["Fridge","Fridge"],["Lamp","Lamp"],["Oven","Oven"]];

//Property dropdowns that are fetched from database
let propertyDistanceMethods = [["Pending...","Pending..."], ["Width","Width"],["Height","Height"], ["Length","Length"]];
let propertyBooleanMethods = [["Pending...","Pending..."],["Light", "Light"]];
let propertyStringMethods   = [["Pending...", "Pending..."],["FunctionOfObj", "FunctionOfObj"]];

//Relation dropdowns that are fetched from database
let relationBooleanMethods = [["Pending...", "Pending..."],["Next to","Next to"],["Above","Above"]];
let relationDistanceMethod = [["Pending...","Pending..."], ["Center Distance","Center Distance"], ["Center DistanceXY","Center DistanceXY"], ["BoundingBoxDistance","Bounding Box"]];
let relationStringMethods  = [["Pending...", "Pending..."],["FunctionOfObj", "FunctionOfObj"]];

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

function fetchDropdowns(){
  fetchPropertyMethods();
  fetchECSMethods();
  fetchRelationMethods();
}

fetchDropdowns();
