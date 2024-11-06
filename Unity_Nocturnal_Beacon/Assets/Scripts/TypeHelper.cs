using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
public static class TypeHelper
{
    public static List<Type> GetAllDerivedTypes<T>() where T : class
    {
        List<Type> derivedTypes = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(T)))
                {
                    derivedTypes.Add(type);
                }
            }
        }


        return derivedTypes;
    }
}