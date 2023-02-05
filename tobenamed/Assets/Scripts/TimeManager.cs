using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public EventHandler<TimeUpdateEventArgs> TimeUpdateEventHandler;
    public bool isPaused;
    private void Update() {
        if (!isPaused) {
            TimeUpdateEventHandler?.Invoke(this, new TimeUpdateEventArgs(Time.deltaTime));
        }
    }
}

public class TimeUpdateEventArgs : EventArgs {
    public TimeUpdateEventArgs(float deltaTime) {
        this.deltaTime = deltaTime;
    }
    public float deltaTime;
}
