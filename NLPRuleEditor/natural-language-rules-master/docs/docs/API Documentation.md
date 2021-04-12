# HTTP Endpoints
**URL**: `/api/translate`

**Methods**: POST

**Details**: Returns a JSON serialized version of a Rule object, along with a translation of said rule into a printable version, and a retranslated version of the rule back into English.

<details>
<summary><strong>POST Details</strong> </summary>
<p>
A translation request can be made with or without a list of custom types that a user has indicated they would like to use in their rule. 

A request body contain two elements: 
* `rule`: a string of the English rule to be translated 
* `customobjects`: a list of strings that represent all the custom object specified in the rule.

As an example post to `/api/translate` with a body formatted like:

```
{
    "rule":"All windows should have a width of 15 inch and are next to a dishwasher.",
    "customobjects":["dishwasher"]
}
```
Will return a response along with a 200 OK if the rule was translated successfully, whose body looks like this:
```
{
  "response": "ALL Object0 = Window\nANY Object1 = dishwasher\n(Object0 Width EQUAL 15.0INCH AND Object0 and Object1 NextTo EQUAL True)", 
  "retranslation": "For all Window objects and any dishwasher object, Window objects must have Width that equals 15.0 inch and Window and dishwasher must have the \"NextTo that is equal to True\" relation.", 
  "rule": "{\"Name\": \"Rule1\", \"Description\": \"All windows should have a width of 15 inch and are next to a dishwasher. \", \"ExistentialClauses\": {\"Object0\": {\"OccurrenceRule\": \"ALL\", \"Characteristic\": {\"Type\": \"Window\", \"PropertyChecks\": []}}, \"Object1\": {\"OccurrenceRule\": \"ANY\", \"Characteristic\": {\"Type\": \"dishwasher\", \"PropertyChecks\": []}}}, \"LogicalExpression\": {\"ObjectChecks\": [{\"ObjName\": \"Object0\", \"Negation\": 0, \"PropertyCheck\": {\"Operation\": \"EQUAL\", \"Value\": 15.0, \"ValueUnit\": \"inch\", \"Name\": \"Width\", \"PCType\": \"NUM\"}}], \"RelationChecks\": [{\"Obj1Name\": \"Object0\", \"Obj2Name\": \"Object1\", \"Negation\": 0, \"PropertyCheck\": {\"Operation\": \"EQUAL\", \"Value\": true, \"Name\": \"NextTo\", \"PCType\": \"BOOL\"}}], \"LogicalExpressions\": [], \"LogicalOperator\": \"AND\"}, \"ErrorLevel\": 0}"
}
```

If the rule cannot be translated, the response will look like:
```
{
    "response": "Sentence has failed to parse.",
}
```

</p>
</details>


***
# SocketIO Events
**Event**: `GetParsedComponents`

**Direction**: Message is sent from client to server

**Details**: Message is sent from client to request that the server parse the recognized components of a rule..

<details>
<p>


A message sent from the client with event   ```GetParsedComponents``` with a body like

```
"All windows should have a width of no less than 15 inch and a height of no more than 2 feet and isabove a sink."
```

will be received by the server, who will eventually respond with a message with the event `ReturnParsedComponents`
</p></details>

***
**Event**: `ReturnParsedComponents`

**Direction**: Message is sent from server to client

**Details**: Server sends JSON serialized version of the list of parsedComponents generated from the rule provided in a preceding message of event `GetParsedComponents` 

<details>
<p>
The server will send a response with the event `ReturnParsedComponents` which consists of something formatted like

```
{"parsedComponents": [{"token": "windows", "category": "Type", "supported": true}, {"token": "width", "category": "Property", "supported": true}, {"token": "no", "category": "Unit", "supported": true}, {"token": "less", "category": "Unit", "supported": true}, {"token": "than", "category": "Unit", "supported": true}, {"token": "15", "category": "Unit", "supported": true}, {"token": "inch", "category": "Unit", "supported": true}, {"token": "height", "category": "Property", "supported": true}, {"token": "no", "category": "Unit", "supported": true}, {"token": "more", "category": "Unit", "supported": true}, {"token": "than", "category": "Unit", "supported": true}, {"token": "2", "category": "Unit", "supported": true}, {"token": "feet", "category": "Unit", "supported": true}, {"token": "sink", "category": "Type", "supported": true}, {"token": "above", "category": "Relation", "supported": true}]}
```
in this case in response to an initial `GetParsedComponents` message sent with `All windows should have a width of no less than 15 inch and a height of no more than 2 feet and is above a sink.`
</p>
</details>

***
**Event**: `ReturnUnknownRecommendations`

**Direction**: Message is sent from server to client

**Details**: Server sends a dictionary for each word marked as unknown with suggestions for the user to consider.
<details>
<p>
As a followup message sent after one of event `ReturnParsedComponents`, returns a dictionary with suggestions for words marked as Unknown

```
{"basins": ["Sink", "CornerCabinets", "CommunicationsAppliance"]}
```
</p></details>