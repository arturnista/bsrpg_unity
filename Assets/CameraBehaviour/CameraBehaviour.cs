using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	private Camera m_Camera;
	private Vector3 m_Focus;
	private float m_Size;

	private float m_OriginalSize;
	private Vector3 m_OriginalPosition;

	private float m_Duration;
	private float m_StartTime;

	void Awake () {
		m_Camera = GetComponent<Camera>();
		m_OriginalSize = m_Camera.orthographicSize;
		m_OriginalPosition = Vector3.zero;
		m_OriginalPosition.z = -10f;
	}
	
	void Update () {
		float time = (Time.time - m_StartTime) / m_Duration;
		if(time < 1) {
			transform.position = Vector3.Lerp(m_Focus, m_OriginalPosition, time);
			m_Camera.orthographicSize = Mathf.Lerp(m_Size, m_OriginalSize, time);
		} else {
			transform.position = m_OriginalPosition;
			m_Camera.orthographicSize = m_OriginalSize;
		}
	}

	public void Focus(Vector3 posFocus, float focusTime) {
		m_Focus = posFocus;
		m_Focus.z = -10f;
		transform.position = m_Focus;

		m_Size = m_OriginalSize - 1f;
		m_Camera.orthographicSize = m_Size;

		m_Duration = focusTime;
		m_StartTime = Time.time;
	}
}
