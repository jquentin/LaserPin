using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class TransformUtils 
{
	
	public static void DestroyChildren(this Transform root) {
		int childCount = root.childCount;
		for (int i = root.childCount - 1 ; i >= 0 ; i--) {
			GameObject.Destroy(root.GetChild(i).gameObject);
		}
	}
	
	public static void DestroyChildrenImmediate(this Transform root, params Transform[] exceptions) {
		List<Transform> excList = new List<Transform>(exceptions);
		int childCount = root.childCount;
		for (int i = root.childCount - 1 ; i >= 0 ; i--) {
			Transform t = root.GetChild(i);
			if (!excList.Contains(t))
				GameObject.DestroyImmediate(t.gameObject);
		}
	}
	
	public static List<Transform> FindChildren(this Transform root, string name) {
		List<Transform> res = new List<Transform>();
		for(int i = 0 ; i < root.childCount ; i++)
		{
			Transform t = root.GetChild(i);
			if (t.name.Contains(name))
				res.Add(t);
			res.AddRange(t.FindChildren(name));
		}
		return res;
	}
	
	public static List<Transform> childrenList(this Transform root) {
		List<Transform> res = new List<Transform>();
		for(int i = 0 ; i < root.childCount ; i++)
		{
			Transform t = root.GetChild(i);
			res.Add(t);
		}
		return res;
	}
	
	public static List<GameObject> FindChildrenGameObjects(this Transform root, string name) {
		List<GameObject> res = new List<GameObject>();
		for(int i = 0 ; i < root.childCount ; i++)
		{
			Transform t = root.GetChild(i);
			if (t.name.Contains(name))
				res.Add(t.gameObject);
			res.AddRange(t.FindChildrenGameObjects(name));
		}
		return res;
	}
	
	/// <summary>
	/// Finds the specified component on the game object or one of its parents.
	/// </summary>
	
	static public T FindInParents<T> (this Transform trans) where T : Component
	{
		if (trans == null) return null;
		T comp = trans.GetComponent<T>();
		if (comp == null)
		{
			Transform t = trans.transform.parent;
			
			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
		}
		return comp;
	}
	
	public static Type GetOrAddComponent<Type>(this GameObject go) where Type : Component
	{
		Type res = go.GetComponent<Type>();
		if (res == null)
			res = go.AddComponent<Type>();
		return res;
	}
	
	public static Type GetOrAddComponent<Type>(this Component c) where Type : Component
	{
		Type res = c.GetComponent<Type>();
		if (res == null)
			res = c.gameObject.AddComponent<Type>();
		return res;
	}

}
