using System;
using UnityEngine;
using Utils;

public class TimeManager : Singleton<TimeManager> {
    public float minForTimeEvent = 1;
    
    public delegate void TimeEvent();

    public event TimeEvent ClockTick;

    private DateTime lastTimeDetected = DateTime.Now;

    private float elapsedTime = 0;

    private void Update() {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 60 * minForTimeEvent) {
            elapsedTime = 0;
            if (ClockTick != null) {
                ClockTick();
            }
        }
    }
    
    //dato che è per mobile bisogna tenere traccia di quando il giocatore metterà in pausa l'app
    private void OnApplicationPause(bool pause) {
        if (pause) {
            lastTimeDetected = DateTime.Now;
        }
        else {
            elapsedTime += TimeUtils.DifferenceInSeconds(lastTimeDetected, DateTime.Now);
        }
    }
}
