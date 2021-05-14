This floder contains scripts that define functions used for translating the block based visual rule into a JSON equivallent. 
The block based visual can be seen and edited on the left side of the web page.
The JSON definition can be seen on the right hand side and can be closed and opened. TODO: add feature for allowing an update in JSON definition to update the block based visual.?

There are 4 scripts in this folder: 1. BlockJSONECS.js 2.BlockJSONRelations.js 3.BlockJSON.js 4.UpdateBlocksFunction.js

1. BlockJSONECS.js
This file contains functions used for updating the JSON definition for ECS and Property blocks.

2. BlockJSONRelation.js
This file contains functions used for updating the JSON definition for a rules relations input (Corresponds to check and logical expression blocks).

3.BlockJSON.js 
This file contains functions used for creating and printing the JSON definition of a rule block.

4.UpdateBlocksFunction.js
This file contiains functions for moving blocks from one rule block to another so that the json definition updates properly as well.