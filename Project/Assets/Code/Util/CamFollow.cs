using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CamFollow : MonoBehaviour {
	
	#region public variables
	
	public Camera target = null;
	public float maxDistance = 1;
	public float deadDistance = 0.1f;
	public float smoothTime = 0.5f;
	
	#endregion
		
	#region protected variables
	
	protected Vector3 vel = Vector3.zero;
	
	#endregion
	
	#region monobehaviour methods
	
	// Use this for initialization
	void Start () 
	{
		if(target != null)
		{
			transform.position = target.transform.position;
			transform.rotation = target.transform.rotation;
			camera.fieldOfView = target.fieldOfView;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target != null)
		{
			Vector3 dv = target.transform.position - transform.position;
			float d = dv.magnitude;
			transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref vel, smoothTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, 360f*smoothTime*Time.deltaTime);
			
			camera.fieldOfView = target.fieldOfView;
		}
	}
	
	#endregion
}
