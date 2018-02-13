using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public abstract class Game : MonoBehaviour {

	protected static int Score;


	public static string Name {
		get { return SceneManager.GetActiveScene().name; }
	}
	
	/**
	 * 0: Menu
	 * 1: SetToPlay
	 * 2: Play
	 * 3: SetToGameOver
	 * 4: GameOver
	 */
	private static int _state;
	
	

	
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
		/*if(_playing)
			OnPlay();
		else
			OnGameOver();*/

		switch (_state) {
			
			// Menu
			case 0:
				OnMenu();
				if (IsTouched())
					_state = 1;
				break;
			
			// Set To Play
			case 1:
				OnPlay(true);
				_state = 2;
				break;
			
			// Play
			case 2:
				OnPlay(false);
				break;
			
			// Set To Gameover
			case 3:
				OnGameOver(true);
				_state = 4;
				break;
			
			// Gameover
			case 4:
				OnGameOver(false);
				break;
			
			// Throw exception otherwise
			default:
				throw new Exception("The _state variable must be between 0 and 4");
				
		}
	}


	public abstract void OnMenu();
	/**
	 * Called on each frame during the play time
	 */
	public abstract void OnPlay(bool setToPlay);
	
	/**
	 * Called on each frame during the gameover time
	 */
	public abstract void OnGameOver(bool setToGameOver);

	public static void Play() {
		//_playing = true;
	}

	public virtual void GameOver() {
		//_playing = false;
		_state = 3;
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
	 * Return true if the space bar is pressed or the screen is touched
	 */
	public static bool IsTouched()
	{
		return Input.GetKeyDown(KeyCode.Space) || Input.touches.Any(touch => touch.phase == TouchPhase.Began);
	}
	
	
	/**
	 * Resets the static variables to not been kept during reseting Scene
	 */
	public static void ResetStatic() {
		_state = 0;
		Score = 0;
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
