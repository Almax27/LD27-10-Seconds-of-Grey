using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	#region public variables
	
	public float speedBoost = 2;
	public float movementSpeed = 10;
	
	#endregion
	
	#region protected variables
	
	protected Animator animator = null;
	protected bool isWalking = true;
	
	#endregion
	
	#region public methods
	
	public void StartWalking()
	{
		isWalking = true;
		animator.SetBool("isChatting", false);
		animator.SetBool("isWalking", true);
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
		else
		{
			animator.SetBool("isWalking", true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		float boost = 1;
		if(Input.GetKey(KeyCode.Space))
			boost = speedBoost;
		
		animator.speed = boost;
		
		if(isWalking)
		{
			transform.Translate(0,0,movementSpeed*Time.deltaTime*boost);
		}
	}
	
	void OnTriggerEnter(Collider _other)
	{
		if(_other.tag == "NPC")
		{
			GameManager.Instance.StartConversation(null);
			isWalking = false;
			animator.SetBool("isChatting", true);
			animator.SetBool("isWalking", false);
		}
	}
	
	#endregion
}
