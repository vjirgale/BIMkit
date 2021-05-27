# Project Overview
This project is intended to allow easier customization of rules and regulations to be evaluated against a Building Image Modelling (BIM) model, a task which as of now requires intimate familiarity with the syntax required for these rules to be evaluated by external programs. This web application allows a user to write a set of rules in plain English and translate these rules into a more specific rule language, one which can be exported to a file and easily imported into an evaluation program. It is intended to be used by any users working with BIM, regardless of their familiarity with the rule language or the ways in which this language is interpreted by external model evaluation programs. This project will serve as an intermediary layer between creating written requirements for a project and evaluation of these requirements in modelling software.


***
# Project Glossary
* **Building Information Modeling (BIM)** - The digital process of creating and representing buildings and other construction elements using software such as Autodesk Revit. 
* **Rule** - An individual constraint or suggestion for a building, such as a building code or guideline.
* **Ruleset** - A collection of rules, all of which should apply to a building under different circumstances.
* **Design Rule Language (DRL)** - The representation of a rule in a more precise format easy to understand and apply to a BIM model. Developed by Christoph Sydora, see [his paper](https://era.library.ualberta.ca/items/161fc740-e93b-4908-879e-ed71eb944bf5) on the subject.
* **Types** - A component of a rule in Christoph's rule language. Types are the different kinds of objects which exist within a building. For example, walls, sinks, cabinets, and so on.
* **Units** - A component of a rule in Christoph's rule language. Units are any measure, such as distance, weight, volume, and so on.
* **Relations** - A component of a rule in Christoph's rule language. Relations are the connection between different types in the rule. For example, the keyword BETWEEN might be a relation limitation between two different types.
* **Properties** - A component of a rule in Christoph's rule language. Properties are the inherent values of each member of a type. For example, a sink might be required to be of a certain length to pass the rule.

***
# Storyboarding
![Storyboard for Application](https://i.imgur.com/x6mFl0R.png)

***
# User Stories

| **ID** | **Name** | **Priority** | **Description** | **Tests** |
| ----------- | ----------- | ----------- | ----------- | ----------- |
| US 1 | Input Natural Language | Must Have | As a user, I want to be able to type a rule in natural language, so that I can express my desired rule without needing to know any specific syntax. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Write a new rule in plain english in the provided text input box. </li><li>Follow provided popup suggestions if writing is not fully recognized.</li><li>Click the Submit button.</li><li>Confirm that the rule generated is semantically the same as what you have written by cross-referencing the reverse translation provided.</li></ol> |
| US 2 | Key Word Highlighting | Should Have | As a user, I want key words (such as relations, types, units, and properties) to be highlighted and color coded as I type so that I can confirm that the text I output is being registered properly. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Write a new rule in plain english in the provided text input box.</li><li>Confirm that:<ol><li>All units that have been written are highlighted in the correct colour</li><li>All relations that have been written are highlighted in the correct colour</li><li>All properties that have been written are highlighted in the correct colour</li><li>All types that have been written are highlighted in the correct colour</li></ol></li></ol> |
| US 3 | Unknown Word Highlighting | Must Have | As a user, I want to be made aware when I use words that are not understood by the system, so that I can avoid semantic errors in the output. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Write a new rule in plain english in the provided text input box. Include a word that the system is not built to recognize, eg. “bagel”.</li><li>Confirm that the word has been highlighted red.</li><li>Delete the word, and confirm that the highlighting disappears with it.</li></ol> |
| US 4 | Unknown Word Suggestions | Must Have | As a user, I want to be provided suggestions for key words not understood by the system, so that I can easily correct my input. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Write a new rule in plain english in the provided text input box. Include a word that the system is not built to recognize, eg. “bagel”.</li><li>Confirm that the word has been highlighted red, and that a popup with possible suggestions has appeared.<ol><li>Select a suggestion and confirm that the unrecognized word has been changed</li><li>Delete the word, and confirm that the highlighting and popup have disappeared.</li></ol></li></ol> |
| US 5 | Rule Conversion to DRL | Must Have | As a user, I want the rules that I write in English to be converted to DRL, so that I can use the newly formatted rules to evaluate a 3D model in an external program. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li></ol>|
| US 6 | JSON Export | Must Have | As a user, I want to export a ruleset of DRL formatted rules to JSON, so that I can save them and import them into my evaluation program. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li><li>Repeat steps 1-7 for a few additional rules.</li><li>Click the export button.</li><li>Ensure that a JSON file is downloaded, and that the JSON file matches the rule in the spreadsheet.</li></ol> |
| US 7 | DRL Conversion to English | Would Have | As a user, I want to see the generated DRL formatted rules translated back to English, so that I can verify that the intent of my rules have been properly expressed by the system. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li><li>Ensure that the English output that is re-translated below the design language version is semantically identical to the rule you wrote initially.</li></ol> |
| US 8.0 | Ruleset Creation | Should Have | As a user, I want to be able to create a new ruleset. | <ol><li>Open the main page.</li><li>Create a new ruleset.</li><li>Ensure that the ruleset was created.</li></ol> |
| US 8.1 | Ruleset Name Editing | Should Have | As a user, I want to be able to edit the name of my ruleset, so that I can adjust the title if necessary. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Click on the name of the ruleset in the navigation bar.</li><li>Write a new name in the text box that appears.</li><li>Click outside of the box, and confirm that the name for the ruleset has been modified.</li></ol> |
| US 8.2 | Ruleset Rule Deletion | Should Have | As a user, I want to be able to remove a rule from my ruleset, so that I can remove it if it is not necessary. |<ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li><li>On the navigation bar, click the X icon next to the selected rule.</li><li>Ensure the rule has been deleted.</li></ol>|
| US 8.3 | Ruleset Rule Addition | Should Have | As a user, I want to be able to add a new rule to my ruleset, so that I can expand upon the rules I have already. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li><li>On the navigation bar, click the + icon.</li><li>Ensure that a new rule has been added to the list.</li></ol> |
| US 9 | Web service | Must Have | As a user/administrator, I want to be able to access the tool through a web browser, so that I can use it easily from anywhere. | <ol><li>Open the main page on a variety of internet connected devices.</li></ol> |
| US 10 | English Rule Editing | Should Have | As a user, I want to be able to edit my original English version of a rule, so that I can easily change the rule and the generated DRL formatted version of the rule to better fit my needs. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li><li>Modify the original English input to change the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the changes you made.</li></ol> |
| US 11 | DRL Editing | Could Have | As a user, I want to be able to edit the DRL formatted rule generated by the system, so that I can ensure the translated version keeps the intent of my original rule. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule. Export the ruleset to a JSON file.</li><li>Click on various elements of the translated rule and modify them.</li><li>Export the ruleset again. Ensure that the exportable JSON file has changed.</li></ol> |
| US 12 | Rule Ordering | Could Have | As a user, I want to be able to confirm the ordering I intend for certain rules is captured properly by the system, so that I know the DRL translated version is semantically identical to my original English rule. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box. Ensure it is one where ordering in the final rule is important.</li><li>Use any suggestions provided in popups as necessary to ensure the system fully registers the rule.</li><li>Click the ‘Submit’ button</li><li>Ensure that the rule output in the design language is correct with the entry in the spreadsheet corresponding to your English rule.</li><li>Using the popup that occurs, switch the ordering of the rule.</li><li>Ensure that the rule has been modified.</li></ol> |
| US 13 | Virtual Objects | Must Have | As a user, I want to be able to create a new virtual object if a desired Type is not available in the system, so that I can create rules based on Types not already defined within the system. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>In the textbox, write a new type not defined by the system.</li><li>Confirm that a popup asks if you would like to create a new object.</li><li>Complete the rule and Submit it.</li><li>Confirm that the object is in the outputted rule.</li></ol> |
| US 14 | Inclusive or Exclusive | Would Have | As a user, I want to be able to specify whether an "or" in my rule is inclusive or exclusive, so that I know the rule is semantically correct. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Using the examples provided in the ‘Rules in Multiple Formats’ spreadsheet in the ‘401 Rule NLP’ Google Drive folder, input a rule in English into the text input box. Ensure it is a rule containing ‘or’.</li><li>Ensure that a popup asking if the or is inclusive or exclusive pops up.<ol><li>Analyze and convert the rule without clicking. Ensure that it matches the default inclusive setting.</li><li>Click the option, then convert the rule. Ensure that it matches the exclusive setting.</li></ol></li></ol> |
| US 15 | Rule Priority | Must Have | As a user, I want to be able to choose from different priority options for a rule, so that I can express how certain rules have higher or lower priority than others. | <ol><li>Open the main page. </li><li>Create a new ruleset.</li><li>Modify the priority of the rule.</li><li>Create a rule from the provided spreadsheet of examples.</li><li>Ensure that the priority is saved properly when a JSON file is exported.</li></ol> |
| US 16 | Key Word List Editing | Must Have | As an administrator, I want to be able to edit lists of relations, types, units, and properties, so that the app stays up to date with changes in the interior design industry. | <ol><li>Replace the supplied list of keywords with a new one.</li><li>Ensure the importation process goes well by checking console output.</li><li>Test with a variety of rules to ensure the terms added are now supported.</li></ol> |
| US 17 | JSON Import | Should Have | As a user, I want to be able to import JSON rules, so that I can edit them. | <ol><li>Open the main page. </li><li>Select the Import option. </li><li>Select an appropriate JSON file.</li><li>Ensure that all rules appear in the navigation bar after input, and that all rules are as they were before the JSON was exported.</li></ol> |
| US 18 | More robust English to DRL Conversion | Must Have | This is a continuation of US 5, as we repurposed US 5 to just be a simplistic implementation, so that we could finish it in one sprint. This user story is for tracking the rest of the functionality that was originally planned for US 5 | |

**Must Haves**

* 1, 3, 4, 5, 6, 9, 13, 15, 16, 18

**Should Haves**

* 2, 8, 10

**Would Haves**

* 7, 14

**Could Haves**

* 11, 12, 17

***
# Technical Resources
* **Backend**: [Flask](https://palletsprojects.com/p/flask/) with [PostgreSQL](https://www.postgresql.org/)
* **Frontend**: [React](https://reactjs.org/) using [Create-React-App](https://create-react-app.dev/)
    * **React libraries**:
        * [Draft.js](https://draftjs.org/)
        * [React Popup](https://react-popup.elazizi.com/)
        * [Ant Design](https://ant.design/)

* **Deployment**: [Cybera](https://www.cybera.ca/) with [Gunicorn](https://gunicorn.org/)
* **Other Resources of Note**: [spaCy.io](https://spacy.io/) for some NLP functionality

***
# Similar Products

### [Rita-DSL (live demo)](https://rita-demo.herokuapp.com/)
* Given a set of rules written in RITA (a regex-like language) and natural language, matches words in the text that fit the specified rules.
* Functionally very different (actually a live demo of RITA, which can be found [here](https://github.com/zaibacu/rita-dsl)). However, this live demo has a UI that achieves much of what we are aiming to do. It takes input in, analyzes it, and prints JSON output on the same page.
* We could also potentially use the RITA language itself in this project. I have not determined if this is practical or not.

# Open-Source Products

### [FRET: Formal Requirements Elicitation Tool](https://github.com/NASA-SW-VnV/fret)
* Requirements elicitation tool for entering requirements into a domain-specific rule language
* Highlights words in different colours corresponding to the different parts of a requirement as parsing occurs
* Allows for creation of rules in rulesets
* Text is entered in a mixture of natural language and domain-specific language terms, so parsing is somewhat similar but not the same as what we intend to do. But the UI and ways of interaction is something worth keeping an eye on.

### [In2sql](https://github.com/FerreroJeremy/ln2sql)
* Tool to convert natural language into SQL queries
* Not in the domain of our project, but SQL query translation shares similarities with what we are aiming to accomplish
* Given an SQL database dump, a selected language, and input phrase, outputs an SQL query to match this phrase.
* While this tool is limited in terms of parsing ability (it does not recognize plural versus singular, and specific key phrases are somewhat required), it does use a customizable thesaurus file for the given language being used to allow for improved recognition if a specific phrase is not being recognized properly.
