# Overview
This is a web based application that is a GUI for creating and editing rules. This application integrates Blockly, an open-source software, to create a block-based visual for creating kitchen design rules. Blockly hompage: https://developers.google.com/blockly

To run this application download the project and open the index.html file located in the blocklyApplication folder using Google Chrome.

# Documentation
To develop this application download the project and open the blocklyApplication folder in a code editor.

## Table of Contents
1. index.html
2. WokspaceFunctions
3. Natural_Language_Rules
4. CustomBlockLibrary
5. libraries

## 1. index.html

### 1.1 buttons and ruleset name input
These are located at the top of the page. JS files related to these elements can be found in the ButtonFunctions folder. The **Import Ruleset** button is used to import a .json file containing a ruleset object into the blockly workspace. **Export Ruleset** button is used to export the current blockly workspace into a .json file. **Clear Workspace** button can be used to delete all blocks in the blockly workspace. The **Ruleset name:** input is used to update the name of .json file created and exported when **Export Ruleset** button is clicked.

### 1.2 blockly workspace
This is the main section. On the left there is a toolbox with categories that can be clicked to display blocks than can be dragged into the workspace. To add/delete/modify a category update the toolbox xml element in index.html. The blocks in each category can be updated by modifying the child elements of the toolbox element.
See https://developers.google.com/blockly/guides/configure/web/toolbox.

In the main portion blocks can be connected to create rules. If a block is not completed (for example a child block is missing) a warning icon will pop up on the top left of the block. This icon can be clicked to display a hint indicating the issue. Click the icon again to close the hint. Blocks can be deleted by selecting them and pressing the delete key on your keyboard or by dragging and dropping them on the toolbox or garbage icon located at the bottom-left of the workspace.


### 1.3 nlr textarea
This section is to the right of the blockly workspace. It contains a translation of the current blockly workspace in the nlr json format. This text is updated whenever the blockly workspace is updated. Currently updating the text area will not have an effect on the blockly workspace. On the right of this section there is a button that can be used to hide and show the text area. In addition the space between the blockly workspace and nlr textarea can be draged left and right to resize the two elements.

## 2. WorkspaceFunctions Folder
This folder contains js files that include functions that are triggered from events such as clicking a button or modifying the workspace.
### 2.1 ClearWorkspace.js
### 2.2 ExportRuleset.js
### 2.3 ImportRuleset.js
### 2.4 OnChange.js
### 2.5 RenameRuleset.js
### 2.6 TextAreaStyle.js

## 3. Natural_Language_Rules


## 4. CustomBlockLibrary


## 5. Libraries
