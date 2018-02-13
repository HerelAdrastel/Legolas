using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Game
{

	public float Velocity;
	public float Force;
	
	// The ScoreComponent shown during playing
	public Transform ScoreComponent;
	
	public ParticleSystem PlayerDeath;


	private Rigidbody2D _rigidbody;
	private CircleCollider2D _collider;
	private Text _scoreText;
	
	private enum Direction{Left = -1, Right = 1}

	private Direction _previousDirection;
	private Direction _direction;

	private float _radius;


	// Use this for initialization
	public override void Start () {
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_scoreText = ScoreComponent.GetComponent<Text>();

		_direction = Random.value < 0.5 ? Direction.Left : Direction.Right;
		_previousDirection = _direction;

		_radius = _collider.bounds.size.x / 2;
	
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
		// Horizontal calculations
		// If the player touches the left wall
		if (transform.position.x < Left + _radius) {
			_direction = Direction.Right;
			
			if(_previousDirection != _direction)
				IncreaseScore();
			
		} 
		
		// If the player touches the right wall
		else if (transform.position.x > Right - _radius) {
			_direction = Direction.Left;
			if(_previousDirection != _direction) 
				IncreaseScore();
		}

		_previousDirection = _direction;
		
		
		// Vertical calculations
		// Blocks on the top
		if (transform.position.y > Top - _radius) {
			_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, 0);

			float y = Mathf.Min (transform.position.y, Top - _radius);
			transform.position = new Vector3 (transform.position.x, y, transform.position.z);
		}
		
		// Dies on bot
		else if (transform.position.y < Bottom - _radius) {
			GameOver();
		}


		_rigidbody.velocity = new Vector2 ((float) _direction * Velocity, _rigidbody.velocity.y);
		
		// Jump
		if (IsTouched())
			Jump();

	}

	// todo: ajouter conditions setToPlay
	public override void OnMenu() {
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	public override void OnPlay(bool setToPlay) {
		_rigidbody.constraints = RigidbodyConstraints2D.None;
	}

	public override void OnGameOver(bool setToGameOver) {
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		
		if(setToGameOver)
			PlayParticle(PlayerDeath, transform.position);
		
		Destroy(gameObject);
	}

	public void Jump()
	{
		// Resets the verical velocity
		_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, 0);
		
		// Jump
		_rigidbody.AddForce(Vector2.up * Force, ForceMode2D.Impulse);
	}

	public void IncreaseScore()
	{
		Score++;
		_scoreText.text = Score.ToString();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		GameOver();
	}
}
