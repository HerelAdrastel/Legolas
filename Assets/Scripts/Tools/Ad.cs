using GoogleMobileAds.Api;
using UnityEngine;

namespace Tools {
	
	/**
	 * Singleton instance
	 */
	public class Ad {

		private static Ad _instance;

		public RewardBasedVideoAd Reward {
			get { return _reward; }
		}

		private RewardBasedVideoAd _reward;

		private InterstitialAd _interstitial;

		public static Ad GetInstance() {
			_instance = _instance ?? new Ad();
			return _instance;
		}

		public Ad() {

			// Load AdMob
			MobileAds.Initialize("ca-app-pub-2468409005145405~4748383171");

			
			// Load rewarded video
			_reward = RewardBasedVideoAd.Instance;
			_interstitial = new InterstitialAd("ca-app-pub-2468409005145405/4697250925");
			
			LoadReward();
			LoadInterstitial();
			
		}

		/**
		 * Load Rewarded video
		 */
		public void LoadReward() {
			_reward.LoadAd(GetRequest(), "ca-app-pub-2468409005145405/7701849572");
		}

		/**
		 * Show the rewarded video and loads an other
		 */
		public void ShowReward() {
			_reward.Show();
			LoadReward();
		}
		
		/**
		 * Load interstitial video
		 */
		public void LoadInterstitial() {
			_interstitial.LoadAd(GetRequest());
		}

		/**
		 * Show interstitial video and loads an other
		 */
		public void ShowInterstitial() {
			_interstitial.Show();
			LoadInterstitial();
		}

		public static AdRequest GetRequest() {
			return new AdRequest.Builder().AddTestDevice("99C3C959EC6AB2DA9AE7DD926A6FBFAE").Build();
		}

	}
}
