using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game : MonoBehaviour {

	private static bool _playing = false;

	protected static int Score;
	
	/**
	 * Called during the class creation
	 * The children classes must override the method and add base.Start at the END of the method
	 */
	public virtual void Start () {
		
	}
	
	/**
	 * Called every frames
	 * The children classes must override the method and add base.Start at the END of the method
	 */
	public virtual void Update () {
		if(_playing)
			onPlay();
		else
			onPause();
	}

	/**
	 * Called on each frame during the play time
	 */
	public abstract void onPlay();
	
	/**
	 * Called on each frame during the gameover time
	 */
	public abstract void onPause();

	public static void Play() {
		_playing = true;
	}

	public virtual void GameOver() {
		_playing = false;
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

	public static void PlayParticle(ParticleSystem prefab, Vector3 position) {
		ParticleSystem particleSystem = Instantiate(prefab, position, Quaternion.identity);
		
		Destroy(particleSystem.gameObject, particleSystem.main.startLifetime.constant);
	}

	
	/**
	 * Execute function after few seconds
	 *
	 * public void main() {
	 *	StartCoroutine(waitAndRun())
	 * }
	 *
	 * public IEnumerator waitAndRun() {
	 *
	 * 	yield WaitForSeconds(1);
	 *  blablabla
	 *
	 * }
	 */
	
}
