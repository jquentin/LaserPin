using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System;
using UnityEditorInternal;
using System.Reflection;

public abstract class TransferWindow<T> : EditorWindow
{
	static public SortingLayerTransferWindow instance;
	
	protected List<IdNamePair> list = new List<IdNamePair>();
	
	private string textList = "";
	private string printedtextList = "";

	private Vector2 scrollPos = Vector2.zero;
	
	protected class IdNamePair
	{
		public int originalId = 0;
		public string targetName = "";

		public IdNamePair()
		{
		}

		public IdNamePair(int id, string name)
		{
			originalId = id;
			targetName = name;
		}

		public override string ToString ()
		{
			return "" + originalId + " -> " + targetName + "\n";
		}

	}
	
	protected void Update()
	{
		minSize = new Vector2(400f, 560f);
		// This is necessary to make the framerate normal for the editor window.
		Repaint();
	}
	
	protected void OnGUI ()
	{
		int missingEntries = 3- list.Count;
		if (missingEntries > 0)
			for (int i = 0 ; i < missingEntries ; i++)
				list.Add(new IdNamePair());
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		GUILayout.Label("Transfer from values:");
		
		for(int i = 0 ; i < list.Count ; i++)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("id:", GUILayout.Width(25f));
			list[i].originalId = EditorGUILayout.IntField(list[i].originalId, GUILayout.Width(90f));
			GUILayout.Label(" -> name:", GUILayout.Width(60f));
			list[i].targetName = GUILayout.TextField(list[i].targetName, GUILayout.Width(110f));
			if (i == list.Count - 1)
			{
				if (GUILayout.Button("Add 1", GUILayout.Width(50f)))
				{
					list.Add(new IdNamePair());
				}
			}
			GUILayout.EndHorizontal();
		}
		
		GUILayout.Space(10f);
		if (GUILayout.Button("Transfer on all objects in the scene"))
		{
			SetSortingLayerNameToAll();
		}

		GUI.enabled = isSelectionValid;

		if (GUILayout.Button("Transfer on selected objects"))
		{
			SetSortingLayerNameToSelected();
		}

		GUI.enabled = isSelectionValidRecursive;

		if (GUILayout.Button("Transfer on selected objects recursively"))
		{
			SetSortingLayerNameToSelectedRecursive();
		}

		GUI.enabled = true;
		
		GUILayout.Space(20f);
		
		GUILayout.Label("Import list from the text printed by \"Print sorting layers Id->Name pairs\" function");
		
		textList = GUILayout.TextArea(textList, GUILayout.Height(100f));
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Populate list", GUILayout.Width(120f)))
		{
			PopulateListFromText(textList);
		}
		GUILayout.Label("It won't import the Default layer or the unknow layers");
		GUILayout.EndHorizontal();
		
		GUILayout.Space(20f);
		
		if (GUILayout.Button("Print sorting layers Id->Name pairs from the project"))
		{
			PrintProjectLayerIdName();
		}
		
		GUILayout.Space(10f);
		
		if (GUILayout.Button("Print sorting layers Id->Name pairs from objects in the scene"))
		{
			PrintObjectsIdName();
		}
		
		if (!string.IsNullOrEmpty(printedtextList))
		{
			GUILayout.TextArea(printedtextList);
			if (GUILayout.Button("Copy to clipboard"))
			{
				EditorGUIUtility.systemCopyBuffer = printedtextList;
			}
		}
		
		GUILayout.Space(10f);
		
		if (isSelectionValid)
		{
			GUILayout.Label(selectionState);
		}
		
