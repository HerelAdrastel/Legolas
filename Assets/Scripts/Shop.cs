using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public Transform ButtonLocked;
	public Transform ButtonUnlocked;
	public Transform Grid;
	public GameObject PointComponent;

	private Text _pointText;

	private static int _points;
	
	public static int[] Prices = {
		1, 100
	};
	
	// Use this for initialization
	public void Start () {
		_points = PlayerPrefs.GetInt("points", 0);

		_pointText = PointComponent.GetComponent<Text>();

		UpdatePointText();
		Refresh();
		

	}

	public void Refresh() {

		foreach (Transform child in Grid) {
			Destroy(child.gameObject);
		}
		
		for (int i = 0; i < Prices.Length; i++) {

			if(IsLocked(i))
				DisplayLocked(i);
			
			else
				DisplayUnlocked(i);
		}
	}

	public void DisplayLocked(int index) {
		Transform button = Instantiate(ButtonLocked);
		button.SetParent(Grid, false);
		button.name = index.ToString();
		button.GetChild(0).GetComponent<Text>().text = Prices[index].ToString();

		// Add listener
		button.GetComponent<Button>().onClick.AddListener(OnBuy);
	}

	public Transform DisplayUnlocked(int index) {
		
		string path = string.Format("Player/{0}", index);
		Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
		Transform button = Instantiate(ButtonUnlocked);

		button.SetParent(Grid, false);

		button.GetChild(0).GetComponent<Image>().sprite = sprite;

		return button;
	}

	public bool IsLocked(int index) {
		return PlayerPrefs.GetInt(index.ToString(), 0) == 0;
	}

	public void Unlock(string index) {
		PlayerPrefs.SetInt(index, 1);
		PlayerPrefs.Save();
	}

	public void OnBuy() {
		/*int index = Convert.ToInt32(button.name);

		_points = 1000;
		if (_points >= Prices[index]) {
			_points -= Prices[index];
			UpdatePointText();
			Unlock(button.gameObject.name);

			DisplayUnlocked(index);
			Destroy(button.gameObject);

			Refresh();
		}*/
	}

	public void UpdatePointText() {
		_pointText.text = _points.ToString();
	}
}
