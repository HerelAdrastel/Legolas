using UnityEngine;
using UnityEngine.Serialization;

namespace Main {
	public class FollowPlayer : MonoBehaviour {

		public Transform Player;

		private void Update() {
			if(Player != null)
				transform.position = Player.position;
		}
	}
}
