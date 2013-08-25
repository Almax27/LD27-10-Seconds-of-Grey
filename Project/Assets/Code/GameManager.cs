using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	#region constants
	
	public const float walkingTime = 5;
	public const float conversationTime = 10;
	
	#endregion
	
	#region public variables
	
	public static ConversationData conversationData = null;
	
	public GameObject player = null;
	public float playerSpeed = 10;
	
	public Conversation conversationTemplate = null;
	public GameObject[] worldChunks = new GameObject[0];
	
	public float worldKarma = 0;
	
	public float tick = 0;
	
	#endregion
	
	#region protected variables
	
	protected Conversation currentConversation = null;
	protected List<GameObject> worldChunkInstances = new List<GameObject>();
	
	protected Animator playerAnimator = null;
	
	#endregion
	
	#region public methods
	
	public void StartConversation()
	{
		GameObject gobj = Instantiate(conversationTemplate.gameObject) as GameObject;
		currentConversation = gobj.GetComponent<Conversation>();
		currentConversation.karma = Random.Range(-50,50) + (int)worldKarma/5;;
	}
	
	#endregion
	
	#region monobehaviour methods
	
	// Use this for initialization
	void Start () 
	{
		playerAnimator = player.GetComponentInChildren<Animator>();
		playerAnimator.SetBool("isWalking", true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		tick += Time.deltaTime;
		if(tick > walkingTime + conversationTime)
		{
			worldKarma += currentConversation.karma;
			Destroy(currentConversation);
			currentConversation = null;
			
			playerAnimator.SetBool("isChatting", false);
			playerAnimator.SetBool("isWalking", true);
			
			tick = 0;
		}
		else if(currentConversation == null && tick > walkingTime)
		{
			StartConversation();
			
			playerAnimator.SetBool("isChatting", true);
			playerAnimator.SetBool("isWalking", false);
		}
		else if(tick <= walkingTime)
		{
			player.transform.Translate(0,0,playerSpeed*Time.deltaTime);
		}
		
		if(worldKarma >= 100 || worldKarma <= -100)
		{
			//game over
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
	void OnGUI()
	{
		GUILayout.BeginVertical("box");
		
		GUILayout.Label("World Karma: " + worldKarma);
		
		GUILayout.EndVertical();
	}
	
	#endregion	
}
