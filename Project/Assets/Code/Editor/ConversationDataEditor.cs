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
		
		GUILayout.Label("Depth: " + depth);
		
		for(int j = 0; j < choices.Count; j++)
		{
			ConversationChoice choice = choices[j];
			
			GUILayout.BeginHorizontal("box");
			
			if(GUILayout.Button("-", GUILayout.Width(20))) 
				choices.RemoveAt(j);
			
			EditorGUILayout.MinMaxSlider(ref choice.minKarma, ref choice.maxKarma, -100, 100, GUILayout.Width(100));
			choice.karmaReward = EditorGUILayout.FloatField(choice.karmaReward, GUILayout.Width(50));
			
			choice.text = GUILayout.TextArea(choice.text, GUILayout.MinWidth(100));
			
			if(choice.responces.Count == 0)
			{
				if(GUILayout.Button("Add Responce", GUILayout.Width(100))) 
					choice.responces.Add(new ConversationChoice());
			}
			else
			{
				ChoiceList(choice.responces, depth+1);
			}
			
			GUILayout.EndHorizontal();
		}
		
		if(GUILayout.Button("Add Choice", GUILayout.Width(100))) 
			choices.Add(new ConversationChoice());
		
		GUILayout.EndVertical();
	}
	
	#endregion
}