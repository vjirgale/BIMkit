# BIMkit
## Overview
BIM-kit is a project developed by [Christoph Sydora](https://www.csydora.ca). BIM-kit is a collection of Tools/Applications and Web Services that together store, modify, and evaluate Building Information Modeling (BIM) models. The work is part of his PhD thesis, under the supervision of Dr. Eleni Stroulia at the University of Alberta. Work relating to this project has been published at the EC3 Conference in July 2021 (link will be made available when accessible).
## Components
BIM-kit is made up of a growing number of components, each with a task specific goal. In its current state, the following components have been developed:
1.	[**DBMS**](./DBMS/): Is the central Repository for storing BIM models. It is a web service that controls a [MongoDB](https://www.mongodb.com/) database for the models, objects, user access, and additional BIM related classes. The [AdminApp](./AdminApp/) contains all functionally of the DBMS.
2.	[**BIM-kit Viewer**](./BIMkitViewer/): This is the primary 3D viewer of BIM models from the BIM-kit Repository. This application was developed using the [Unity Game Engine](https://unity.com/). Addons for rule selection and Model Checking have been included; Generative Design development is ongoing and initial variant will be added shortly.
3.	[**RMS**](./RMS/): Controls a [MongoDB](https://www.mongodb.com/) database for the rules, rulesets, user access, and methods used for model evaluation. The [RuleAdminApp](./ RuleAdminApp/) contains all functionally of the RMS.
4.	**Rule Editors**: There are currently three rule editors. The first is the [WFRuleEditor](./WFRuleEditor/) which is a .NET WindowsForm application. Second is the [BlocklyRuleEditor](./BlocklyRuleEditor/) which uses [Google Blockly](https://developers.google.com/blockly) puzzle-like pieces to create rules. Finally, the [NLPRuleEditor](./NLPRuleEditor/) takes input (somewhat) natural language in an attempt to parse it into design rules. (Note: the WFRuleEditor is the most updated editor, while the other two are prototypes for proof of concept purposes are require updates).
5.	[**Model Checking**]](./ModelCheckService/): A service for evaluating a model under a set of design rules. For testing, an individual checking application was developed in the [MCGDApp](./MCGDApp/)
6.	[**Generative Design**](./GenerativeDesignService/) is a service for automatically adding furnishing to a model based on specified design rule constraints. This project was ordinally done as part a MSc project by Christoph and is currently being migrated over to the new code base.
7.	[**Model Simulation**](./ModelSimulation/) is an application for evaluating a model using Unity Simulation. This project is ongoing and part of a project on virus transmission within a building and will be added at a future date.
8.	**Additional Libraries and Tools**: [MathPackage](./MathPackage/) is library that contains some useful vector and mesh methods. [ModelConverter](./ModelConverter/) is an application for converting Industry Foundation Class (IFC) and .obj files into the format used in BIM-kit.
Each Component has individual README files with further details and instructions.
## Testing Instructions
The tools required for running BIM-kit are:
-	[Unity](https://unity.com/) (2019.3.12f1 prefered)
-	[MongoDB](https://www.mongodb.com/)
-	[Visual Studios](https://visualstudio.microsoft.com/downloads/) (2019) .NET 4.7.2

As the majority of projects (aside from the NLP and Blockly Rule Editors) are written in C#, all projects are accessible via a single Visual Studio solution file. Assuming you have MongoDB installed, to run the applications, simply open [BIMkit.sln](./BIMkit.sln) in Visual Studios and run the project. All .NET application will run simultaneously. You may be required to modify the URL for your local MongoDB instance in the DBMS and RMS.

The Unity projects, which are the BIM-kit Viewer and ModelSimulation, require running the [scene](./BIMkitViewer/Assets/Scenes/) file in the Unity environment
