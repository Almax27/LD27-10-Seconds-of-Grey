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
	
	public float goodDirLightIntensity = 0.5f;
	public float badDirLightIntensity = 0.3f;
	
	public float goodSpotLightIntensity = 0.5f;
	public float badSpotLightIntensity = 0.3f;
	
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
			l.intensity = Mathf.Lerp(badDirLightIntensity, goodDirLightIntensity, t);
			l.shadowStrength = Mathf.Lerp(badShadowStrength, goodShadowStrength, t);
		}
		
		foreach(Light l in Light.GetLights(LightType.Spot,0))
		{
			l.enabled = false;
			l.color = spotLightColor;
			l.intensity = Mathf.Lerp(badSpotLightIntensity, goodSpotLightIntensity, t);
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
	
	public PlayerController player = null;
	
	public KarmaSettings karmaSettings = new KarmaSettings();
	
	public Conversation conversationTemplate = null;
	public GameObject[] worldChunks = new GameObject[0];
	
	public float worldKarma = 0;
	
	#endregion
	
	#region protected variables
	
	bool isIntro = true;
	
	protected float tick = 0;
	
	protected Conversation currentConversation = null;
	protected List<GameObject> worldChunkInstances = new List<GameObject>();
	
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
			
			_NPC.SetState(NPCController.NPCState.CHAT);
			
			tick = 0;
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
		AudioManager.PlayMusic(music);
		Conversation.GetData();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(currentConversation != null)
		{
			tick += Time.deltaTime;
			if(tick > conversationTime)
			{				
				worldKarma += currentConversation.karma;
				
				//resolve animations
				currentConversation.NPC.SetState(NPCController.NPCState.SIDESTEP);
				
				Destroy(currentConversation.gameObject);
				currentConversation = null;
				
				player.StartWalking();
			}
		}
		
		karmaSettings.Update(worldKarma);
		
		if(worldKarma >= 100 || worldKarma <= -100)
		{
			//game over
			Instantiate(Resources.Load("GameOver"));
			this.enabled = false;
		}
		
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
