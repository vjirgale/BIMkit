# Overview
This is a web based application that is a GUI for creating and editing rules. This application integrates Blockly, an open-source software, to create a block-based visual for creating kitchen design rules. Blockly hompage: https://developers.google.com/blockly

To run this application download the project and open the index.html file located in the blocklyApplication folder using Google Chrome.

# Documentation
To develop this application download the project and open the blocklyApplication folder in a code editor.

## Table of Contents
1. index.html
2. Rules
3. CustomBlockLibrary
4. Libraries
5. Examples

## 1. index.html
This is the main and only html page of the application.
### 1.1 buttons and ruleset name input
These are located at the top of the page. JS files related to these elements can be found in the ButtonFunctions folder. The **Import Ruleset** button is used to import a .json file containing a ruleset object into the blockly workspace. **Export Ruleset** button is used to export the current blockly workspace into a .json file. **Clear Workspace** button can be used to delete all blocks in the blockly workspace. The **Ruleset name:** input is used to update the name of .json file created and exported when **Export Ruleset** button is clicked.

### 1.2 blockly workspace
On the left there is a toolbox with categories that can be clicked to display blocks than can be dragged into the workspace. To add/delete/modify a category update the toolbox xml element in index.html. The blocks in each category can be updated by modifying the child elements of the appropriate category element. Examples can be found at in the examples.

In the main section blocks can be connected to create rules. If a block is not completed (for example a child block is missing) a warning icon will pop up on the top left of the block. This icon can be clicked to display a hint indicating the issue. Click the icon again to close the hint. Blocks can be deleted by selecting them and pressing the delete key on your keyboard or by dragging and dropping them on the toolbox or garbage icon located at the bottom-left of the workspace. The workspace has an onchange event handler (located in index.html) that is fired whenever a block is created/modified/moved/deleted.

### 1.3 nlr textarea
This section is to the right of the blockly workspace. It contains a translation of the current blockly workspace in json format. The onchange event handler is used to update this definition whenever the blockly workspace is modified. Functions used in the onchange event handler can be found in the OnChange.js file located in TextareaFunctions folder. Currently updating the text area will not have an effect on the blockly workspace. On the right of this section there is a button that can be used to hide and show the text area. In addition the space between the blockly workspace and nlr textarea can be draged left and right to resize the two elements. JS files related to these stylings can be found in TextAreaStyle.js in the TextareaFunctions folder.


## 2. Rules
This folder contains two folders **DesignRules** folder and **BlockToRuleTranslate** folder. The **DesignRules** folder contains .js files with classes of design ruleset components. The **BlockToRuleTranslate** folder contains .js files for translating a rule block to a design rule. These files also contain a funcions to convert the class into xml. This is used when importing blocks.

## 3. CustomBlockLibrary
This folder contains files containing the definitions for blocks used in the blockly workspace. 

## 4. Libraries
This application uses Blockly See: https://developers.google.com/blockly and FileSaver See: https://github.com/eligrey/FileSaver.js/

# Examples
## Create a new Category
in index.html go to the element with id = "toolbox".
![plot](./Images/BlocklyApplicationCategory.PNG)
