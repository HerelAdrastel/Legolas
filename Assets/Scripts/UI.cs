using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class UI : Game {

	/**
	 * All the gameover items which are hidden and shown
	 */
	public GameObject[] MenuItems;
	public GameObject PlayItem;
	public GameObject[] GameOverItems;
	
	private static int _highscore;
	private static int _gamePlayed;
	
	
	/**
	 * Canvas components
	 */
	private RectTransform _canvasTransform;	
	
	/**
	 * The Menu components
	 */
	public Transform MenuInfosComponent;

	private Text _menuInfosText;
	
	/**
	 * The Gameover components
	 */
	public Transform ScoreComponent;
	public Transform HighscoreComponent;
	public Transform PointComponent;

	private Text _scoreText;
	private Text _highscoreText;
	private Text _pointText;

	// Use this for initialization
	public override void Start () {
		base.Start();
		
		_highscore = PlayerPrefs.GetInt("highscore", 0);

		_canvasTransform = GetComponent<RectTransform>();

		_gamePlayed = PlayerPrefs.GetInt("gameplayed", 0);
		_menuInfosText = MenuInfosComponent.GetComponent<Text>();
		
		_scoreText = ScoreComponent.GetComponent<Text>();
		_highscoreText = HighscoreComponent.GetComponent<Text>();
		_pointText = PointComponent.GetComponent<Text>();
	}

	
	// todo: ajouter conditions
	
	/**
	 * Shows Menu items and hides the rest
	 */
	public override void OnMenu(bool setToMenu) {
		
		// Execute the next lines only once
		if (!setToMenu) return;
		
		UpdateMenuInfos();

		// Shows start menu
		foreach (GameObject item in MenuItems)
			item.SetActive(true);

		// Hides play menu
		PlayItem.SetActive(false);

		// Hides gameover menu
		foreach (GameObject item in GameOverItems)
			item.SetActive(false);
		
		// Check the length before animation
		if(MenuItems.Length != 3)
			throw new Exception("MenuItems must contain 3 elements");

		StartCoroutine(SlideBottom(MenuItems[0], 0.75f, 0));
		StartCoroutine(Blink(MenuItems[1]));
		StartCoroutine(SlideTop(MenuItems[2], 0.75f, 0));

	}

	
	/**
	 * Hide Start Menu item and show play menu
	 */
	public override void OnPlay(bool setToPlay) {
		
		// Execute the next lines only once
		if (!setToPlay) return;
		
		// Start Hide menu
		foreach (GameObject item in MenuItems)
			item.SetActive(false);
		
		// Shows play menu
		PlayItem.SetActive(true);
			
			
		// Adds 1 to gamePlayed
		_gamePlayed++;
		PlayerPrefs.SetInt("gameplayed", _gamePlayed);
		PlayerPrefs.Save();
	}

	
	/**
	 * Update the Score Infos, Hide the play menu and shows the gameover menu
	 */
	public override void OnGameOver(bool setToGameOver) {
		
		// Execute the next lines only onces
		if (!setToGameOver) return;
		
		UpdateScoreInfos();
		
		PlayItem.SetActive(false);
		
		foreach (GameObject item in GameOverItems)
			item.SetActive(true);

		float duration = 2.5f / GameOverItems.Length;
		float startAfter = 0;

		for (int i = 0; i < GameOverItems.Length; i++) {

			StartCoroutine(SlideLeft(GameOverItems[i], duration, startAfter));
			startAfter += duration / 2;
		}
	}
	
	
	/**
	 * Restars scene
	 * 
	 */
	public void OnRetryClick() {
		ResetStatic();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	
	/**
	 * Share score
	 */
	
	// todo ; ça ne marche pas, resoudre le bug !
	public void OnShareClick() {

		#if UNITY_ANDROID
		string text = String.Format(
			"Wow! I got {0} points on {1}, can you beat me ?\nDownload on http://bit.ly/RexalAndroid", 
			Score, Name
		);

		
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call ("startActivity", intentObject);
		#endif
	}

	/**
	 * Updates Start menu infos
	 */
	public void UpdateMenuInfos() {
		_menuInfosText.text = string.Format("HIGHSCORE: {0}\nGAMES PLAYED: {1}", _highscore, _gamePlayed);
	}

	
	/**
	 * Updates Gameover score infos
	 */
	public void UpdateScoreInfos() {

		// Save highscore
		if (Score > _highscore) {
			_highscore = Score;

			// Save highscore
			PlayerPrefs.SetInt("highscore", _highscore);
			PlayerPrefs.Save();
		}
		
		// Save points
		int storedPoints = PlayerPrefs.GetInt("points", 0);
		storedPoints += Points;
		PlayerPrefs.SetInt("points", storedPoints);
		PlayerPrefs.Save();
		
		// Display score and highscore
		_scoreText.text = Score.ToString();
		_highscoreText.text = _highscore.ToString();
		//_pointText.text = storedPoints.ToString();

		StartCoroutine(IncreaseNumber(_pointText, storedPoints - Points, storedPoints, 10, 1.5f));

	}

	// todo : bord arrondis
	/**
	 * Slides from right to left
	 *
	 * The X anchor must be set to center
	 */
	public IEnumerator SlideLeft(GameObject component, float duration, float startAfter) {

		RectTransform rect = component.GetComponent<RectTransform>();

		float offset = _canvasTransform.rect.width / 2 + rect.rect.width / 2;

		float from = rect.anchoredPosition.x + offset;
		float to = rect.anchoredPosition.x;
		
		rect.anchoredPosition = new Vector2(from, rect.anchoredPosition.y);
		
		yield return new WaitForSeconds(startAfter);
		
		for (float i = 0; i < duration; i += Time.deltaTime) {
			float x = from - offset * i / duration;
			rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
			yield return null;
		}
		
		rect.anchoredPosition = new Vector2(to, rect.anchoredPosition.y);
	}

	/**
	 * Slide in from bottom to top
	 *
	 * Must be on the middle screen or below
	 */
	public IEnumerator SlideTop(GameObject component, float duration, float startAfter) {
		RectTransform rect = component.GetComponent<RectTransform>();

		float offset = -_canvasTransform.rect.height / 2;

		float from = rect.anchoredPosition.y + offset;
		float to = rect.anchoredPosition.y;
		
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, from);

		yield return new WaitForSeconds(startAfter);
		
		for (float i = 0; i < duration; i += Time.deltaTime) {
			float y = from - offset * i / duration;
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
			yield return null;
		}
		
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, to);
	}
	
	/**
	 * Slide in from top to bottom
	 *
	 * Must be on the middle screen or above
	 */
	public IEnumerator SlideBottom(GameObject component, float duration, float startAfter) {
		RectTransform rect = component.GetComponent<RectTransform>();

		float offset = _canvasTransform.rect.height /2;

		float from = rect.anchoredPosition.y + offset;
		float to = rect.anchoredPosition.y;
		
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, from);

		yield return new WaitForSeconds(startAfter);
		
		for (float i = 0; i < duration; i += Time.deltaTime) {
			float y = from - offset * i / duration;
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
			yield return null;
		}
		
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, to);
	}

	/**
	 * Blink with fade until component is disabled
	 */
	public IEnumerator Blink(GameObject component) {
		CanvasGroup cg = component.GetComponent<CanvasGroup>();

		float alpha = 1;
		const float speed = 0.9f;
		int direction = -1;

		while (component.activeSelf) {
			alpha += speed * Time.deltaTime * direction;

			if (alpha <= 0) {
				alpha = 0;
				direction = 1;
			}
			
			else if (alpha >= 1) {
				alpha = 1;
				direction = -1;
			}

			cg.alpha = alpha;

			yield return null;
		} 
	}
	
	/**
	 * Increase number animation
	 */
	public IEnumerator IncreaseNumber(Text text, int from, int to, float speed, float startAfter) {
		
		text.text = from.ToString();
		
		yield return new WaitForSeconds(startAfter);

		
		while (from <= to) {

			text.text = from.ToString();
			from++;

			yield return new WaitForSeconds(1 / speed);
		}
	} 
}
