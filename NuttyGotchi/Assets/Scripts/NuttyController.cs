using System.Collections;
using Managers;
using UnityEngine;

public class NuttyController : MonoBehaviour 
{
	public GameObject welcomePanel;

	public Baloon baloon;

	public Animator animator;

	private NuttyManager.Status status = NuttyManager.Status.Well;

	private CanvasGroup welcomeCanvasGroup;

	private void NiceToMeetYou() {
		animator.SetBool("isDead", false);
		welcomeCanvasGroup = welcomePanel.GetComponent<CanvasGroup>();
		StartCoroutine(FadeInOutPanel(0.01f));

		CheckForEvent();
	}
	
	private IEnumerator FadeInOutPanel(float step) {
		welcomePanel.SetActive(true);

		while (welcomeCanvasGroup.alpha < 1) {
			yield return null;
			welcomeCanvasGroup.alpha += step;
		}

		yield return new WaitForSeconds(3f);

		while (welcomeCanvasGroup.alpha > 0) {
			yield return null;
			welcomeCanvasGroup.alpha -= step;
		}

		welcomePanel.SetActive(false);
	}

	private IEnumerator LateCheckForEvent() {
		yield return new WaitForSeconds(5f);
		CheckForEvent();
	}
	
	// ReSharper disable Unity.PerformanceAnalysis
	private void CheckForEvent() {
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
					animator.SetBool("isSad", true);
					baloon.Show(status);
					break;
			}
		}
	}
	
	private void WellNutty() {
		animator.SetBool("isSad", false);
	}
	
	private void DeadNutty() {
		animator.SetBool("isDead", true);
		NuttyManager.Instance.GameOver(); 
		
	}
	
	//4 placeholder
	private void FeedAction() {
		animator.SetBool("isSad", false);
		animator.SetTrigger("Eat");
		baloon.Hide();
		StartCoroutine(LateCheckForEvent());
	}
	
	private void PlayAction() {
		animator.SetBool("isSad", false);
		animator.SetTrigger("Play");
		baloon.Hide();
		StartCoroutine(LateCheckForEvent());
	}
	
	private void SleepAction() {
		animator.SetBool("isSad", false);
		status = NuttyManager.Status.Sleeping;
		animator.SetBool("isSleeping", true);
		baloon.Hide();
	}
	
	private void WakeUpAction() {
		status = NuttyManager.Status.Well;
		animator.SetBool("isSleeping", false);
		StartCoroutine(LateCheckForEvent()); 
	}
	
	private void Start() {
		NuttyManager.Instance.FirstStart += NiceToMeetYou;
		
		NuttyManager.Instance.Load();
		CheckForEvent();
		TimeManager.Instance.ClockTick += CheckForEvent;

		NuttyManager.Instance.Feed += FeedAction;
		NuttyManager.Instance.Sleep += SleepAction;
		NuttyManager.Instance.Play += PlayAction;
		NuttyManager.Instance.WakeUp += WakeUpAction;
	}
	
	private void OnDestroy() {
		if (TimeManager.Instance != null) {
			TimeManager.Instance.ClockTick -= CheckForEvent;
		}
		
		if (NuttyManager.Instance != null) {
			NuttyManager.Instance.Feed -= FeedAction;
			NuttyManager.Instance.Sleep -= SleepAction;
			NuttyManager.Instance.Play -= PlayAction;
			NuttyManager.Instance.WakeUp -= WakeUpAction;
		}
	}
}