using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class BarStatus : MonoBehaviour {
	public NuttyManager nuttyManager;
	public Slider slider;

	private void Start() {
		slider.value = 0;
	}

	private void Food() {
		if (gameObject.CompareTag("Food")) {
			slider.value = nuttyManager.elapsedSinceLastMeal;
			slider.maxValue = Constants.MAX_TIME_FROM_LAST_MEAL;
		}
		
	}

	private void Play() {
		if (gameObject.CompareTag("Fun")) {
			slider.value = nuttyManager.elapsedSinceLastGame;
			slider.maxValue = Constants.MAX_TIME_FROM_LAST_GAME;
		}
	}

	private void Sleep() {
		if (gameObject.CompareTag("Sleep")) {
			slider.value = nuttyManager.elapsedSinceLastSleep;
			slider.maxValue = Constants.MAX_TIME_FROM_LAST_SLEEP;
		}
	}

		private void Death() {
			if (gameObject.CompareTag("Health")) {
				slider.value = nuttyManager.elapsedSinceLastMeal;
				slider.maxValue = Constants.MAX_TIME_BEFORE_DIE;
			}
		}

		private void Update() {
		Food();
		Play();
		Sleep();
		Death();
	}
}
