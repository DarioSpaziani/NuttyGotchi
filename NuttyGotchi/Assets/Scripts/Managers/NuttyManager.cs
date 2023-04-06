using System;
using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Utils;
using Random = UnityEngine.Random;

namespace Managers {

	public class NuttyManager : Singleton<NuttyManager> {

		public GameObject gameOverPanel;
		
		public GameObject eatButton;
		public GameObject sleepButton;
		public GameObject playButton;
		public GameObject wakeUpButton;
		public GameObject sliderActions;

		public GameObject sleepPanel;

		public TextMesh calendar;

		public int elapsedSinceLastMeal;
		public int elapsedSinceLastSleep;
		public int elapsedSinceLastGame;

		public GameObject wall;
		[FormerlySerializedAs("material")] public Material wallMaterial;
		public Light directionalLight;

		public enum Status {
			Well,
			Hungry,
			Tired,
			Sleeping,
			Boring,
			Death
		}
		
		//delegate for the nutty interaction and all event link with that
		public delegate void NuttyInteraction();

		public event NuttyInteraction Feed;
		public event NuttyInteraction Sleep;
		public event NuttyInteraction Play;
		public event NuttyInteraction WakeUp;
		public event NuttyInteraction FirstStart;

		private NuttyStats stats;

		//it is a component built in framework of unity, work for the fade in and fade out of the panel
		private CanvasGroup gameOverCanvasGroup;

		private void Awake() {
			elapsedSinceLastMeal = 0;
			elapsedSinceLastSleep = 0;
			elapsedSinceLastGame = 0;
		}

		private void Start() {
			print("meal stats printed in Start : " + elapsedSinceLastMeal);
		}

		private void Update() {
			
		}

		//load the stats, if there are yet in memory or create new ones with the relative event
		public void Load() {
			if (SaveAndLoadManager.Instance.NeedFirstSave()) {
				stats = SaveAndLoadManager.Instance.FirstSave();
				if (FirstStart != null) {
					FirstStart();
				}
			}
			else {
				stats = SaveAndLoadManager.Instance.Load(); //check if is good, if isn't change name Load in SaveAndLoad
			}
		}
		
		private void RestartGame() {
			SaveAndLoadManager.Instance.RemoveSave();
			stats = SaveAndLoadManager.Instance.FirstSave();
			FirstStart?.Invoke();
		}
		
		//check the stats
		[Obsolete("Obsolete")]
		public Status CheckForStats() {
			DateTime now = DateTime.Now;

			DateTime birthday = TimeUtils.DateTimeFromULongInLocalTime(stats.birthDay);
			DateTime lastMeal = TimeUtils.DateTimeFromULongInLocalTime(stats.lastMeal);
			DateTime lastSleep = TimeUtils.DateTimeFromULongInLocalTime(stats.lastSleep);
			DateTime lastGame = TimeUtils.DateTimeFromULongInLocalTime(stats.lastPlay);
			
			//compare all stats saved with the actual time
			//for any interval of time get it,compare with the relative variables
			int days = TimeUtils.DifferenceInDays(birthday, now);
			calendar.text = days.ToString();
			
			elapsedSinceLastMeal = TimeUtils.DifferenceInMinutes(lastMeal, now);
			elapsedSinceLastSleep = TimeUtils.DifferenceInMinutes(lastSleep, now);
			elapsedSinceLastGame = TimeUtils.DifferenceInMinutes(lastGame, now);

			if (elapsedSinceLastSleep > Constants.MAX_TIME_FROM_LAST_SLEEP) {
				return elapsedSinceLastSleep > Constants.MAX_TIME_BEFORE_DIE ? Status.Death : Status.Tired;
			}
			
			if (elapsedSinceLastMeal > Constants.MAX_TIME_FROM_LAST_MEAL) {
				return elapsedSinceLastMeal > Constants.MAX_TIME_BEFORE_DIE ? Status.Death : Status.Hungry;
			}
			
			if (elapsedSinceLastGame > Constants.MAX_TIME_FROM_LAST_GAME) {
				return Status.Boring;
			}

			return Status.Well;
		}
		
		
		#region NuttyAction

		public void FeedNutty() {
			stats.lastMeal = TimeUtils.ULongFromDateTime(DateTime.Now);
			SaveAndLoadManager.Instance.Save(stats);

			Feed?.Invoke();
		}
		
		public void PlayNutty() {
			stats.lastPlay = TimeUtils.ULongFromDateTime(DateTime.Now);
			SaveAndLoadManager.Instance.Save(stats);

			StartCoroutine(WallMaterialRandom(2f));
			Play?.Invoke();
		}
		
		public void SleepNutty() {
			stats.lastSleep = TimeUtils.ULongFromDateTime(DateTime.Now);
			SaveAndLoadManager.Instance.Save(stats);

			eatButton.SetActive(false);
			playButton.SetActive(false);
			sleepButton.SetActive(false);
			

			wakeUpButton.SetActive(true);

			directionalLight.enabled = false;
			wall.SetActive(true);
			wallMaterial.color = Color.black;
			
			sliderActions.SetActive(false);
			
			StartCoroutine(WaitForSleep());
			
			Sleep?.Invoke();
		}
		
		public void WakeUpNutty() {

			eatButton.SetActive(true);
			playButton.SetActive(true);
			sleepButton.SetActive(true);
			
			wakeUpButton.SetActive(false);
			
			directionalLight.enabled = true;
			wall.SetActive(false);
			sleepPanel.SetActive(false);
			
			sliderActions.SetActive(true);

			WakeUp?.Invoke();
		}

		public void GameOver() {
			gameOverCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();

			StartCoroutine(GameOverPanelAnim(0.1f));
		}

		private IEnumerator GameOverPanelAnim(float step) {
			gameOverPanel.SetActive(true);

			while (gameOverCanvasGroup.alpha < 1) {
				yield return null;
				gameOverCanvasGroup.alpha += step;
			}

			yield return new WaitForSeconds(3);
			RestartGame();
			
			while (gameOverCanvasGroup.alpha > 0) {
				yield return null;
				gameOverCanvasGroup.alpha -= step;
			}
			
			gameOverPanel.SetActive(false);
		}
		
		private IEnumerator WaitForSleep() {
			yield return new WaitForSeconds(2f);
			sleepPanel.SetActive(true);
		}

		private IEnumerator WallMaterialRandom(float maxTime) {
			wallMaterial.color = Random.ColorHSV(0, 1, 1, 1, 0.5f, 1);
			wall.SetActive(true);

			float elapsedTime = 0;
			while (elapsedTime <= maxTime) {
				wallMaterial.color = Random.ColorHSV(0, 1, 1, 1, 0.5f, 1);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			
			wall.SetActive(false);
		}

		#endregion

	}
}