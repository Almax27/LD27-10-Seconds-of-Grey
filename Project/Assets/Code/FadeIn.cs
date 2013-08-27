using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class FadeIn : MonoBehaviour 
{	
	float t = 0;
	
	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
		Texture2D tex = new Texture2D(1,1);
		tex.SetPixel(0,0,Color.white);
		guiTexture.texture = tex;
		guiTexture.color = Color.black;
		Application.LoadLevel("Game");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Color c = guiTexture.color;
		
		t += Time.deltaTime;
		c.a = Easing.Ease(t, 1, 0, 2, Easing.Method.QuadInOut);
		
		guiTexture.color = c;
		
		if(t > 2)
			Destroy(gameObject);
	}
}
