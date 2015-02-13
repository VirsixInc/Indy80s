using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLBase : System.Object {
	
	public static string path = "Database.xml";
	
	public XMLBase() {
		
	}
	
	protected virtual void Init() {
		
	}
	
	public void Save() {
		var serializer = new XmlSerializer(this.GetType());
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
	
	public static T Load<T>(ref bool loaded) where T : XMLBase, new() {
		path = typeof(T).ToString() + ".xml";
		
		var serializer = new XmlSerializer(typeof(T));
		if(!File.Exists(path)) {
			loaded = false;
			Debug.Log("\"" + path + "\" not found. Creating \"" + path + "\"");
			
			object parsedValue = default(T);
			try
			{
				parsedValue = Convert.ChangeType(new T(), typeof(T));
			}
			catch (InvalidCastException)
			{
				parsedValue = null;
				Debug.Log("Can't cast");
			}
			catch (ArgumentException)
			{
				parsedValue = null;
			}
			
			((T)parsedValue).Init();
			
			return (T)parsedValue;
			
		} else {
			loaded = true;
			using(var stream = new FileStream(path, FileMode.Open))
			{
				object parsedValue = default(T);
				try
				{
					parsedValue = Convert.ChangeType(serializer.Deserialize(stream), typeof(T));
				}
				catch (InvalidCastException)
				{
					parsedValue = null;
				}
				catch (ArgumentException)
				{
					parsedValue = null;
				}
				return (T)parsedValue;
			}
		}
	}

	public static T Load<T>() where T : XMLBase, new() {
		path = typeof(T).ToString() + ".xml";
		
		var serializer = new XmlSerializer(typeof(T));
		if(!File.Exists(path)) {
			Debug.Log("\"" + path + "\" not found. Creating \"" + path + "\"");
			
			object parsedValue = default(T);
			try
			{
				parsedValue = Convert.ChangeType(new T(), typeof(T));
			}
			catch (InvalidCastException)
			{
				parsedValue = null;
				Debug.Log("Can't cast");
			}
			catch (ArgumentException)
			{
				parsedValue = null;
			}
			
			((T)parsedValue).Init();
			
			return (T)parsedValue;
			
		} else {
			using(var stream = new FileStream(path, FileMode.Open))
			{
				object parsedValue = default(T);
				try
				{
					parsedValue = Convert.ChangeType(serializer.Deserialize(stream), typeof(T));
				}
				catch (InvalidCastException)
				{
					parsedValue = null;
				}
				catch (ArgumentException)
				{
					parsedValue = null;
				}
				return (T)parsedValue;
			}
		}
	}
}