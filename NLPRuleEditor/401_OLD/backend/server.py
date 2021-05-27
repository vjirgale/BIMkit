from flask import Flask, request, abort
from flask_cors import CORS
from flask_socketio import SocketIO, emit
from parsing import Parser
import json
app = Flask(__name__)
cors = CORS(app,resources={r"/api/*":{"origins":"*"}})
socketio = SocketIO(app, cors_allowed_origins="*")
parser = Parser()

@app.route('/api/test')
def index():
    return {'message': 'Hello! If you are reading this, the API is working!'}

# Skeleton version of translation endpoint. Just repeats back what was sent.
@app.route('/api/translate', methods=['POST'])
def translate():
    if request.get_json():
        json = request.get_json()
        if 'rule' in json.keys():
            # We parse the rule, then return the string version 
            if 'customobjects' in json.keys():
                custObjs = json['customobjects']
            else:
                custObjs = []
            try:
                parsedRule = parser.englishToDRL(json["rule"], custObjs)
                responseStr = parsedRule.rule.toString()
            except Exception as e:
                print(e)
                responseStr = "Sentence has failed to parse."
                return {'response': responseStr}
            if responseStr.count('(') != responseStr.count(')'):
                responseStr = "Sentence has failed to parse."
                return {'response': responseStr}
            return {'response': responseStr, 'rule': parsedRule.rule.toJson(), 'retranslation': parsedRule.rule.toEnglish()}
    abort(400)

@socketio.on('GetParsedComponents')
def handle_color_request(string):
    parsedComponents = parser.getComponentsToJSON(string)
    print(parsedComponents)
    emit('ReturnParsedComponents', parsedComponents)
    # We also want it to emit unknown word recommendations
    unknownWordRecommendations = parser.unknownSuggestions(parser.parseComponents(string))
    print(unknownWordRecommendations)
    emit('ReturnUnknownRecommendations', json.dumps(unknownWordRecommendations))