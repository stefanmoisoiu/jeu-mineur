using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedHatch : Hatch
{
    [Header("Hatch Properties")]
    [SerializeField] private bool offsetTime = false;
    
    [SerializeField] private float openedTime = 1f;
    [SerializeField] private float closedTime = 1f;

    private void Update()
    {
        // if openedTime = 1 & closedTime = 1 -> |0s|---closed---|1s|---opened---|2s|
        float totalTime = openedTime + closedTime;
        float hatchTime = (Time.time + (offsetTime ? totalTime / 2 : 0)) % totalTime;
        bool isOpened = hatchTime > closedTime;
        SetState(isOpened);
    }


}
