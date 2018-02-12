using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Game
{

	public float Velocity;
	public float Force;
	public Transform ScoreComponent;
	

	private Rigidbody2D _rigidbody;
	private CircleCollider2D _collider;
	private Text _scoreText;
	
	private enum Direction{Left = -1, Right = 1}

	private Direction _previousDirection;
	private Direction _direction;

	private float _radius;

	private int _score;

	// Use this for initialization
	public void Start () {

		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_scoreText = ScoreComponent.GetComponent<Text>();

		_direction = Random.value < 0.5 ? Direction.Left : Direction.Right;
		_previousDirection = _direction;

		_radius = _collider.bounds.size.x / 2;
	

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

	public override void onGameOver() {
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	/**
	 * Return true if the space bar is pressed or the screen is touched
	 */
	public static bool IsTouched()
	{
		return Input.GetKeyDown(KeyCode.Space) || Input.touches.Any(touch => touch.phase == TouchPhase.Began);
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
		_score++;
		_scoreText.text = _score.ToString();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		GameOver();
	}
}
