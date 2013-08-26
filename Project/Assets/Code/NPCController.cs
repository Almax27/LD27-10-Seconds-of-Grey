using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {
	
	#region public variables
	
	
	
	#endregion
	
	#region protected variables
	
	protected Animator animator = null;
	
	#endregion
	
	#region monobehaviour methods
	
	// Use this for initialization
	void Start ()
	{
		animator = GetComponentInChildren<Animator>();
		if(animator == null)
		{
			Debug.LogError("No Animator Found");
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider _other)
	{
		if(_other.tag == "Player")
		{
			GameManager.Instance.StartConversation(this);
		}
	}
	
	#endregion
}
