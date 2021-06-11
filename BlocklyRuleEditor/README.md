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

### 1.1 buttons and input
### 1.5 blockly workspace
This is the main section. On the left there is a toolbox that can be used to drag in blocks. In the main portion blocks can be connected to create rules. If a block is not completed (for example a child block is missing) a warning icon will pop up on the top left of the block. This icon can be clicked to display a hint indicating the issue. Click the icon again to close the hint.
### 1.6 nlr textarea
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
