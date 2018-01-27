using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Timer : MonoBehaviour {
	public Text timer;
	public int CountDownMs;

	private int timeRemainingMs;

	// Use this for initialization
	void Start () {
		timer.text = (CountDownMs / 1000).ToString("0.00");
		timeRemainingMs = CountDownMs;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeRemainingMs > 0) {
			timeRemainingMs = timeRemainingMs - (int)(Time.deltaTime * 1000);
			timer.text = (timeRemainingMs/ 1000).ToString("0.00");
		} else {
			timeRemainingMs = 0;
			timer.text = "0.00";
		}
	}

	bool TimeUp() {
		return timeRemainingMs == 0;
	}
}