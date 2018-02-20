using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Singleton : MonoBehaviour {
	private static Singleton _instance = null;
	
	public static Singleton Instance {
		get {
			if (_instance == null)
				_instance = (Singleton)FindObjectOfType(typeof(Singleton));
			
			return _instance;
		}
	}
     
	public void Awake () {
		if (Instance != this)
			Destroy(gameObject);
		
		else
			DontDestroyOnLoad(gameObject);
	}
}
