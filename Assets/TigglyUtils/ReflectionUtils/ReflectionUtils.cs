using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ReflectionUtils 
{
	
	
	public static List<System.Type> GetSubTypes<T>() where T : class
	{
		var types = new List<System.Type>();
		
		foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
		{
			if (assembly.FullName.StartsWith("Mono.Cecil"))
				continue;
			
			if (assembly.FullName.StartsWith("UnityScript"))
				continue;
			
			if (assembly.FullName.StartsWith("Boo.Lan"))
				continue;
			
			if (assembly.FullName.StartsWith("System"))
				continue;
			
			if (assembly.FullName.StartsWith("I18N"))
				continue;
			
			if (assembly.FullName.StartsWith("UnityEngine"))
				continue;
			
			if (assembly.FullName.StartsWith("UnityEditor"))
				continue;
			
			if (assembly.FullName.StartsWith("mscorlib"))
				continue;
			
			foreach (System.Type type in assembly.GetTypes())
			{
				if (!type.IsClass)
					continue;
				
				if (type.IsAbstract)
					continue;
				
				if(!type.IsSubclassOf(typeof(T)))
					continue;
				
				types.Add(type);
			}
		}
		
		return types;
	}


}
