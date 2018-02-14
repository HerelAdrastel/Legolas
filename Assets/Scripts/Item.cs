using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Game {
	
	
	public float Velocity;

	private Rigidbody2D _rigidbody;

	// Use this for initialization
	public void Start () {
		_rigidbody = GetComponent<Rigidbody2D>();
		_rigidbody.velocity = Vector2.down * Velocity;
		
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public override void OnMenu(bool setToMenu) {
		
	}

	public override void OnPlay(bool setToPlay) {
		
	}

	public override void OnGameOver(bool setToGameOver) {
		Destroy(gameObject);
	}
	
	
}
