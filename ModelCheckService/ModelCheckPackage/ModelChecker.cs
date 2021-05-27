using DbmsApi;
using DbmsApi.API;
using Microsoft.CSharp;
using RuleAPI.Methods;
using RuleAPI.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelCheckPackage
{
    public class ModelChecker
    {
        private static Dictionary<ObjectTypes, MethodInfo> VOMethods;
        private static Dictionary<string, MethodInfo> PropMethods;
        private static Dictionary<string, MethodInfo> RelMethods;

        public RuleCheckModel Model { get; private set; }
        public List<RuleCheckObject> VirtualObjectsCreated { get; private set; }
        public List<Rule> Rules { get; private set; }
        public List<Tuple<Rule, Type, MethodInfo>> CompiledRules { get; private set; }
        public List<RuleResult> RuleResults { get; private set; }

        #region Setup Methods:

        public ModelChecker(Model model, List<Rule> rules)
        {
            // Add all the Functions:
            GetAllMethods();
            SetNewRules(rules);
            SetNewModel(model);
        }

        public void SetNewModel(Model model)
        {
            Model = new RuleCheckModel(model);
            CheckAndCreateTypesIfNeeded();

            RuleResults = new List<RuleResult>();
        }

        public void SetNewRules(List<Rule> rules)
        {
            Rules = rules;
            CompiledRules = GetCompiledRules();

            RuleResults = new List<RuleResult>();
        }

        public void SetNewRules(List<Tuple<Rule, Type, MethodInfo>> compiledRules)
        {
            Rules = compiledRules.Select(cr=>cr.Item1).ToList();
            CompiledRules = compiledRules;
        }

        public void GetAllMethods()
        {
            VOMethods = MethodFinder.GetAllVOMethodInfos();
            PropMethods = MethodFinder.GetAllPropertyInfos();
            RelMethods = MethodFinder.GetAllRelationInfos();
        }

        public void CheckAndCreateTypesIfNeeded()
        {
            VirtualObjectsCreated = new List<RuleCheckObject>();

            // Create any virtual objects needed
            List<ObjectTypes> createdTypes = new List<ObjectTypes>();
            foreach (Rule rule in Rules)
            {
                foreach (ExistentialClause ec in rule.ExistentialClauses.Values)
                {
                    if (VOMethods.ContainsKey(ec.Characteristic.Type) && (!createdTypes.Contains(ec.Characteristic.Type)))
                    {
                        List<RuleCheckObject> newVirtualObjs = CreateVirtualType(Model, ec.Characteristic.Type);
                        VirtualObjectsCreated.AddRange(newVirtualObjs);
                        Model.Objects.AddRange(newVirtualObjs);

                        createdTypes.Add(ec.Characteristic.Type);
                    }
                }
            }
        }

        public List<RuleCheckObject> CreateVirtualType(RuleCheckModel model, ObjectTypes objectType)
        {
            MethodInfo methodInfo = VOMethods[objectType];
            object[] parametersArray = new object[] { model };
            List<RuleCheckObject> returnedObjects = (List<RuleCheckObject>)methodInfo.Invoke(this, parametersArray);
            return returnedObjects;
        }

        private List<Tuple<Rule, Type, MethodInfo>> GetCompiledRules()
        {
            if (Rules == null)
            {
                throw new Exception("Need to set rules");
            }

            List<Tuple<Rule, Type, MethodInfo>> returnRuleVal = new List<Tuple<Rule, Type, MethodInfo>>();
            foreach (Rule rule in Rules)
            {
                // quick rule sanity checks such as making sure the RelationCheck indecies are not the same in a single check:
                if (!ValidateRule(rule))
                {
                    continue;
                }

                // Generate the code:
                string ruleString = GenerateRuleCode(rule);

                // Compile the rule:
                Tuple<Type, MethodInfo> compileResult = CompileRule(rule, ruleString);

                returnRuleVal.Add(new Tuple<Rule, Type, MethodInfo>(rule, compileResult.Item1, compileResult.Item2));
            }

            return returnRuleVal;
        }

        #endregion

        #region Check Methods:

        public List<RuleResult> CheckModel(double defaultForNoOutput)
        {
            if (Model == null || Rules == null)
            {
                throw new Exception("Need to set rules and/or model");
            }

            if (CompiledRules == null)
            {
                CompiledRules = GetCompiledRules();
            }

            RuleResults = new List<RuleResult>();
            foreach (Tuple<Rule, Type, MethodInfo> compiledRule in CompiledRules)
            {
                Rule rule = compiledRule.Item1;
                Stopwatch timer = new Stopwatch();
                timer.Start();

                // Compile the rule:
                Tuple<Type, MethodInfo> compileResult = new Tuple<Type, MethodInfo>(compiledRule.Item2, compiledRule.Item3);

                // Execute the rule:
                Tuple<RuleResult, RuleResult> results = ExecuteRule(compileResult, rule, this, defaultForNoOutput);

                timer.Stop();
                TimeSpan ts = timer.Elapsed;
                results.Item1.Runtime = ts;
                results.Item2.Runtime = ts;
                RuleResults.Add(results.Item1);
            }

            return RuleResults;
        }

        public static bool ValidateRule(Rule rule)
        {
            // TODO: More Validation could be added
            if (rule.ExistentialClauses.Any(ec => ec.Value.OccurrenceRule != OccurrenceRule.NONE) && rule.ExistentialClauses.Any(ec => ec.Value.OccurrenceRule == OccurrenceRule.NONE))
            {
                // This would always fail...
                return false;
            }

            if (string.IsNullOrWhiteSpace(rule.Name))
            {
                return false;
            }

            return true;
        }

        public static bool RuleRelevance(Rule rule)
        {
            // TODO: Until Relevance is determined, this will return true
            return true;
        }

        public static string GenerateRuleCode(Rule rule)
        {
            string returnVal = "";
            returnVal += "using System.Collections.Generic;";
            returnVal += "using System.Linq;";
            returnVal += "using RuleAPI.Models;";
            returnVal += "namespace ModelCheckPackage";
            returnVal += "{";
            returnVal += "    public class Rule" + rule.Id;
            returnVal += "    {";
            returnVal += "        public static List<RuleInstance> Execute(Rule rule, ModelChecker modelCheck)";
            returnVal += "        {";
            returnVal += "            var ruleInstances = new List<RuleInstance>();";
            int ecCount = 0; //existentialClauseCount
            List<string> objs = new List<string>();
            foreach (var ecKvp in rule.ExistentialClauses)
            {
                returnVal += "            var " + ecKvp.Key + "s = new List<KeyValuePair<string, RuleCheckObject>>();";
                ecCount++;
            }
            returnVal += "            foreach (RuleCheckObject obj in modelCheck.Model.Objects)";
            returnVal += "            {";
            ecCount = 0;
            foreach (var ecKvp in rule.ExistentialClauses)
            {
                returnVal += "                if (ModelChecker.CheckIfObjectHasCharacteristics(obj, rule.ExistentialClauses[\"" + ecKvp.Key + "\"].Characteristic))";
                returnVal += "                {";
                returnVal += "                    " + ecKvp.Key + "s.Add(new KeyValuePair<string, RuleCheckObject>(\"" + ecKvp.Key + "\", obj));";
                returnVal += "                }";
                ecCount++;
            }
            returnVal += "            }";
            ecCount = 0;
            foreach (var ecKvp in rule.ExistentialClauses)
            {
                returnVal += "            foreach (var obj" + ecCount + " in " + ecKvp.Key + "s)";
                returnVal += "            {";

                objs.Add("obj" + ecCount);
                ecCount++;
            }
            if (ecCount > 1)
            {
                List<string> objEqString = new List<string>();
                for (int i = 0; i < ecCount; i++)
                {
                    for (int j = i + 1; j < ecCount; j++)
                    {
                        objEqString.Add("(obj" + i + ".Value == obj" + j + ".Value)");
                    }
                }
                returnVal += "            if (" + string.Join(" || ", objEqString) + ")";
                returnVal += "            {";
                returnVal += "                continue;";
                returnVal += "            }";
            }
            returnVal += "                var objList = new List<KeyValuePair<string, RuleCheckObject>>() { " + string.Join(", ", objs) + " };";
            returnVal += "                double passed = ModelChecker.GetRuleInstanceResult(modelCheck, rule, objList.ToDictionary(x => x.Key, x => x.Value));";
            returnVal += "                ruleInstances.Add(new RuleInstance(objList.Select(x=>x.Value).ToList(), passed, rule));";
            foreach (var ecKvp in rule.ExistentialClauses)
            {
                returnVal += "        }";
            }
            returnVal += "            return ruleInstances;";
            returnVal += "        }";
            returnVal += "    }";
            returnVal += "}";

            returnVal.Replace("\\", "");
            return returnVal;
        }

        public static Tuple<Type, MethodInfo> CompileRule(Rule rule, string ruleString)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters
            {
                // True - memory generation, false - external file generation
                GenerateInMemory = false,
                // True - exe file generation, false - dll file generation
                GenerateExecutable = false
                //OutputAssembly = "CompiledRules.dll"
            };
            parameters.ReferencedAssemblies.Add(Assembly.GetCallingAssembly().Location);
            parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(ModelChecker)).Location);
            parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Rule)).Location);
            parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Model)).Location);
            parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(Enumerable)).Location);

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, ruleString);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }
                throw new InvalidOperationException(sb.ToString());
            }
            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("ModelCheckPackage.Rule" + rule.Id);
            MethodInfo execute = program.GetMethod("Execute");

            return new Tuple<Type, MethodInfo>(program, execute);
        }

        public static Tuple<RuleResult, RuleResult> ExecuteRule(Tuple<Type, MethodInfo> compileResult, Rule rule, ModelChecker modelCheck, double defaultForNoOutput)
        {
            Type program = compileResult.Item1;
            MethodInfo execute = compileResult.Item2;
            object classInstance = Activator.CreateInstance(program, null);
            object[] parametersArray = new object[] { rule, modelCheck };
            List<RuleInstance> output = execute.Invoke(classInstance, parametersArray) as List<RuleInstance>;

            //input for RFunction keeps track of Clause position in output
            List<KeyValuePair<int, ExistentialClause>> positions = new List<KeyValuePair<int, ExistentialClause>>();

            //orders existential clauses so that ALL clauses are first
            int i = 0;
            foreach (var ecKvp in rule.ExistentialClauses)
            {
                KeyValuePair<int, ExistentialClause> clause = new KeyValuePair<int, ExistentialClause>(i, ecKvp.Value);
                if (ecKvp.Value.OccurrenceRule == OccurrenceRule.ALL)
                {
                    //sorts all cases to the start of the list
                    positions.Insert(0, clause);
                }
                else
                {
                    positions.Add(clause);
                }

                i++;
            }
            //calls recursive function
            double rulePassVal = RFunction(output, positions, defaultForNoOutput);

            List<RuleInstance> outputInt = output.Where(o => o.PassVal != 1.0).ToList();
            RuleResult resultInt = new RuleResult(rule, rulePassVal, outputInt);
            RuleResult result = new RuleResult(rule, rulePassVal, output);

            return new Tuple<RuleResult, RuleResult>(result, resultInt);
        }

        public static double RFunction(List<RuleInstance> output, List<KeyValuePair<int, ExistentialClause>> positions, double defaultForNoOutput)
        {
            if (output.Count == 0)
            {
                return defaultForNoOutput;
            }

            double passes = 1.0;
            if (positions[0].Value.OccurrenceRule == OccurrenceRule.ANY)
            {
                passes = 0.0;
            }
            //recursion ends on last clause
            if (positions.Count == 1)
            {
                //checks if final layer passes based on Occurrence rule
                foreach (RuleInstance rule in output)
                {
                    if (positions[0].Value.OccurrenceRule == OccurrenceRule.ALL)
                    {
                        passes = passes * rule.PassVal;
                    }
                    if (positions[0].Value.OccurrenceRule == OccurrenceRule.ANY)
                    {
                        passes = Math.Max(passes, rule.PassVal);
                    }
                    if (positions[0].Value.OccurrenceRule == OccurrenceRule.NONE)
                    {
                        passes = passes * (1 - rule.PassVal);
                    }
                }
            }
            else
            {
                //splits output list by objects in current existential clause
                Dictionary<RuleCheckObject, List<RuleInstance>> objPassedCounter = new Dictionary<RuleCheckObject, List<RuleInstance>>();
                foreach (RuleInstance instance in output)
                {
                    RuleCheckObject obj = instance.Objs[positions[0].Key];
                    if (objPassedCounter.ContainsKey(obj))
                    {
                        objPassedCounter[obj].Add(instance);
                    }
                    else
                    {
                        objPassedCounter[obj] = new List<RuleInstance>() { instance };
                    }
                }

                //loops through each split list
                foreach (KeyValuePair<RuleCheckObject, List<RuleInstance>> objectList in objPassedCounter)
                {
                    KeyValuePair<int, ExistentialClause> T = positions[0];
                    //Removes Existential Clause so that the recursive function uses the next Existential Clause
                    positions.RemoveAt(0);
                    //calls function again for each split list
                    double returnPass = RFunction(objectList.Value, positions, defaultForNoOutput);
                    //adds Existential Clause back so that the foreach loop can continue
                    positions.Insert(0, T);

                    //checks if current level passes based on OccurrenceRule
                    if (positions[0].Value.OccurrenceRule == OccurrenceRule.ALL)
                    {
                        passes = passes * returnPass;
                    }
                    if (positions[0].Value.OccurrenceRule == OccurrenceRule.ANY)
                    {
                        passes = Math.Max(passes, returnPass);
                    }
                    if (positions[0].Value.OccurrenceRule == OccurrenceRule.NONE)
                    {
                        passes = passes * returnPass;
                    }
                }
            }
            return passes;
        }

        #endregion

        #region Static functions that are used in the compiled model check rules:

        public static bool CheckIfObjectHasCharacteristics(RuleCheckObject obj, Characteristic charic)
        {
            // check that the object matches the characteristic type:
            if (!RecusiveTypeCheck(charic.Type, ObjectTypeTree.GetNode(obj.Type)))
            {
                return false;
            }

            // The object has to pass all Characteristic Property Checks:
            bool allCharacterChecksPass = true;
            foreach (PropertyCheck pCheck in charic.PropertyChecks)
            {
                Property props = GetOrAddPropertyToObject(obj, pCheck.Name);
                double checkPassed = OnePropertyPassesPropertyCheck(props, pCheck);

                if (checkPassed != 1.0)
                {
                    // A check has failed thus the object does not match all characterisitics:
                    allCharacterChecksPass = false;
                    break;
                }
            }

            return allCharacterChecksPass;
        }

        public static bool RecusiveTypeCheck(ObjectTypes checkType, ObjectType objType)
        {
            if (objType == null)
            {
                return false;
            }
            if (objType.ID == checkType)
            {
                return true;
            }
            return RecusiveTypeCheck(checkType, objType.Parent);
        }

        public static double OnePropertyPassesPropertyCheck(Property property, PropertyCheck pCheck)
        {
            double checkPassed = 0.0;
            if (pCheck.GetType() == typeof(PropertyCheckBool) && property.Type == PropertyType.BOOL)
            {
                PropertyCheckBool propertyCheckBool = pCheck as PropertyCheckBool;
                checkPassed = propertyCheckBool.CheckProperty(property as PropertyBool);
            }
            if (pCheck.GetType() == typeof(PropertyCheckNum) && property.Type == PropertyType.NUM)
            {
                PropertyCheckNum propertyCheckBool = pCheck as PropertyCheckNum;
                checkPassed = propertyCheckBool.CheckProperty(property as PropertyNum);
            }
            if (pCheck.GetType() == typeof(PropertyCheckString) && property.Type == PropertyType.STRING)
            {
                PropertyCheckString propertyCheckBool = pCheck as PropertyCheckString;
                checkPassed = propertyCheckBool.CheckProperty(property as PropertyString);
            }

            return checkPassed;
        }

        public static Property GetOrAddPropertyToObject(RuleCheckObject obj, string propertyName)
        {
            // Check if the object has the property already:
            if (obj.Properties.ContainsKey(propertyName))
            {
                return obj.Properties[propertyName];
            }

            // Find the function that matches the property:
            Property newProp = null;
            if (PropMethods.ContainsKey(propertyName))
            {
                object returnVal = PropMethods[propertyName].Invoke(null, new object[] { obj });
                if (returnVal.GetType() == typeof(bool))
                {
                    newProp = new PropertyBool(propertyName, (bool)returnVal);
                }
                if (returnVal.GetType() == typeof(double))
                {
                    newProp = new PropertyNum(propertyName, (double)returnVal);
                }
                if (returnVal.GetType() == typeof(string))
                {
                    newProp = new PropertyString(propertyName, (string)returnVal);
                }
            }
            newProp = newProp ?? new PropertyString(propertyName, "null");
            obj.Properties.Add(newProp);

            return newProp;
        }

        public static Property GetOrAddPropertyToRelation(RuleCheckRelation relation, string propertyName)
        {
            // Check if the relation has the property already:
            if (relation.Properties.ContainsKey(propertyName))
            {
                return relation.Properties[propertyName];
            }

            Property newProp = null;
            if (RelMethods.ContainsKey(propertyName))
            {
                object returnVal = RelMethods[propertyName].Invoke(null, new object[] { relation });
                if (returnVal.GetType() == typeof(bool))
                {
                    newProp = new PropertyBool(propertyName, (bool)returnVal);
                }
                if (returnVal.GetType() == typeof(double))
                {
                    newProp = new PropertyNum(propertyName, (double)returnVal);
                }
                if (returnVal.GetType() == typeof(string))
                {
                    newProp = new PropertyString(propertyName, (string)returnVal);
                }
            }
            newProp = newProp ?? new PropertyString(propertyName, "null");
            relation.Properties.Add(newProp);

            return newProp;
        }

        public static RuleCheckRelation FindOrCreateObjectRelation(RuleCheckModel model, RuleCheckObject obj1, RuleCheckObject obj2)
        {
            List<RuleCheckRelation> matchingRelations = model.Relations.Where(rel => rel.FirstObj == obj1 && rel.SecondObj == obj2).ToList();
            if (matchingRelations.Count > 0)
            {
                return matchingRelations.First();
            }
            RuleCheckRelation newObjRel = new RuleCheckRelation(obj1, obj2);
            model.Relations.Add(newObjRel);

            return newObjRel;
        }

        public static double GetRuleInstanceResult(ModelChecker modelCheck, Rule rule, Dictionary<string, RuleCheckObject> objects)
        {
            return GetLogicalExpressionResult(modelCheck, rule.LogicalExpression, objects);
        }

        public static double GetLogicalExpressionResult(ModelChecker modelCheck, LogicalExpression logicExp, Dictionary<string, RuleCheckObject> objects)
        {
            double result = 1.0;
            bool firstResult = true;
            foreach (ObjectCheck oc in logicExp.ObjectChecks)
            {
                Property props = GetOrAddPropertyToObject(objects[oc.ObjName], oc.PropertyCheck.Name);
                double ocResult = OnePropertyPassesPropertyCheck(props, oc.PropertyCheck);
                ocResult = oc.Negation == Negation.MUST_HAVE ? ocResult : 1.0 - ocResult;
                if (firstResult)
                {
                    result = ocResult;
                    firstResult = false;
                }
                else
                {
                    switch (logicExp.LogicalOperator)
                    {
                        case (LogicalOperator.AND):
                            result = result * ocResult;
                            break;
                        case (LogicalOperator.OR):
                            result = Math.Max(result, ocResult);
                            break;
                        case (LogicalOperator.XOR):
                            result = Math.Abs(result - ocResult);
                            break;
                    }
                }
            }
            foreach (RelationCheck rc in logicExp.RelationChecks)
            {
                RuleCheckRelation rel = FindOrCreateObjectRelation(modelCheck.Model, objects[rc.Obj1Name], objects[rc.Obj2Name]);
                Property props = GetOrAddPropertyToRelation(rel, rc.PropertyCheck.Name);
                double rcResult = OnePropertyPassesPropertyCheck(props, rc.PropertyCheck);
                rcResult = rc.Negation == Negation.MUST_HAVE ? rcResult : 1.0 - rcResult;
                if (firstResult)
                {
                    result = rcResult;
                    firstResult = false;
                }
                else
                {
                    switch (logicExp.LogicalOperator)
                    {
                        case (LogicalOperator.AND):
                            result = result * rcResult;
                            break;
                        case (LogicalOperator.OR):
                            result = Math.Max(result, rcResult);
                            break;
                        case (LogicalOperator.XOR):
                            result = Math.Abs(result - rcResult);
                            break;
                    }
                }
            }
            foreach (LogicalExpression le in logicExp.LogicalExpressions)
            {
                double leResult = GetLogicalExpressionResult(modelCheck, le, objects);
                if (firstResult)
                {
                    result = leResult;
                    firstResult = false;
                }
                else
                {
                    switch (logicExp.LogicalOperator)
                    {
                        case (LogicalOperator.AND):
                            result = result * leResult;
                            break;
                        case (LogicalOperator.OR):
                            result = Math.Max(result, leResult);
                            break;
                        case (LogicalOperator.XOR):
                            result = Math.Abs(result - leResult);
                            break;
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
