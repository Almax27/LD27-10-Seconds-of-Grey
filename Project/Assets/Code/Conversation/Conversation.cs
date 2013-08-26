using UnityEngine;
using System.Collections.Generic;

public class Conversation : MonoBehaviour {
	
	public enum State
	{
		Entering,
		Idle,
		Exiting
	}
	#region public variables
	
	public float karma = 0;
	
	public GUISkin guiSkin = null;
	
	public int defaultChoiceCount = 3;
	public int depth = 0;
	public List<ConversationChoice> currentChoices = new List<ConversationChoice>();
	public bool aiTurn = false;
	public ConversationChoice lastChoice = null;
	
	public NPCController NPC = null;
	
	public Transform[] thingsToScale = new Transform[0];
	
	public TextMesh AIText = null;
	public TextMesh[] playerChoiceText = null;
	
	public Color defaultColor = Color.black;
	public Color hoverColor = Color.black;
	
	#endregion
	
	#region protected variables
	
	float animTime = 1;
	State state = State.Entering;
	float tick = 0;
	
	#endregion
	
	#region public methods
	
	public void Exit()
	{
		state = State.Exiting;
		tick = animTime;
	}
	
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
		float scale = 1;
		
		switch(state)
		{
		case State.Entering:
			//do animation
			if(tick < animTime)
			{
				tick += Time.deltaTime;
				scale = Easing.Ease(tick, 0f, 1f, animTime, Easing.Method.BackOut);
			}
			else
			{
				tick = animTime;
				transform.localScale = Vector3.one;
				state = State.Idle;
			}
			break;
		case State.Idle:
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			//do hovering
			for(int i = 0; i < playerChoiceText.Length; i++)
			{
				TextMesh text = playerChoiceText[i];
				if(text.collider.Raycast(r, out hit, 100))
				{
					if(Input.GetMouseButtonUp(0))
					{
						karma += currentChoices[i].karmaReward;
						ProcessChoice(currentChoices[i]);
						ProcessChoice(AIPickChoice());
					}
					text.color = hoverColor;
				}
				else
					text.color = defaultColor;
			}
			break;
		case State.Exiting:
			if(tick > 0)
			{
				tick -= Time.deltaTime;
				scale = Easing.Ease(tick, 0f, 1f, animTime, Easing.Method.BackOut);
			}
			else
			{
				Destroy(gameObject);
			}
			break;
		}
		for(int i =0; i < thingsToScale.Length; i++)
		{
			thingsToScale[i].localScale = Vector3.one * scale;
		}
	}
	
	void OnGUI()
	{
		/*
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
		*/
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
		for(int i = 0; i < playerChoiceText.Length; i++)
		{
			if(i < currentChoices.Count)
			{
				playerChoiceText[i].gameObject.SetActive(true);
				playerChoiceText[i].text = currentChoices[i].text;
			}
			else
			{
				playerChoiceText[i].gameObject.SetActive(false);
				playerChoiceText[i].text = "";
			}
		}
	}
	
	ConversationChoice AIPickChoice()
	{
		ConversationChoice choice = currentChoices[Random.Range(0,currentChoices.Count)];
		AIText.text = choice.text;
		return choice;
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