		GUILayout.EndScrollView();
		
	}
	
	protected abstract string selectionState
	{
		get;
	}
	
	protected abstract bool isSelectionValid
	{
		get;
	}

	protected abstract bool isSelectionValidRecursive
	{
		get;
	}

	protected abstract List<T> allObjectsInScene
	{
		get;
	}
	
	protected abstract List<T> allObjectsInSelection
	{
		get;
	}

	protected abstract List<T> allObjectsInSelectionRecursive
	{
		get;
	}
	
	public void SetSortingLayerNameToAll()
	{
		foreach (T component in allObjectsInScene)
			Applytransfer(component);
	}
	
	public void SetSortingLayerNameToSelected()
	{
		foreach(T component in allObjectsInSelection)
			Applytransfer(component);
	}

	public void SetSortingLayerNameToSelectedRecursive()
	{
		foreach(T component in allObjectsInSelectionRecursive)
			Applytransfer(component);
	}
	
	protected abstract void Applytransfer(T component);
	
	public void PopulateListFromText(string text)
	{
		List<IdNamePair> newList = new List<IdNamePair>();
		string[] entries = text.Split(new string[]{"\n"}, System.StringSplitOptions.RemoveEmptyEntries);
		foreach(string entry in entries)
		{
			string[] entryValues = entry.Split(new string[]{"->"}, System.StringSplitOptions.RemoveEmptyEntries);
			int id;
			if (entryValues == null || entryValues.Length != 2)
			{
				Debug.LogError("Could not parse the list.\nMake sure you copied the text returned by \"Print sorting layers Id->Name pairs\" function.");
				break;
			}
			
			if (!int.TryParse(entryValues[0], out id))
			{
				Debug.LogError("Could not parse the list.\nMake sure you copied the text returned by \"Print sorting layers Id->Name pairs\" function.");
				break;
			}
			string name = entryValues[1].Trim();
			if (string.IsNullOrEmpty(name))
			{
				Debug.LogError("Could not parse the list.\nMake sure you copied the text returned by \"Print sorting layers Id->Name pairs\" function.");
				break;
			}
			if (id != 0 && !name.Equals("<unknown layer>"))
			{
				IdNamePair newEntry = new IdNamePair();
				newEntry.originalId = id;
				newEntry.targetName = name;
				newList.Add(newEntry);
			}
		}
		list = newList;
	}
	
	private void PrintObjectsIdName()
	{
		printedtextList = GetSceneObjectsIdName();
	}

	private void PrintProjectLayerIdName()
	{
		printedtextList = GetProjectLayerIdName();
	}
	
	public abstract string GetSceneObjectsIdName();
	
	public abstract string GetProjectLayerIdName();

}

public class PhysicsLayerTransferWindow : TransferWindow<GameObject>
{

	protected override string selectionState
	{
		get
		{
			return "Currently selected GameObject: \n" + Selection.gameObjects[0].name 
					+ "\nId = " + Selection.gameObjects[0].layer
					+ "\nName = " + LayerMask.LayerToName(Selection.gameObjects[0].layer);
		}
	}
	
	protected override bool isSelectionValid
	{
		get
		{
			return (Selection.activeGameObject != null);
		}
	}
	
	protected override bool isSelectionValidRecursive
	{
		get
		{
			return (Selection.activeGameObject != null);
		}
	}

	protected override List<GameObject> allObjectsInScene
	{
		get
		{
			return new List<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>());
		}
	}
	
	protected override List<GameObject> allObjectsInSelection
	{
		get
		{
			return new List<GameObject>(Selection.gameObjects);
		}
	}

	private static List<GameObject> GetAllChildren(Transform t)
	{
		List<GameObject> res = new List<GameObject>();
		res.Add(t.gameObject);
		for ( int i = 0 ; i < t.childCount ; i++)
		{
			res.AddRange(GetAllChildren(t.GetChild(i)));
		}
		return res;
	}

	protected override List<GameObject> allObjectsInSelectionRecursive
	{
		get
		{
			List<GameObject> res = new List<GameObject>();
			foreach(GameObject go in Selection.gameObjects)
				res.AddRange(GetAllChildren(go.transform));

			return res;
		}
	}
	
	protected override void Applytransfer(GameObject go)
	{
		int oldId = go.layer;
		bool transfered = false;
		foreach(IdNamePair entry in list)
		{
			if (entry.originalId != 0 && !string.IsNullOrEmpty(entry.targetName) && go.layer == entry.originalId)
			{
				go.layer = LayerMask.NameToLayer(entry.targetName);
				transfered = true;
				break;
			}
		}
		if (transfered)
		{
			Debug.Log("Transfered object " + go.name 
			          + " with layer id: " + oldId
			          + " to layer named: " + LayerMask.LayerToName(go.layer)
			          + " with layer id: " + go.layer);
		}
	}

	public override string GetSceneObjectsIdName()
	{
		Dictionary<int, string> dic = new Dictionary<int, string>(); 
		foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
		{
			if (!dic.ContainsKey(go.layer))
				dic.Add(go.layer, LayerMask.LayerToName(go.layer));
		}
		string res = "";
		foreach(KeyValuePair<int, string> entry in dic)
			res += new IdNamePair(entry.Key, entry.Value).ToString();
		return res;
	}

	public override string GetProjectLayerIdName()
	{
		string res = "";
		//user defined layers start with layer 8 and unity supports 31 layers
		for(int i=8;i<=31;i++)
		{
			string layerN = LayerMask.LayerToName(i);
			if(!string.IsNullOrEmpty(layerN))
				res += new IdNamePair(i, layerN).ToString();
		}
		return res;
	}

	[MenuItem("Tiggly/Physics Layers/Open Physics Layers Transfer Window")]
	public static void TransferPhysicsLayers()
	{
		EditorWindow.GetWindow<PhysicsLayerTransferWindow>(false, "Physics layers transfer", true).Show();
	}

}

