using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {
	
	#region public types
	
	public enum State
	{
		IN,
		IDLE,
		OUT
	}
	
	public enum ExitAction
	{
		RELOAD,
		DESTROY
	}
	
	#endregion
	
	#region public variables
	
	public GUITexture topTexture = null;
	public GUITexture bottomTexture = null;
	
	public Vector2 translationAxis = Vector2.one;
	public Easing.Method easingMethodIn = Easing.Method.Linear;
	public Easing.Method easingMethodOut = Easing.Method.Linear;
	public float easingDuration = 1;
	
	public ExitAction exitAction = ExitAction.DESTROY;
	
	#endregion
	
	#region portected variables
	
	protected State state = State.IN;
	protected Vector2 dir = Vector2.one;
	protected float tick = 0;
		
	#endregion
	
	#region monobehaviour methods
	
	// Use this for initialization
	void Start () 
	{
		translationAxis.Normalize();
		dir = new Vector2(translationAxis.x, Mathf.Abs(translationAxis.y));
		
		DontDestroyOnLoad(gameObject);
		
		topTexture.transform.localPosition = new Vector3(dir.x, dir.y, transform.position.z);
		bottomTexture.transform.localPosition = new Vector3(-dir.x, -dir.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Easing.Method easingMethod = Easing.Method.Linear;
		
		switch(state)
		{
		case State.IN:
			easingMethod = easingMethodIn;
			tick += Time.deltaTime;
			if(tick > easingDuration)
			{
				tick = easingDuration;
				state = State.IDLE;
			}
			break;
		case State.OUT:
			easingMethod = easingMethodOut;
			tick -= Time.deltaTime;
			if(tick < 0)
			{
				tick = 0;
				Destroy(gameObject);
			}
			break;
		case State.IDLE:
			if(Input.anyKeyDown)
			{
				state = State.OUT;
				if(exitAction == ExitAction.RELOAD)
					Application.LoadLevel(Application.loadedLevel);
			}
			break;
		}
		
		float ease = Easing.Ease(tick, 1, 0, easingDuration, easingMethod);
		
		Vector2 pos = dir * ease;
		topTexture.transform.localPosition = new Vector3(pos.x, pos.y, transform.position.z);
		bottomTexture.transform.localPosition = new Vector3(-pos.x, -pos.y, transform.position.z);
	}
	
	#endregion
}
