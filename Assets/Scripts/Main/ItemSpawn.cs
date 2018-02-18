using Tools;
using UnityEngine;

namespace Main {
	
	public class ItemSpawn : Game {

		public Transform Item;
	
		public float Interval;

		private Interval _interval;

		// Use this for initialization
		public override void Start () {
			_interval = new Interval(SpawnEnemy, Interval);
		
			base.Start();
		}
	
		// Update is called once per frame
		public override void Update () {
			base.Update();
			_interval.Update(Time.deltaTime);
		}

		public override void OnMenu(bool setToMenu) {
			_interval.Pause();
		}

		public override void OnPlay(bool setToPlay) {
			_interval.Play();
		}

		public override void OnGameOver(bool setToGameOver) {
			_interval.Pause();
		}

		public void SpawnEnemy(int iterations) {
		
			Transform item = Instantiate(Item);

			PolygonCollider2D collider = item.GetComponent<PolygonCollider2D>();

			float halfSize = collider.bounds.size.x / 2;
		
			float x = RandomFloat(Left + halfSize, Right - halfSize);

			item.position = new Vector3(x, transform.position.y, transform.position.z);
		}
	}
}
