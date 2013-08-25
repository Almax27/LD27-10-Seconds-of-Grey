using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLUtil {

	
	#region serialization methods
	
	public static void SaveResource(string fileName, object obj)
	{
		if(obj == null)
		{
			Debug.LogWarning("Invaild XML Resource: " + fileName);
			return;
		}
		
#if UNITY_EDITOR
		string path = Path.Combine(Application.dataPath, "Resources/"+fileName+".xml");
		
		XmlSerializer serializer = new XmlSerializer(obj.GetType());
		using(FileStream stream = new FileStream(path, FileMode.Create)) //overrite if exists
		{
			Debug.Log(string.Format("Saving: {0}", path));
			serializer.Serialize(stream, obj);
		}
#else
		Debug.LogWarning("Cannot save data outside the editor!");
#endif
	}
	
	public static T LoadResource<T>(string fileName)
	{		
		T obj = default(T);
		
#if UNITY_EDITOR
		string path = Path.Combine(Application.dataPath, "Resources/"+fileName+".xml");
		
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		try //catch when doesn't exist
		{
			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				Debug.Log(string.Format("Loading: {0}", path));
				obj = (T) serializer.Deserialize(stream);
			}
		} 
		catch(IOException e)
		{
			Debug.Log(e);
		}
#else
		UnityEngine.Object textAsset = Resources.Load(fileName);
		if(textAsset != null)
		{
			TextAsset xmlText = textAsset as TextAsset;
			
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(xmlText.text);
			
			Debug.Log("Loading: " + fileName);
			obj = (T) serializer.Deserialize(reader);
		}			
#endif
		if(obj == null)
			Debug.Log("Failed to load: " + fileName);
		
		return obj;
	}
	
	public static void SaveLocalData(string fileName, object obj)
	{
		if(obj == null)
		{
			Debug.LogWarning("Invaild XML Resource: " + fileName);
			return;
		}
		
		string path = Path.Combine(Application.persistentDataPath, fileName);
		
		XmlSerializer serializer = new XmlSerializer(obj.GetType());
		using(FileStream stream = new FileStream(path, FileMode.Create)) //overrite if exists
		{
			Debug.Log(string.Format("Saving GameState to: {0}", path));
			serializer.Serialize(stream, obj);
		}	
	}
	
	public static T LoadLocalData<T>(string fileName)
	{
		T obj = default(T);
		
		string path = Path.Combine(Application.persistentDataPath, fileName);
		
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		try //catch when doesn't exist
		{
			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				Debug.Log(string.Format("Loading: {0}", path));
				obj = (T) serializer.Deserialize(stream);
			}
		} 
		catch(IOException e)
		{
			Debug.Log(e.Message);
			//Debug.Log("No file found... making a new one");
			//obj = new T();
			//SaveLocalData(fileName, obj);
		}
		catch(System.IO.IsolatedStorage.IsolatedStorageException e)
		{
			Debug.Log(e.Message);
			//Debug.Log("IsolatedStorage exception: No file found... making a new one");
			//obj = new T();
			//SaveLocalData(fileName, obj);
		}
		catch(System.Xml.XmlException e)
		{
			Debug.Log(e.Message);
			//Debug.Log("XmlException: Failed to read file... deleting and remaking");
			//File.Delete(path);
			//obj = new T();
			//SaveLocalData(fileName, obj);
		}
		
		return obj;
	}
	
	public static bool ResourceExists(string fileName, bool inResources)
	{
		if(inResources)
			return Resources.Load(fileName) != null;
		else
			return File.Exists(Path.Combine(Application.dataPath, fileName));
	}
	
	#endregion
}
