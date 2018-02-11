using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Game
{

	public float Velocity; 
	public float AngularVelocity;


	private Rigidbody2D _rigidbody;
	private BoxCollider2D _collider;
	
	// Use this for initialization
	public void Start ()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	public override void Update () {
		
		base.Update();
		
		if (transform.position.y + _collider.bounds.size.y < Bottom)
			Destroy (gameObject);
		
		_rigidbody.velocity = Vector2.down * Velocity;
		_rigidbody.angularVelocity = AngularVelocity;
	}

	public override void onGameOver() {
		Destroy(gameObject);
	}
}
