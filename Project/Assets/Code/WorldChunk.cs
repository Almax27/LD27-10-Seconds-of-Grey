using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class WorldChunk : MonoBehaviour 
{
	#region static control
	const int bufferSize = 5;
	
	static List<WorldChunk> previous = new List<WorldChunk>();
	static WorldChunk current = null;
	static List<WorldChunk> next = new List<WorldChunk>();
	
	#endregion
	
	#region public variables
	
	public bool spawnNPCs = true;
	
	#endregion
	
	#region private methods
	
	#endregion
	
	#region monobehaviour methods
	
	// Use this for initialization
	void Awake () 
	{
		if(spawnNPCs)
		{
			Object[] npcs = Resources.LoadAll("NPCs");
			GameObject gobj = npcs[Random.Range(0,npcs.Length-1)] as GameObject;
			if(gobj != null)
			{
				gobj = Instantiate(gobj) as GameObject;
				gobj.transform.parent = transform;
				
				Vector3 p = Vector3.zero;
				p.x = GameManager.Instance.player.transform.position.x;
				p.z = Random.Range(collider.bounds.min.z,collider.bounds.max.z);
				gobj.transform.position = p;
			}
		}
	}
	
	void OnDestroy()
	{
		//clean make sure no references are left of us
		previous.Remove(this);
		next.Remove(this);
		if(current == this)
			current = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider _other)
	{
		if(_other.tag == "Player")
		{
			Object[] chunks = Resources.LoadAll("WorldChunks");
			
			//remove oldest and add current
			if(previous.Count != 0)
			{
				WorldChunk oldest = previous[previous.Count-1];
				previous.RemoveAt(previous.Count-1);
				Destroy(oldest.gameObject);
			}
			if(current != null)
			{
				previous.Insert(0, this);
			}
			
			//populate previous buffer until full
			while(previous.Count < bufferSize)
			{
				WorldChunk adjacent = this;
				if(previous.Count != 0)
					adjacent = previous[previous.Count-1];
				
				WorldChunk chunk = (Instantiate(chunks[Random.Range(0, chunks.Length)]) as GameObject).GetComponent<WorldChunk>();
				chunk.transform.position = adjacent.transform.position - new Vector3(0,0,(adjacent.collider.bounds.size.z + chunk.collider.bounds.size.z)/2f);
				previous.Add(chunk);
			}
			
			next.Remove(this);
			current = this;
			
			while(next.Count < bufferSize)
			{
				WorldChunk adjacent = this;
				if(next.Count != 0)
					adjacent = next[next.Count-1];
				
				WorldChunk chunk = (Instantiate(chunks[Random.Range(0, chunks.Length)]) as GameObject).GetComponent<WorldChunk>();
				chunk.transform.position = adjacent.transform.position + new Vector3(0,0,(adjacent.collider.bounds.size.z + chunk.collider.bounds.size.z)/2f);
				next.Add(chunk);
			}
		}
	}
	
	#endregion
}
