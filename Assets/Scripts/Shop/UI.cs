using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shop {
	
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public class UI : MonoBehaviour {

		public Text PointText;
		public Transform Button;
        
		private static int _points;
		
		public static int[] Prices = {
			0, 25, 25, 50, 50, 100
		};
	
		// Use this for initialization
		public void Start () {
		
		
			_points = PlayerPrefs.GetInt("points", 0);
		
			UpdatePoints();
			LoadButtons();
		}

	
		/**
	 * Load all buttons with properties
	 */
		public void LoadButtons() {
			for (int i = 0; i < Prices.Length; i++) {

				// Unlock by default the first item
				int unlockByDefault = i == 0 ? 1 : 0;
				
				bool isUnlocked = PlayerPrefs.GetInt(i.ToString(), unlockByDefault) == 1;
				bool isSelected = PlayerPrefs.GetInt("selected", 0) == i;
			
				Transform button = Instantiate(Button);
			
			
				button.SetParent(transform, false);
				button.GetComponent<ShopButton>().Setup(i, Prices[i], isUnlocked, isSelected);
				button.name = i.ToString();

			}
		}

		/**
	 * Update the GUI text
	 */
		public void UpdatePoints() {
			PointText.text = _points.ToString();
		}

		/**
	 * Check if can buy
	 * Return true if possible
	 */
		public bool Buy(int id) {

			// Stops if can't buy
			if (_points < Prices[id]) return false;
		
			// Update points
			_points -= Prices[id];
			UpdatePoints();

			
			// Save new points and the unlocked item
			PlayerPrefs.SetInt("points", _points);
			PlayerPrefs.SetInt(id.ToString(), 1);
			PlayerPrefs.SetInt("selected", id);
			PlayerPrefs.Save();
		
			UnselectAll();
			return true;
		}

		/**
	 * Backgroundly select the player. The GUI update is done in the ShopButton.cs
	 */
		public void Select(int id) {
		
			UnselectAll();

			PlayerPrefs.SetInt("selected", id);
			PlayerPrefs.Save();
		}

	
		/**
	 * Update graphically all the buttons to unselect them all
	 */
		public void UnselectAll() {
			foreach(Transform child in transform)
				child.GetComponent<ShopButton>().Unselect();
		}


		/**
	 * On back click
	 */
		public void OnBack() {
			SceneManager.LoadScene(0);
		}
	}
}
