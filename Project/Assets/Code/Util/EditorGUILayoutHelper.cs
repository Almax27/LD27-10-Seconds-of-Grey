using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class EditorGUILayoutHelper
{
	
	#region static variables
	
	static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();
	
	#endregion
	
	#region static helper methods
	
	public static void VerticalTextFieldList(string _title, string _foldoutName, List<string> _text)
	{
		GUILayout.BeginVertical("box");

		//title
		GUILayout.BeginHorizontal();
		
		if(!foldouts.ContainsKey(_foldoutName))
			foldouts.Add(_foldoutName, true);
		
		EditorGUILayout.BeginVertical(GUILayout.Width(197));

		bool foldout = foldouts[_foldoutName] = EditorGUILayout.Foldout(foldouts[_foldoutName], _title);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Label");
		if(GUILayout.Button("+", GUILayout.MaxWidth(20))) 
		{
			_text.Insert(0, string.Empty);
			Deselect();
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.EndHorizontal();
		
		if(foldout)
		{
			EditorGUI.indentLevel++;

			//name data
			for(int i = 0; i < _text.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				
				_text[i] = EditorGUILayout.TextField(_text[i], GUILayout.Width(200));
				
				if(GUILayout.Button("+", GUILayout.Width(20))) 
				{
					_text.Insert(++i, string.Empty);
					Deselect();
				}
				
				if(GUILayout.Button("-", GUILayout.Width(20)))
				{
					_text.RemoveAt(i--);
					Deselect();
				}
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUI.indentLevel--;
		}
		
		EditorGUILayout.EndVertical();
	}
	
	public static void Deselect()
	{
		GUIUtility.hotControl = 0; //release hot control
		GUIUtility.keyboardControl = -1; //release keyboard 
	}
	
	#endregion
}
