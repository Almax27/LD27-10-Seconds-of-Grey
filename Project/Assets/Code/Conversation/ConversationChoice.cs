using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConversationChoice
{
	#region public variables
	
	public string text = "";
	public List<ConversationChoice> responces = new List<ConversationChoice>();
	public float minKarma = -100;
	public float maxKarma = 100;
	public float karmaReward = 0;
	
	#endregion
	
	#region protected variables
	
	#endregion
	
	#region public methods
	
	#endregion
}
