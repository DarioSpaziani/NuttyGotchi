using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Managers {

	public class NuttyManager : Singleton<NuttyManager> {

		public GameObject gameOverPanel;
		
		public GameObject eatButton;
		public GameObject sleepButton;
		public GameObject playButton;
		public GameObject wakeUpButton;

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

		//load the stats, if there are yet in memory or create new ones with the relative event
		public void Load() {
			if (SaveAndLoadManager.Instance.needFirstSave()) {
				stats = SaveAndLoadManager.Instance.FirstSave();
				if (FirstStart != null) {
					FirstStart();
				}
				else {
					stats = SaveAndLoadManager.Instance.Load(); //check if is good, if isn't change name Load in SaveAndLoad
				}
			}
			
		}

		//check the stats
		public Status CheckForStats() {
			DateTime now = DateTime.Now;

			DateTime lastMeal = TimeUtils.dateTimeFromULongInLocalTime(stats.lastMeal);
			DateTime lastSleep = TimeUtils.dateTimeFromULongInLocalTime(stats.lastSleep);
			DateTime lastGame = TimeUtils.dateTimeFromULongInLocalTime(stats.lastPlay);
			
			//compare all stats saved with the actual time
			//for any interval of time get it,compare with the relative variables
			int elapsedSinceLastMeal = TimeUtils.differenceInMinutes(lastMeal, now);
			int elapsedSinceLastSleep = TimeUtils.differenceInMinutes(lastSleep, now);
			int elapsedSinceLastGame = TimeUtils.differenceInMinutes(lastGame, now);

			if (elapsedSinceLastSleep > Constants.MAX_TIME_FROM_LAST_SLEEP) {
				return elapsedSinceLastSleep > Constants.MAX_TIME_BEFORE_DIE ? Status.Death : Status.Tired;
			}
			
			if (elapsedSinceLastMeal > Constants.MAX_TIME_FROM_LAST_MEAL) {
				print("ciao");
				return elapsedSinceLastMeal > Constants.MAX_TIME_BEFORE_DIE ? Status.Death : Status.Hungry;
				
			}
			
			if (elapsedSinceLastGame > Constants.MAX_TIME_FROM_LAST_GAME) {
				return Status.Boring;
			}

			return Status.Well;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		private void RestartGame() {
			SaveAndLoadManager.Instance.RemoveSave();
			stats = SaveAndLoadManager.Instance.FirstSave();
			FirstStart?.Invoke();
		}

		public void FeedNutty() {
			stats.lastMeal = TimeUtils.uLongFromDateTime(DateTime.Now);
			SaveAndLoadManager.Instance.Save(stats);

			Feed?.Invoke();
		}
		
		public void PlayNutty() {
			stats.lastPlay = TimeUtils.uLongFromDateTime(DateTime.Now);
			SaveAndLoadManager.Instance.Save(stats);

			Play?.Invoke();
		}
		
		public void SleepNutty() {
			stats.lastSleep = TimeUtils.uLongFromDateTime(DateTime.Now);
			SaveAndLoadManager.Instance.Save(stats);

			eatButton.SetActive(false);
			playButton.SetActive(false);
			sleepButton.SetActive(false);
			
			wakeUpButton.SetActive(true);
			
			if (Sleep != null) {
				Sleep();
			}
		}
		
		public void WakeUpNutty() {

			eatButton.SetActive(true);
			playButton.SetActive(true);
			sleepButton.SetActive(true);
			
			wakeUpButton.SetActive(false);
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
	}

}