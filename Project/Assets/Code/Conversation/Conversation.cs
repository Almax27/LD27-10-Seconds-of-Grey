using UnityEngine;
using System.Collections.Generic;

public class Conversation : MonoBehaviour {
	
	#region public variables
	
	public float karma = 0;
	
	public GUISkin guiSkin = null;
	
	public int defaultChoiceCount = 3;
	public int depth = 0;
	public List<ConversationChoice> currentChoices = new List<ConversationChoice>();
	public bool aiTurn = false;
	public ConversationChoice lastChoice = null;
	
	#endregion
	
	
	// Use this for initialization
	void Start () 
	{
		currentChoices = GetRootChoicesAtRandom(defaultChoiceCount);
		
		aiTurn = false;
		ProcessChoice(AIPickChoice());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI()
	{
		GUISkin tempGUISkin = GUI.skin;
		GUI.skin = guiSkin;
		
		GUILayout.BeginArea(new Rect(Screen.width/2f -100f, Screen.height/2f -100f, Screen.width/2f, Screen.height/2f));
		
		GUILayout.BeginVertical("box", GUILayout.Width(200));
		
		GUILayout.Label(lastChoice.text);
		for(int i = 0; i < currentChoices.Count; i++)
		{
			ConversationChoice choice = currentChoices[i];
			if(GUILayout.Button(choice.text, GUILayout.Width(200)))
			{
				ProcessChoice(choice);
				ProcessChoice(AIPickChoice());
				break;
			}
		}
		
		GUILayout.EndVertical();
		
		GUILayout.EndArea();
		
		GUI.skin = tempGUISkin;
	}
	
	void ProcessChoice(ConversationChoice choice)
	{
		lastChoice = choice;
		if(choice.responces.Count > 0)
		{
			currentChoices = choice.responces;
		}
		else
		{
			currentChoices = GetRootChoicesAtRandom(defaultChoiceCount);
		}
	}
	
	ConversationChoice AIPickChoice()
	{
		return currentChoices[Random.Range(0,currentChoices.Count)];
	}
	
	#region static helpers
	
	static ConversationData dataLoaded;
	public static ConversationData Data
	{
		get
		{
			if(dataLoaded == null)
			{
				dataLoaded = XMLUtil.LoadResource<ConversationData>("ConversationData");
			}
			return dataLoaded;
		}
	}
	
	public static List<ConversationChoice> GetRootChoicesAtRandom(int count)
	{
		List<ConversationChoice> choices = new List<ConversationChoice>(Data.choices);
		List<ConversationChoice> chosen = new List<ConversationChoice>();
		for(int i = 0; i < count; i++)
		{
			int index = Random.Range(0,choices.Count);
			chosen.Add(choices[index]);
			choices.RemoveAt(index);
		}
		return chosen;
	}
	
	#endregion
}
