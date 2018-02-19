using System.Diagnostics.CodeAnalysis;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Shop {
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public class ShopButton : MonoBehaviour {

		public GameObject Locked;
		public GameObject Unlocked;
		public Text PriceText;
		public Image Icon;

		// The background color when selected or not
		public Image Background;
		public Color StandartColor;
		public Color SelectedColor;
	
		private int _id;
		private int _price;
		private UI _ui;
	
	
	
		// Use this for initialization
		public void Start () {
			_ui = GetComponentInParent<UI>();
		}
	
		// Update is called once per frame
		public void Update () {
			
		}

		/**
	 * The constructor of the button
	 */
		public void Setup(int id, int price, bool isUnlocked, bool isSelected) {
			_id = id;
			_price = price;

			PriceText.text = price.ToString();
		
			if(isUnlocked)
				Unlock();
		
			else
				Lock();

			Color backgroundColor = isSelected ? SelectedColor : StandartColor;
			Background.color = backgroundColor;

			// todo voir si load marche en load<T>
			string path = string.Format("Player/{0}", _id);
			Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
			Icon.sprite = sprite;
		}

		/**
	 * Set the button locked graphically
	 */
		public void Lock() {
			Locked.SetActive(true);
			Unlocked.SetActive(false);
		}

		/**
	 * Set the button unlocked graphically
	 */
		public void Unlock() {
			Locked.SetActive(false);
			Unlocked.SetActive(true);
		}

		/**
	 * Set the button selected graphically
	 */
		public void Select() {
			Background.color = SelectedColor;
		}

	
		/**
	 * Set the button unselected graphically
	 */
		public void Unselect() {
			Background.color = StandartColor;
		}
	
	
		/**
	 * Callback used in Unity Editor of a locked button
	 */
		public void OnUnlock() {
			if (_ui.Buy(_id)) {
				Unlock();
				Select();
			}
		}

		/**
	 * Callback used in Unity Editor of a unlocked button
	 */
		public void OnSelect() {
			_ui.Select(_id);
			Select();
		}
	}
}
