using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider))]
public class NPCController : MonoBehaviour {
	
	public enum NPCState
	{
		IDLE,
		CHAT,
		SIDESTEP
	}
	#region public variables
	
	
	
	#endregion
	
	#region protected variables
	
	protected Animator animator = null;
	
	#endregion
	
	#region public methods
	
	public void SetState(NPCState state)
	{
		animator.SetBool("chat", false);
		animator.SetBool("sidestep", false);
		switch(state)
		{
		case NPCState.IDLE:
		{
			
		}break;
		case NPCState.CHAT:
		{
			animator.SetBool("chat", true);
		}break;
		case NPCState.SIDESTEP:
		{
			animator.SetBool("sidestep", true);
		}break;
		}
	}
	
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
			animator.SetBool("isChatting", true);
		}
	}
	
	#endregion
}
