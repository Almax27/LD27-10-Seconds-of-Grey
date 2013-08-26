using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ConversationDataEditor : EditorWindow 
{
	#region public variables
	
	#endregion
	
	#region protected variables
	
	protected ConversationData data = null;
	
	protected int toolbarIndex = 0;
	protected string[] toolbarSelections = {"Questions", "Statements"};
	
	protected Vector2 scrollPos = Vector2.zero;	
	
	protected GUIStyle boldStyle = null;
	
	#endregion
	
	#region private methods
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Tools/Conversation Data Editor")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		ConversationDataEditor window = (ConversationDataEditor)EditorWindow.GetWindow (typeof (ConversationDataEditor));
		
		window.data = XMLUtil.LoadResource<ConversationData>(ConversationData.FileName);
	}
	
	void OnGUI () 
	{
		if(boldStyle == null)
		{
			boldStyle = new GUIStyle(GUI.skin.label);
			boldStyle.fontStyle = FontStyle.Bold;
		}
		
		if(data == null)
		{
			GUILayout.Label("No data found");
			if(GUILayout.Button("Create new data"))
			{
				data = new ConversationData();
				XMLUtil.SaveResource(ConversationData.FileName, data);
			}
		}
		else
		{
			GUILayout.BeginHorizontal();
			
			if(GUILayout.Button("Save"))
			{
				XMLUtil.SaveResource(ConversationData.FileName, data);
			}
			if(GUILayout.Button("Load"))
			{
				data = XMLUtil.LoadResource<ConversationData>(ConversationData.FileName);
			}
			if(GUILayout.Button("Clear"))
			{
				if(EditorUtility.DisplayDialog("Warning!", "This will clear all data, are you sure?", "yes", "no"))
				{
					data = new ConversationData();
				}
			}
			
			GUILayout.EndHorizontal();
			
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			
			ChoiceList(data.choices, 0);
			
			GUILayout.EndScrollView();
		}
	}
	
	void ChoiceList(List<ConversationChoice> choices, int depth)
	{		
		GUILayout.BeginVertical();
		
		GUILayout.Label("Player Choice: " + depth, boldStyle);
		
		for(int j = 0; j < choices.Count; j++)
		{
			ConversationChoice choice = choices[j];
			
			GUILayout.BeginHorizontal("box");
			
				GUILayout.BeginVertical(GUILayout.Width(100));
				
					GUILayout.BeginHorizontal();
					
						if(GUILayout.Button("-", GUILayout.Width(20))) 
							choices.RemoveAt(j);
						
						EditorGUILayout.MinMaxSlider(ref choice.minKarma, ref choice.maxKarma, -100, 100, GUILayout.Width(100));
						choice.karmaReward = EditorGUILayout.FloatField(choice.karmaReward, GUILayout.Width(50));
					
					GUILayout.EndHorizontal();
			
					if(choice.text == "")
						GUI.color = Color.red;
			
					choice.text = GUILayout.TextArea(choice.text, GUILayout.MinWidth(100));
			
					GUI.color = Color.white;
					
				GUILayout.EndVertical();
				
				if(choice.responces.Count == 0)
				{
					//always have at least 1 responce
					choice.responces.Add(new ConversationChoice());
				}
				else
				{
					AIReponce(choice.responces, depth);
				}
			
			GUILayout.EndHorizontal();
			
		}
		
		if(GUILayout.Button("Add Choice", GUILayout.Width(100))) 
			choices.Add(new ConversationChoice());
		
		GUILayout.EndVertical();
	}
	
	void AIReponce(List<ConversationChoice> choices, int depth)
	{
		GUILayout.BeginVertical();
		
		GUILayout.Label("AI Reponce: " + depth, boldStyle);
		
		for(int j = 0; j < choices.Count; j++)
		{
			ConversationChoice choice = choices[j];
			
			GUILayout.BeginHorizontal("box");
			
			if(GUILayout.Button("-", GUILayout.Width(20))) 
				choices.RemoveAt(j);
			
			if(choice.text == "")
				GUI.color = Color.red;
	
			choice.text = GUILayout.TextArea(choice.text, GUILayout.MinWidth(100));
	
			GUI.color = Color.white;
			
			if(choice.responces.Count == 0)
			{
				if(GUILayout.Button("Add Choice", GUILayout.Width(100))) 
					choice.responces.Add(new ConversationChoice());
			}
			else
			{
				ChoiceList(choice.responces, depth+1);
			}
			
			GUILayout.EndHorizontal();
		}
		
		if(GUILayout.Button("Add Reponce", GUILayout.Width(100))) 
			choices.Add(new ConversationChoice());
		
		GUILayout.EndVertical();
	}
	
	#endregion
}