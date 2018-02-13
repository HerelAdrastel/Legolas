using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : Game {

	/**
	 * All the gameover items which are hidden and shown
	 */
	public GameObject[] MenuItems;
	public GameObject PlayItem;
	public GameObject[] GameOverItems;
	
	private static int _highscore;

	
	/**
	 * The ScoreComponent shown during GameOver
	 */
	public Transform ScoreComponent;
	public Transform HighscoreComponent;

	private Text _scoreText;
	private Text _highscoreText;

	// Use this for initialization
	public override void Start () {
		base.Start();
		
		_highscore = PlayerPrefs.GetInt("highscore", 0);

		_scoreText = ScoreComponent.GetComponent<Text>();
		_highscoreText = HighscoreComponent.GetComponent<Text>();
	}

	
	// todo: ajouter conditions
	
	/**
	 * Shows Menu items and hides the rest
	 */
	public override void OnMenu() {

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
	}

	
	/**
	 * Update the Score Infos, Hide the play menu and shows the gameover menu
	 */
	public override void OnGameOver(bool setToGameOver) {
		UpdateScoreInfos();
		
		PlayItem.SetActive(false);
		
		foreach (GameObject item in GameOverItems)
			item.SetActive(true);
		
	}
	
	
	/**
	 * Restars scene
	 * 
	 */
	// ReSharper disable once UnusedMember.Global
	public void OnRetryClick() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

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
}