public class SortingLayerTransferWindow : TransferWindow<SpriteRenderer>
{

	protected override string selectionState
	{
		get
		{
			return "Currently selected SpriteRenderer: \n" + Selection.gameObjects[0].name 
					+ "\nId = " + Selection.gameObjects[0].GetComponent<SpriteRenderer>().sortingLayerID
					+ "\nName = " + Selection.gameObjects[0].GetComponent<SpriteRenderer>().sortingLayerName;
		}
	}
	
	protected override bool isSelectionValid
	{
		get
		{
			return (Selection.gameObjects != null 
			        && Selection.gameObjects.Length > 0
			        && Selection.gameObjects[0] != null
			        && Selection.gameObjects[0].GetComponent<SpriteRenderer>() != null);
		}
	}
	
	protected override bool isSelectionValidRecursive
	{
		get
		{
			return (allObjectsInSelectionRecursive != null && allObjectsInSelectionRecursive.Count > 0);
		}
	}
	
	protected override List<SpriteRenderer> allObjectsInScene
	{
		get
		{
			return new List<SpriteRenderer>(Resources.FindObjectsOfTypeAll<SpriteRenderer>());
		}
	}
	
	protected override List<SpriteRenderer> allObjectsInSelection
	{
		get
		{
			List<SpriteRenderer> res = new List<SpriteRenderer>();
			foreach(GameObject go in Selection.gameObjects)
				res.AddRange(go.GetComponents<SpriteRenderer>());
			
			return res;
		}
	}

	private static List<SpriteRenderer> GetAllChildren(Transform t)
	{
		List<SpriteRenderer> res = new List<SpriteRenderer>();
		if (t.GetComponent<SpriteRenderer>())
			res.Add(t.GetComponent<SpriteRenderer>());
		for ( int i = 0 ; i < t.childCount ; i++)
		{
			res.AddRange(GetAllChildren(t.GetChild(i)));
		}
		return res;
	}
	
	protected override List<SpriteRenderer> allObjectsInSelectionRecursive
	{
		get
		{
			List<SpriteRenderer> res = new List<SpriteRenderer>();
			foreach(GameObject go in Selection.gameObjects)
				res.AddRange(GetAllChildren(go.transform));
//				res.AddRange(go.GetComponentsInChildren<SpriteRenderer>(true));
			
			return res;
		}
	}
	
	protected override void Applytransfer(SpriteRenderer sr)
	{
		int oldId = sr.sortingLayerID;
		bool transfered = false;
		foreach(IdNamePair entry in list)
		{
			if (entry.originalId != 0 && !string.IsNullOrEmpty(entry.targetName) && sr.sortingLayerID == entry.originalId)
			{
				sr.sortingLayerName = entry.targetName;
				transfered = true;
				break;
			}
		}
		if (transfered)
		{
			Debug.Log("Transfered object " + sr.name 
			          + " with layer id: " + oldId
			          + " to layer named: " + sr.sortingLayerName
			          + " with layer id: " + sr.sortingLayerID);
		}
	}
	
	public override string GetSceneObjectsIdName()
	{
		Dictionary<int, string> dic = new Dictionary<int, string>(); 
		foreach (SpriteRenderer sr in allObjectsInScene)
		{
			if (!dic.ContainsKey(sr.sortingLayerID))
				dic.Add(sr.sortingLayerID, sr.sortingLayerName);
		}
		string res = "";
		foreach(KeyValuePair<int, string> entry in dic)
			res += new IdNamePair(entry.Key, entry.Value).ToString();
		return res;
	}
	
	public override string GetProjectLayerIdName()
	{
		int[] ids = GetSortingLayerUniqueIDs();
		string[] names = GetSortingLayerNames();
		string res = "";
		for(int i = 0 ; i < ids.Length ; i++)
			res += ids[i] + " -> " + names[i] + "\n";
		return res;
	}

	[MenuItem("Tiggly/Sprite Renderer/Open Sorting Layers Transfer Window")]
	public static void TransferSortingLayers()
	{
		EditorWindow.GetWindow<SortingLayerTransferWindow>(false, "Sorting layers transfer", true).Show();
	}
	
	// Get the sorting layer names
	public static string[] GetSortingLayerNames() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		string[] res = (string[])sortingLayersProperty.GetValue(null, new object[0]);
		foreach(string s in res)
			Debug.Log(s);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
	// Get the unique sorting layer IDs -- tossed this in for good measure
	public static int[] GetSortingLayerUniqueIDs() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		int[] res = ((int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]));
		foreach(int i in res)
			Debug.Log(i);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}
	
}
