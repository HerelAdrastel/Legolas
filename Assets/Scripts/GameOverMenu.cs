using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Game {

	public GameObject[] ToShow;

	// Use this for initialization
	public void Start () {
		
		foreach (GameObject item in ToShow) {
			item.SetActive(false);
		}
		
	}
	

	public override void onGameOver() {
		
		foreach (GameObject item in ToShow) {
			item.SetActive(true);
		}
	}
}
