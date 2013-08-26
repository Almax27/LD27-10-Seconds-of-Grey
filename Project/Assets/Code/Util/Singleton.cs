using UnityEngine;
using System.Collections.Generic;

public class Singleton : MonoBehaviour 
{
	static List<string> instanced = new List<string>();
	
	// Use this for initialization
	void Awake () 
	{
		if(!instanced.Contains(name))
		{
			instanced.Add(name);	
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
