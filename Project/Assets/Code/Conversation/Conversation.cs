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
	
	public NPCController NPC = null;
	
	#endregion
	
	
	// Use this for initialization
	void Start () 
	{
		currentChoices = GetRootChoicesAtRandom(defaultChoiceCount, null);
		
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
		
		GUILayout.Label("Karma: " + karma);
		
		GUILayout.Label(lastChoice.text);
		for(int i = 0; i < currentChoices.Count; i++)
		{
			ConversationChoice choice = currentChoices[i];
			if(GUILayout.Button(choice.text, GUILayout.Width(200)))
			{
				karma += choice.karmaReward;
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
		if(choice.responces.Count > 0)
		{
			currentChoices = choice.responces;
		}
		else
		{
			currentChoices = GetRootChoicesAtRandom(defaultChoiceCount, lastChoice);
		}
		lastChoice = choice;
	}
	
	ConversationChoice AIPickChoice()
	{
		return currentChoices[Random.Range(0,currentChoices.Count)];
	}
	
	#region static helpers
	
	static ConversationData dataLoaded;
	public static ConversationData GetData()
	{
		if(dataLoaded == null)
		{
			dataLoaded = XMLUtil.LoadResource<ConversationData>(ConversationData.FileName);
		}
		return dataLoaded;
	}
	
	public static List<ConversationChoice> GetRootChoicesAtRandom(int count, ConversationChoice exclude)
	{
		int minCount = Mathf.Min(count, GetData().choices.Count); 
		
		List<ConversationChoice> choices = new List<ConversationChoice>(GetData().choices);
		List<ConversationChoice> chosen = new List<ConversationChoice>();
		
		while(chosen.Count < minCount)
		{
			ConversationChoice c = choices[Random.Range(0,choices.Count)];
			if(c != exclude)
			{
				chosen.Add(c);
				choices.Remove(c);
			}
		}
		return chosen;
	}
	
	#endregion
}
