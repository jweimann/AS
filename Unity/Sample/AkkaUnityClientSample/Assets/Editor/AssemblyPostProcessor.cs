using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;


[InitializeOnLoad]
public static class AssemblyPostProcessor
{
    static AssemblyPostProcessor()
    {
        PostProcess(Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp.dll");

        //        if( anyAssembliesWereChanged )
        //        {
        //            UnityEditorInternal.InternalEditorUtility.RequestScriptReload(); 
        //        }
    }

    public static void PostProcess(string path)
    {
        Debug.Log("Up and running");

        var assembly = AssemblyDefinition.ReadAssembly(path);

        foreach (var module in assembly.Modules)
        {
            foreach (var type in module.Types)
            {
                if (type.HasMethods)
                {
                    MethodReference injectTestMethod = type.Methods[0];

                    foreach (var method in type.Methods)
                    {
                        if (method.Name == "InjectTest")
                        {
                            injectTestMethod = method;
                        }
                    }

                    foreach (var method in type.Methods)
                    {
                        if (HasAttribute("NRTime", method.CustomAttributes))
                        {
                            ILProcessor ilProcessor = method.Body.GetILProcessor();
                            ilProcessor.InsertBefore(method.Body.Instructions[0], ilProcessor.Create(OpCodes.Call, injectTestMethod));
                            ilProcessor.InsertBefore(method.Body.Instructions[0], ilProcessor.Create(OpCodes.Ldarg_0));
                        }
                    }
                }
            }

            module.Write(path);
        }
    }

    private static bool HasAttribute(string attributeName, IEnumerable<CustomAttribute> customAttributes)
    {
        return GetAttributeByName(attributeName, customAttributes) != null;
    }

    private static CustomAttribute GetAttributeByName(string attributeName, IEnumerable<CustomAttribute> customAttributes)
    {
        foreach (var attribute in customAttributes)
            if (attribute.AttributeType.FullName == attributeName)
                return attribute;
        return null;
    }
}