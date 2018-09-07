using System;
using GoogleMobileAds.Api;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Main {
	
	public class Player : Game {

		public float Velocity;
		public float Force;
		public int PointMultiplier;
		public Text ScoreText;
	
		public ParticleSystem PlayerDeath;
		
		public AudioClip JumpSound;
		public AudioClip PointSound;
		public AudioClip GameOverSound;


		private Rigidbody2D _rigidbody;
		private CircleCollider2D _collider;
	
		private enum Direction{Left = -1, Right = 1}

		private Direction _previousDirection;
		private Direction _direction;

		private float _radius;

		// Use this for initialization
		public override void Start () {
			_rigidbody = GetComponent<Rigidbody2D>();
			_collider = GetComponent<CircleCollider2D>();

			_direction = Random.value < 0.5 ? Direction.Left : Direction.Right;
			_previousDirection = _direction;

			_radius = _collider.bounds.size.x / 2;

			// Set player appearance
			int skin = PlayerPrefs.GetInt("selected", 0);
			string path = string.Format("Player/{0}", skin);
			Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
			GetComponent<SpriteRenderer>().sprite = sprite;
	

			
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
		public override void OnMenu(bool setToMenu) {
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		}

		public override void OnPlay(bool setToPlay) {
			_rigidbody.constraints = RigidbodyConstraints2D.None;
		}

		public override void OnGameOver(bool setToGameOver) {
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

			if (setToGameOver) {
				PlayParticle(PlayerDeath, transform.position);
				AudioSource.PlayClipAtPoint(GameOverSound, transform.position);
			}

			Destroy(gameObject);
		}

		public void Jump() {
			// Resets the verical velocity
			_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, 0);
		
			// Jump
			_rigidbody.AddForce(Vector2.up * Force, ForceMode2D.Impulse);
			
			// Jump noise
			AudioSource.PlayClipAtPoint(JumpSound, transform.position);
		}

		public void IncreaseScore() {
			Score++;
			ScoreText.text = Score.ToString();
		}

		public void OnTriggerEnter2D(Collider2D other) {
			if(other.CompareTag("Enemy"))
				GameOver();
		
			else if (other.CompareTag("Item")) {
				Points += PointMultiplier;
				AudioSource.PlayClipAtPoint(PointSound, transform.position);
				Destroy(other.gameObject);
			}

			else
				throw new Exception("Unknown tag name");
		}
	}
}
