using UnityEngine;

namespace Main {
	
	public class Item : Game {
	
	
		public float Velocity;

		private Rigidbody2D _rigidbody;

		// Use this for initialization
		public override void Start () {
			_rigidbody = GetComponent<Rigidbody2D>();
			_rigidbody.velocity = Vector2.down * Velocity;
		
			base.Start();
		}

		public override void OnMenu(bool setToMenu) {
		
		}

		public override void OnPlay(bool setToPlay) {
		
		}

		public override void OnGameOver(bool setToGameOver) {
			Destroy(gameObject);
		}
	
	
	}
}
