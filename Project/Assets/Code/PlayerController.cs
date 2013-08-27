using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	#region public variables
	
	public float speedBoost = 2;
	public float movementSpeed = 10;
	
	public CamFollow mainCamera = null;
	public Camera walkingCamera = null;
	public Camera chattingCamera = null;
	
	public Collider playButton = null;
	public Collider creditsButton = null;
	
	#endregion
	
	#region protected variables
	
	protected Animator animator = null;
	protected bool isWalking = true;
	
	protected GameObject credits = null;
	
	#endregion
	
	#region public methods
	
	public void StartWalking()
	{
		mainCamera.target = walkingCamera;
		isWalking = true;
		animator.SetBool("isChatting", false);
		animator.SetBool("isIdle", false);
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
		if(credits != null)
			return;
		
		float boost = 1;
		
		if(isWalking)
		{
			if(Input.anyKey)
				boost = speedBoost;
			
			transform.Translate(0,0,movementSpeed*Time.deltaTime*boost);
		}
		else
		{
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			playButton.renderer.material.color = Color.white;
			if(playButton.Raycast(r, out hit, 100))
			{
				if(Input.GetMouseButtonUp(0))
				{
					StartWalking();
				}
				else
					playButton.renderer.material.color = Color.cyan;
			}
			creditsButton.renderer.material.color = Color.white;
			if(creditsButton.Raycast(r, out hit, 100))
			{
				if(Input.GetMouseButtonUp(0))
				{
					credits = Instantiate(Resources.Load("Credits")) as GameObject;
				}
				else
					creditsButton.renderer.material.color = Color.cyan;
			}
		}
		
		animator.speed = boost;
	}
	
	void OnTriggerEnter(Collider _other)
	{
		if(_other.tag == "NPC")
		{
			mainCamera.target = chattingCamera;
			GameManager.Instance.StartConversation(null);
			isWalking = false;
			animator.SetBool("isChatting", true);
			animator.SetBool("isWalking", false);
			animator.SetBool("isIdle", false);
		}
		if(_other.tag == "Title")
		{
			isWalking = false;
			animator.SetBool("isChatting", false);
			animator.SetBool("isWalking", false);
			animator.SetBool("isIdle", true);
		}
	}
	
	#endregion
}
