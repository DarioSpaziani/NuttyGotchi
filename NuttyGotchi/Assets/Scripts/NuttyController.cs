using System.Collections;
using Managers;
using UnityEngine;

public class NuttyController : MonoBehaviour 
{
	public GameObject welcomePanel;

	private NuttyManager.Status status = NuttyManager.Status.Well;

	private CanvasGroup welcomeCanvasGroup;

	private void NiceToMeetYou() {
		welcomeCanvasGroup = welcomePanel.GetComponent<CanvasGroup>();
		StartCoroutine(FadeInOutPanel(0.01f));

		CheckForEvents();
	}
	
	private IEnumerator FadeInOutPanel(float step) {
		welcomePanel.SetActive(true);

		while (welcomeCanvasGroup.alpha < 1) {
			yield return null;
			welcomeCanvasGroup.alpha += step;
		}

		yield return new WaitForSeconds(3f);

		if (welcomeCanvasGroup.alpha > 0) {
			yield return null;
			welcomeCanvasGroup.alpha -= step;
		}

		welcomePanel.SetActive(false);
	}
	
	// ReSharper disable Unity.PerformanceAnalysis
	private void CheckForEvents() {
		if (status != NuttyManager.Status.Sleeping) {
			status = NuttyManager.Instance.CheckForStats();

			switch (status) {
				case NuttyManager.Status.Well:
					WellNutty();
					break;
				case NuttyManager.Status.Death:
					DeadNutty();
					break;
				default:
					print("sad nutty!");
					break;
			}
		}
	}
	
	private static void WellNutty() {
		print("Well!");
	}
	
	private static void DeadNutty() {
		NuttyManager.Instance.GameOver(); 
	}
	
	//4 placeholder
	private void FeedAction() {
		print("Feed");
	}
	
	private void PlayAction() {
		print("Play");
	}
	
	private void SleepAction() {
		print("Sleep");
	}
	
	private void WakeUpAction() {
		print("WakeUp");
	}
	
	private void Start() {
		NuttyManager.Instance.FirstStart += NiceToMeetYou;
		
		NuttyManager.Instance.Load();
		CheckForEvents();
		TimeManager.Instance.ClockTick += CheckForEvents;

		NuttyManager.Instance.Feed += FeedAction;
		NuttyManager.Instance.Sleep += SleepAction;
		NuttyManager.Instance.Play += PlayAction;
		NuttyManager.Instance.WakeUp += WakeUpAction;
	}
	
	private void OnDestroy() {
		if (TimeManager.Instance != null) {
			TimeManager.Instance.ClockTick -= CheckForEvents;
		}
		
		if (NuttyManager.Instance != null) {
			NuttyManager.Instance.Feed -= FeedAction;
			NuttyManager.Instance.Sleep -= SleepAction;
			NuttyManager.Instance.Play -= PlayAction;
			NuttyManager.Instance.WakeUp -= WakeUpAction;
		}
	}
}