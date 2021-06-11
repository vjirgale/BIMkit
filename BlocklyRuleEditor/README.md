# Overview
This is a web based application that is a GUI for creating and editing rules. This application integrates Blockly, an open-source software, to create a block-based visual for creating kitchen design rules. Blockly hompage: https://developers.google.com/blockly

To run this application download the project and open the index.html file located in the blocklyApplication folder.

To develop this application download the project and open the blocklyApplication folder in a code editor. 

# Documentation
To develop this application download the project and open the blocklyApplication folder in a code editor.

## Table of Contents
1. index.html
2. WokspaceFunctions
3. Natural_Language_Rules
4. CustomBlockLibrary
5. libraries

## 1. Main Page/Index.html
This page contains 6 main elements:
### 1.1 import ruleset button
This button is used to import .json files containing the definition of a ruleset (see Ruleset.js). After clicking this button a window will pop up to allow you to select a local file to import. If the import is successful the blockly workspace will add block visuals for each rule specified in the imported file. If there is an error an alert will pop up notifying the user of what went wrong.
### 1.2 clear workspace button
Clicking this button clears all blocks from the workspace.
### 1.3 export ruleset button
This button is used to export a .json file containing the definition of a ruleset that corresponds to the current blockly workspace. This button is green if the ruleset is valid and red if it is not.Blocks that are not connected to a rule block will be ignored. This file can be imported by using the "Import Ruleset" button.
### 1.4 name ruleset input
This input is used to name the .json file that is created when "Export Ruleset" button is used. The exported file's name will be value+.json where value is the text written in the input.
### 1.5 blockly workspace
This is the main section. On the left there is a toolbox that can be used to drag in blocks. In the main portion blocks can be connected to create rules. If a block is not completed (for example a child block is missing) a warning icon will pop up on the top left of the block. This icon can be clicked to display a hint indicating the issue. Click the icon again to close the hint.
### 1.6 nlr textarea
This section is to the right of the blockly workspace. It contains a translation of the current blockly workspace in the nlr json format. This text is updated whenever the blockly workspace is updated. Currently updating the text area will not have an effect on the blockly workspace. On the right of this section there is a button that can be used to hide and show the text area. In addition the space between the blockly workspace and nlr textarea can be draged left and right to resize the two elements.

## 2. WorkspaceFunctions Folder
This folder contains js files that include functions that are triggered from events such as clicking a button or modifying the workspace.
### 2.1 ClearWorkspace.js
### 2.1 ExportRuleset.js
### 2.1 ImportRuleset.js
### 2.1 OnChange.js
### 2.1 RenameRuleset.js
### 2.1 TextAreaStyle.js

## 3. Natural_Language_Rules


## 4. CustomBlockLibrary


## 5. Libraries
