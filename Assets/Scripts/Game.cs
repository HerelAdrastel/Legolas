using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game : MonoBehaviour {

	protected static bool Gameover = false;
	
	// Use this for initialization
	public virtual void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(Gameover)
			onGameOver();
	}

	public abstract void onGameOver();

	public void GameOver()
	{
		//Debug.Log("Game Over :(");
		Gameover = true;
		//onGameOver();
	}
	
	
	public static float RandomFloat(float min, float max) {
		return Random.value * (max - min) + min;
	}
	
	public static float Left {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0)).x;
		}
	}

	public static float Right {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0)).x;
		}
	}

	public static float Top {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 0)).y;
		}
	}

	public static float Bottom {
		get {
			return Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0)).y;
		}
	}

}
