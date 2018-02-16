using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public abstract class Game : MonoBehaviour {

	protected static int Score;
	protected static int Points;


	public static string Name {
		get { return SceneManager.GetActiveScene().name; }
	}
	
	/**
	 * 0: Menu
	 * 1: Play
	 * 2: GameOver
	 * 
	 * We use a global state to be sure that in all the game child, the setToPlay and setToGameover are executed
	 */
	private static int _globalState;
	private int _state = -1;
	
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

		switch (_globalState) {
		
			// Menu
			case 0:

				if (_state == -1) {
					OnMenu(true);
					_state = _globalState;
				}
				
				else
					OnMenu(false);
				
				if (IsTouched())
					_globalState = 1;
				break;
				
			// Playing
			case 1:
				
				// If set to play
				if (_state == 0) {
					OnPlay(true);
					_state = _globalState;
				}
				
				else
					OnPlay(false);
				break;
			
			// Game Over
			case 2:
				
				// If set to gameover
				if (_state == 1) {
					OnGameOver(true);
					_state = _globalState;
				}
				
				else
					OnGameOver(false);
				break;
			
			default:
				throw new Exception("The _globalState variable must be between 0 and 2");
				
		}
	}


	public abstract void OnMenu(bool setToMenu);
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
		_globalState = 2;
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
		_globalState = 0;
		Score = 0;
		Points = 0;
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
