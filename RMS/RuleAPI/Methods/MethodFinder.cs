using DbmsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RuleAPI.Methods
{
    public static class MethodFinder
    {
        public static readonly double SchrinkAmount = 1.0;

        public static Dictionary<string, Type> GetAllPropertyMethods()
        {
            MethodInfo[] methodInfos = typeof(PropertyMethods).GetMethods(BindingFlags.Public | BindingFlags.Static);

            Dictionary<string, Type> methodNames = new Dictionary<string, Type>();
            foreach (var methodInfo in methodInfos)
            {
                methodNames.Add(methodInfo.Name, methodInfo.ReturnType);
            }
            return methodNames;
        }

        public static Dictionary<string, Type> GetAllRelationMethods()
        {
            MethodInfo[] methodInfos = typeof(RelationMethods).GetMethods(BindingFlags.Public | BindingFlags.Static);

            Dictionary<string, Type> methodNames = new Dictionary<string, Type>();
            foreach (var methodInfo in methodInfos)
            {
                methodNames.Add(methodInfo.Name, methodInfo.ReturnType);
            }
            return methodNames;
        }

        public static Dictionary<ObjectTypes, string> GetAllVOMethods()
        {
            MethodInfo[] methodInfos = typeof(VirtualObjects).GetMethods(BindingFlags.Public | BindingFlags.Static);

            Dictionary<ObjectTypes, string> methodNames = new Dictionary<ObjectTypes, string>();
            foreach (var methodInfo in methodInfos)
            {
                ObjectTypes methodVOType = Enum.GetValues(typeof(ObjectTypes)).Cast<ObjectTypes>().Where(e => Enum.GetName(typeof(ObjectTypes), e).Contains(methodInfo.Name)).First();
                methodNames.Add(methodVOType, methodInfo.Name);
            }
            return methodNames;
        }

        public static Dictionary<ObjectTypes, MethodInfo> GetAllVOMethodInfos()
        {
            MethodInfo[] methodInfos = typeof(VirtualObjects).GetMethods(BindingFlags.Public | BindingFlags.Static);

            Dictionary<ObjectTypes, MethodInfo> methodInfoDict = new Dictionary<ObjectTypes, MethodInfo>();
            foreach (var methodInfo in methodInfos)
            {
                ObjectTypes methodVOType = Enum.GetValues(typeof(ObjectTypes)).Cast<ObjectTypes>().Where(e => Enum.GetName(typeof(ObjectTypes), e).Contains(methodInfo.Name)).First();
                methodInfoDict.Add(methodVOType, methodInfo);
            }
            return methodInfoDict;
        }

        public static Dictionary<string, MethodInfo> GetAllPropertyInfos()
        {
            MethodInfo[] methodInfos = typeof(PropertyMethods).GetMethods(BindingFlags.Public | BindingFlags.Static);

            Dictionary<string, MethodInfo> methodInfoDict = new Dictionary<string, MethodInfo>();
            foreach (var methodInfo in methodInfos)
            {
                methodInfoDict.Add(methodInfo.Name, methodInfo);
            }
            return methodInfoDict;
        }

        public static Dictionary<string, MethodInfo> GetAllRelationInfos()
        {
            MethodInfo[] methodInfos = typeof(RelationMethods).GetMethods(BindingFlags.Public | BindingFlags.Static);

            Dictionary<string, MethodInfo> methodInfoDict = new Dictionary<string, MethodInfo>();
            foreach (var methodInfo in methodInfos)
            {
                methodInfoDict.Add(methodInfo.Name, methodInfo);
            }
            return methodInfoDict;
        }
    }
}