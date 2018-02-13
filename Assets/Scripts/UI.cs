﻿using System;
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

	private Text _scoreText;
	private Text _highscoreText;

	// Use this for initialization
	public override void Start () {
		base.Start();
		
		_highscore = PlayerPrefs.GetInt("highscore", 0);

		_canvasTransform = GetComponent<RectTransform>();

		_gamePlayed = PlayerPrefs.GetInt("gameplayed", 0);
		_menuInfosText = MenuInfosComponent.GetComponent<Text>();
		
		_scoreText = ScoreComponent.GetComponent<Text>();
		_highscoreText = HighscoreComponent.GetComponent<Text>();
	}

	
	// todo: ajouter conditions
	
	/**
	 * Shows Menu items and hides the rest
	 */
	public override void OnMenu() {
		UpdateMenuInfos();

		// Shows start menu
		foreach (GameObject item in MenuItems)
			item.SetActive(true);
		
		// Hides play menu
		PlayItem.SetActive(false);
		
		// Hides gameover menu
		foreach (GameObject item in GameOverItems)
			item.SetActive(false);
	}

	
	/**
	 * Hide Start Menu item and show play menu
	 */
	public override void OnPlay(bool setToPlay) {
		
		// Start Hide menu
		foreach (GameObject item in MenuItems)
			item.SetActive(false);
		
		// Shows play menu
		PlayItem.SetActive(true);
		
		Debug.Log(setToPlay);
		
		// Adds 1 to gamePlayed
		if (setToPlay) {
			Debug.Log(_gamePlayed);
			_gamePlayed++;
			Debug.Log(_gamePlayed);
			PlayerPrefs.SetInt("gameplayed", _gamePlayed);
			PlayerPrefs.Save();
		}
	}

	
	/**
	 * Update the Score Infos, Hide the play menu and shows the gameover menu
	 */
	public override void OnGameOver(bool setToGameOver) {
		UpdateScoreInfos();
		
		PlayItem.SetActive(false);
		
		foreach (GameObject item in GameOverItems)
			item.SetActive(true);

		if (setToGameOver) {
			StartCoroutine(SlideLeftIn(GameOverItems[0], 0.5f, 0));
			StartCoroutine(SlideLeftIn(GameOverItems[1], 0.5f, 0.25f));
			StartCoroutine(SlideLeftIn(GameOverItems[2], 0.5f, 0.5f));
			StartCoroutine(SlideLeftIn(GameOverItems[3], 0.5f, 0.75f));
			StartCoroutine(SlideLeftIn(GameOverItems[4], 0.5f, 1));

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

		if (Score > _highscore) {
			_highscore = Score;

			// Save highscore
			PlayerPrefs.SetInt("highscore", _highscore);
			PlayerPrefs.Save();
		}
		
		// Display score and highscore
		_scoreText.text = Score.ToString();
		_highscoreText.text = _highscore.ToString();
	}

	public IEnumerator SlideLeftIn(GameObject component, float duration, float startAfter) {

		RectTransform rect = component.GetComponent<RectTransform>();

		float offset = _canvasTransform.rect.width / 2 + component.GetComponent<RectTransform>().rect.width / 2;

		float from = component.transform.position.x + offset;
		
		//Vector3 position =  new Vector3(from, component.transform.position.y, component.transform.position.z);
		rect.anchoredPosition = new Vector2(offset, rect.anchoredPosition.y);
		//Debug.Log(Camera.main.WorldToScreenPoint(position));
		
		yield return new WaitForSeconds(startAfter);
		
		for (float i = 0; i < duration; i += Time.deltaTime) {
			float x = from - offset * i / duration;
			//component.transform.position = new Vector3(x, component.transform.position.y, component.transform.position.z);
			rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
			yield return null;
		}

	}
}
