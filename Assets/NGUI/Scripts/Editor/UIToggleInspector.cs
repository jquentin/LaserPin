//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIToggle))]
public class UIToggleInspector : UIWidgetContainerEditor
{
	enum Transition
	{
		Smooth,
		Instant,
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		NGUIEditorTools.SetLabelWidth(100f);
		UIToggle toggle = target as UIToggle;

		GUILayout.Space(6f);
		GUI.changed = false;

		GUILayout.BeginHorizontal();
		SerializedProperty sp = NGUIEditorTools.DrawProperty("Group", serializedObject, "group", GUILayout.Width(120f));
		GUILayout.Label(" - zero means 'none'");
		GUILayout.EndHorizontal();

		EditorGUI.BeginDisabledGroup(sp.intValue == 0);
		NGUIEditorTools.DrawProperty("  State of 'None'", serializedObject, "optionCanBeNone");
		EditorGUI.EndDisabledGroup();

		// Tiggly addition
		NGUIEditorTools.DrawProperty("Set State at Start", serializedObject, "setStateAtStart");
		if (toggle.setStateAtStart)
		{
			NGUIEditorTools.BeginContents(true);
			NGUIEditorTools.DrawProperty("Starting State", serializedObject, "startsActive");
			NGUIEditorTools.EndContents();
		}
		NGUIEditorTools.SetLabelWidth(80f);

		if (NGUIEditorTools.DrawMinimalisticHeader("State Transition"))
		{
			NGUIEditorTools.BeginContents(true);
			NGUIEditorTools.DrawProperty("Sprite", serializedObject, "activeSprite");
			NGUIEditorTools.DrawProperty("Animation", serializedObject, "activeAnimation");

			if (serializedObject.isEditingMultipleObjects)
			{
				NGUIEditorTools.DrawProperty("Instant", serializedObject, "instantTween");
			}
			else
			{
				GUI.changed = false;
				Transition tr = toggle.instantTween ? Transition.Instant : Transition.Smooth;
				GUILayout.BeginHorizontal();
				tr = (Transition)EditorGUILayout.EnumPopup("Transition", tr);
				NGUIEditorTools.DrawPadding();
				GUILayout.EndHorizontal();

				if (GUI.changed)
				{
					NGUIEditorTools.RegisterUndo("Toggle Change", toggle);
					toggle.instantTween = (tr == Transition.Instant);
					NGUITools.SetDirty(toggle);
				}
			}
			NGUIEditorTools.EndContents();
		}

		NGUIEditorTools.DrawEvents("On Value Change", toggle, toggle.onChange);
		serializedObject.ApplyModifiedProperties();
	}
}
