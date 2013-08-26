using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	#region static variables
	
	static AudioManager instance = null;
	static AudioManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
			}
			return instance;
		}
	}
	
	public static float FxVolume
	{
		set { Instance.fxVolume = value; Instance.oneShotSource.volume = Instance.fxVolume; }
		get { return Instance.fxVolume; }
	}
	
	public static float MusicVolume
	{
		set { Instance.musicSource.volume = value; }
		get { return Instance.musicSource.volume; }
	}
	
	#endregion
	
	#region private variables
	
	float fxVolume = 1;
	
	Transform pool = null;
	AudioSource musicSource = null;
	AudioSource oneShotSource = null;
	
	#endregion
	
	#region public static methods
	
	public static void PlaySoundFx(AudioClip _clip, float _volume, Vector3 _soundPos)
	{
		Vector3 camPos = Camera.mainCamera.transform.position;
		Vector3 distance = _soundPos - camPos;
		float maxDistance = 75;
		
		float volume = _volume * Mathf.Clamp01(1 - (distance.magnitude/maxDistance));
		
		if(_clip != null)
			Instance.oneShotSource.PlayOneShot(_clip, volume*Instance.fxVolume);
	}
	
	public static void PlaySoundFx2D(AudioClip _clip, float _volume)
	{	
		if(_clip != null)
			Instance.oneShotSource.PlayOneShot(_clip, _volume*Instance.fxVolume);
	}
	
	public static void PlayMusic(AudioClip clip)
	{
		PlayMusic(clip, false);
	}
	public static void PlayMusic(AudioClip clip, bool forceRestart)
	{
		if(Instance.musicSource.clip != clip || forceRestart)
		{
			Instance.musicSource.clip = clip;
			Instance.musicSource.Play();
		}
	}
	
	public static void StopMusic()
	{
		Instance.musicSource.Stop();
	}
	
	#endregion
	
	#region monobehaviour methods
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		pool = new GameObject("Pool").transform;
		musicSource = new GameObject("Music", typeof(AudioSource)).audio;
		oneShotSource = new GameObject("OneShot", typeof(AudioSource)).audio;
		
		pool.parent = musicSource.transform.parent = oneShotSource.transform.parent = transform;
	}
	
	#endregion
	
}
