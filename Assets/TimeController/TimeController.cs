using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

	private float m_Scale;

	private float m_Duration;
	private float m_StartTime;
	
	void Awake () {
		m_Scale = 1f;
	}
	
	void Update () {
		float time = (Time.time - m_StartTime) / m_Duration;
		if(time < 1) {
			Time.timeScale = Mathf.Lerp(m_Scale, 1f, time);
		} else {
			Time.timeScale = 1f;
		}
	}

	public void SlowTime(float slowBy, float slowTime) {
		m_Scale = slowBy;
		m_Duration = slowTime;
		m_StartTime = Time.time;
	}
}
