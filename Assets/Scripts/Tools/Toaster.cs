using UnityEngine;

namespace Tools {
	
	public class Toaster {


		private static Toaster _instance;
		private string _toastString;
		private string _input;
		private AndroidJavaObject _currentActivity;
		private AndroidJavaObject _context;

		public static Toaster GetInstance() {
			return _instance ?? (_instance = new Toaster());
		}
	
		public Toaster() {
			if (Application.platform != RuntimePlatform.Android) return;
		
			AndroidJavaObject unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			_currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			_context = _currentActivity.Call<AndroidJavaObject>("getApplicationContext");
		}


		public void ShowToastOnUiThread(string toastString) {
			_toastString = toastString;
			_currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(ShowToast));
		}

		private void ShowToast() {
			Debug.Log(this + ": Running on UI thread");

			AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
			AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", _toastString);
			AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", _context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
			toast.Call("show");
		}
	}
}