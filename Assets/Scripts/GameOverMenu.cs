using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : Game {

	/**
	 * All the gameover items which are hidden and shown
	 */
	public GameObject[] ToShow;
	public GameObject ToHide;
	
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

	public override void onPlay() {
		
		foreach (GameObject item in ToShow) {
			item.SetActive(false);
		}
		ToHide.SetActive(true);
	}

	public override void onPause() {

		UpdateScoreInfos();
		
		foreach (GameObject item in ToShow) {
			item.SetActive(true);
		}
		ToHide.SetActive(false);
	}
	
	// ReSharper disable once UnusedMember.Global
	public void OnRetryClick() {
		Play();
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
