using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConversationData 
{
	#region public consts
	
	public const string FileName = "ConversationData";
	
	#endregion
	
	#region public variables
	
	public List<ConversationChoice> choices = new List<ConversationChoice>();
	
	#endregion

}
