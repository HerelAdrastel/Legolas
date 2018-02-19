using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GoogleMobileAds.Api;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main {
    
    
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
         * Canvas components;
         */
        private RectTransform _canvasTransform;

        
        /**
         * Menu components
         */
        public Text MenuInfosText;
        
        /**
         * The Gameover components
         */
        public Text ScoreText;
        public Text HighscoreText;
        public Text PointText;

        // Use this for initialization
        public override void Start() {
            base.Start();

            _highscore = PlayerPrefs.GetInt("highscore", 0);

            _canvasTransform = GetComponent<RectTransform>();

            _gamePlayed = PlayerPrefs.GetInt("gameplayed", 0);
            
            // Load ads
            Ad.GetInstance().Reward.OnAdRewarded += RewardLoaded;
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
            if (MenuItems.Length != 4)
                throw new Exception("MenuItems must contain 3 elements");

            StartCoroutine(SlideBottom(MenuItems[0], 0.75f, 0));
            StartCoroutine(Blink(MenuItems[1]));
            StartCoroutine(SlideTop(MenuItems[2], 0.75f, 0));
            StartCoroutine(SlideTop(MenuItems[3], 0.75f, 0));
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

            float duration = 2.5f;
            float durationPerItem = duration / GameOverItems.Length;
            float startAfter = 0;

            foreach (var item in GameOverItems) {
                StartCoroutine(SlideLeft(item, durationPerItem, startAfter));
                startAfter += durationPerItem / 2;
            }

            if (CanShowAd())
                Ad.GetInstance().ShowInterstitial();
        }
        
        /**
         * Restart scene
         */
        public void OnRetryClick() {
            ResetStatic();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /**
         * Switch shop
         */
        public void OnShop() {
            ResetStatic();
            SceneManager.LoadScene(1);
        }


        // todo ; ça ne marche pas, resoudre le bug !
        /**
         * Share score
         */
        public void OnShareApp() {

            string text =
                string.Format(
                    "Wow ! I made {0} points on WallBall ! " +
                    "https://play.google.com/store/apps/details?id=com.adrastel.legolas", 
                    Points);

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
            intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
            intent.Call<AndroidJavaObject>("setType", "text/plain");
            
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Share");
            currentActivity.Call("startActivity", chooser);
        }
        
        /**
         * Redirects to Play store
         */
        public void OnRateApp() {
            Application.OpenURL ("market://details?id=com.adrastel.legolas");
        }

        public void OnShowAd() {
            Ad.GetInstance().ShowReward();
        }
        
        
        public static bool CanShowAd() {
            return _gamePlayed % 5 == 0 && _gamePlayed != 0;
        }

        
        /**
         * Update Start menu infos
         */
        public void UpdateMenuInfos() {
            MenuInfosText.text = string.Format("HIGHSCORE: {0}\nGAMES PLAYED: {1}", _highscore, _gamePlayed);
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
            int storedPoints = AddPoints(Points);
            
            
            // Display score and highscore
            ScoreText.text = Score.ToString();
            HighscoreText.text = _highscore.ToString();

            StartCoroutine(IncreaseNumber(PointText, storedPoints - Points, storedPoints, 10, 1.5f));
        }
        
        private void RewardLoaded(object sender, Reward e) {

            int points = 20;
            
            int storedPoints = AddPoints(20);
            StartCoroutine(IncreaseNumber(PointText, storedPoints - points, storedPoints, 10, 1.5f));
        }

        public int AddPoints(int points) {
            int storedPoints = PlayerPrefs.GetInt("points", 0);
            storedPoints += points;
            PlayerPrefs.SetInt("points", storedPoints);
            PlayerPrefs.Save();

            return storedPoints;
        }

        // todo : bord arrondis
        
        /**
         * Slide right to left
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

            float offset = _canvasTransform.rect.height / 2;

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
        public static IEnumerator Blink(GameObject component) {
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
        public static IEnumerator IncreaseNumber(Text text, int from, int to, float speed, float startAfter) {
            text.text = from.ToString();

            yield return new WaitForSeconds(startAfter);


            while (from <= to) {
                text.text = from.ToString();
                from++;

                yield return new WaitForSeconds(1 / speed);
            }
        }
        
        
    }
}