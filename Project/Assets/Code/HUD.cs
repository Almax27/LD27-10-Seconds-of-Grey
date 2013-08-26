using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
	#region public variables
	
	static HUD instance = null;
	public static HUD Instance
	{
		get
		{
			if(instance == null)
			{
				instance = (Instantiate(Resources.Load("HUD")) as GameObject).GetComponent<HUD>();
			}
			return instance;
		}
	}
	
	#endregion
	
	#region public variables
	
	public GUITexture worldKarmaBar = null;
	public GUITexture worldKarmaNub = null;
	public GUITexture conversationKarmaBar = null;
	public GUITexture conversationKarmaNub = null;
	
	public float tweenTime = 0.5f;
	
	public float desiredWorldKarma = 0;
	public Conversation currentConversation = null;
	
	#endregion
	
	#region protected variables
	
	protected float visualWorldKarma = 0;
	protected float visualConversationKarma = 0;
	
	#endregion
	
	#region protected methods
	
	float worldKarmaVelocity = 0;
	float conversationKarmaVelocity = 0;
	
	#endregion
	
	#region monobehaviour methods
	
	// Use this for initialization
	void Awake () 
	{
		if(instance != null)
			Destroy(gameObject);
		else
			instance = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		visualWorldKarma = Mathf.SmoothDamp(visualWorldKarma, desiredWorldKarma, ref worldKarmaVelocity, tweenTime);
		if(currentConversation != null)
			visualConversationKarma = Mathf.SmoothDamp(visualConversationKarma, currentConversation.karma, ref conversationKarmaVelocity, tweenTime);
		else
			visualConversationKarma = 0;
		
		Rect inset;
		
		inset = worldKarmaNub.pixelInset;
		inset.x = (visualWorldKarma/100f)*(worldKarmaBar.pixelInset.width/2-inset.width) - (inset.width/2f);
		worldKarmaNub.pixelInset = inset;
		
		inset = conversationKarmaNub.pixelInset;
		inset.x = (visualConversationKarma/100f)*(conversationKarmaBar.pixelInset.width/2-inset.width) - (inset.width/2f);
		conversationKarmaNub.pixelInset = inset;
	}
	
	#endregion
}
