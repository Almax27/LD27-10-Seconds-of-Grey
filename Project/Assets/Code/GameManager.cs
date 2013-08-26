using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class KarmaSettings
{
	public float tweenSpeed = 50f;
	
	public Texture2D skyGradient = null;
	public Texture2D dirLightGradient = null;
	public Texture2D spotLightGradient = null;
	
	public Vector3 goodLightDir = Vector3.zero;
	public Vector3 badLightDir = Vector3.zero;
	
	public float goodLightIntensity = 0.5f;
	public float badLightIntensity = 0.3f;
	
	public float goodShadowStrength = 0.5f;
	public float badShadowStrength = 1f;
	
	private float visualKarma = 0;
	
	public void Update(float desiredKarma)
	{
		visualKarma = Mathf.MoveTowards(visualKarma, desiredKarma, tweenSpeed * Time.deltaTime);
		
		float t = Mathf.InverseLerp(-100,100,visualKarma);
		
		Color skyColor = skyGradient.GetPixelBilinear(t,0);
		Color dirLightColor = dirLightGradient.GetPixelBilinear(t,0);
		Color spotLightColor = spotLightGradient.GetPixelBilinear(t,0);
		
		Camera.main.backgroundColor = skyColor;
		RenderSettings.fogColor = skyColor;
		
		foreach(Light l in Light.GetLights(LightType.Directional,0))
		{
			l.color = dirLightColor;
			l.transform.rotation = Quaternion.Euler( Vector3.Slerp(badLightDir, goodLightDir, t) );
			l.intensity = Mathf.Lerp(badLightIntensity, goodLightIntensity, t);
			l.shadowStrength = Mathf.Lerp(badShadowStrength, goodShadowStrength, t);
		}
		
		foreach(Light l in Light.GetLights(LightType.Spot,0))
		{
			l.color = dirLightColor;
		}
	}
}

public class GameManager : MonoBehaviour {
	
	#region constants
	
	public const float walkingTime = 5;
	public const float conversationTime = 10;
	
	#endregion
	
	#region Instance helper
	
	static GameManager instance = null;
	public static GameManager Instance { get { return instance; } }
	
	#endregion
	
	#region public variables
	
	public static ConversationData conversationData = null;
	
	public AudioClip music;
	
	public GameObject player = null;
	public float playerSpeed = 10;
	
	public KarmaSettings karmaSettings = new KarmaSettings();
	
	public Conversation conversationTemplate = null;
	public GameObject[] worldChunks = new GameObject[0];
	
	public float tick = 0;
	
	public float worldKarma = 0;
	
	#endregion
	
	#region protected variables
	
	protected Conversation currentConversation = null;
	protected List<GameObject> worldChunkInstances = new List<GameObject>();
	
	protected Animator playerAnimator = null;
	
	#endregion
	
	#region public methods
	
	public void StartConversation(NPCController _NPC)
	{
		if(currentConversation == null)
		{
			GameObject gobj = Instantiate(conversationTemplate.gameObject) as GameObject;
			currentConversation = gobj.GetComponent<Conversation>();
			currentConversation.karma = Random.Range(-50,50) + (int)worldKarma/5;
			currentConversation.NPC = _NPC;
		}
	}
	
	#endregion
	
	#region monobehaviour methods
	
	void Awake()
	{
		instance = this;
	}
	
	// Use this for initialization
	void Start () 
	{
		playerAnimator = player.GetComponentInChildren<Animator>();
		playerAnimator.SetBool("isWalking", true);
		
		AudioManager.PlayMusic(music);
	}
	
	// Update is called once per frame
	void Update () 
	{
		tick += Time.deltaTime;
		if(tick > walkingTime + conversationTime)
		{
			worldKarma += currentConversation.karma;
			Destroy(currentConversation.gameObject);
			currentConversation = null;
			
			playerAnimator.SetBool("isChatting", false);
			playerAnimator.SetBool("isWalking", true);
			
			tick = 0;
		}
		else if(currentConversation == null && tick > walkingTime)
		{
			StartConversation(null);
			
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
		
		karmaSettings.Update(worldKarma);
		
		HUD.Instance.desiredWorldKarma = worldKarma;
		HUD.Instance.currentConversation = currentConversation;
	}
	
	void OnGUI()
	{
		GUILayout.BeginVertical("box");
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Music:", GUILayout.Width(40));
		AudioManager.MusicVolume = GUILayout.HorizontalSlider(AudioManager.MusicVolume, 0, 1, GUILayout.Width(50));
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Fx:", GUILayout.Width(40));
		AudioManager.FxVolume = GUILayout.HorizontalSlider(AudioManager.FxVolume, 0, 1, GUILayout.Width(50));
		GUILayout.EndHorizontal();
		
		GUILayout.Label("World Karma: " + worldKarma);
		
		GUILayout.EndVertical();
	}
	
	#endregion	
}
