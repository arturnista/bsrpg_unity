using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	private Camera m_Camera;
	private Player m_Player;

	private Vector3 m_Focus;
	private float m_Size;

	private float m_OriginalSize;
	private Vector3 m_OriginalPosition;

	private float m_DurationHold;
	private float m_StartTimeHold;

	private float m_Duration;
	private float m_StartTime;

	private bool m_IsMoving;

	void Awake () {
		m_Camera = GetComponent<Camera>();

		m_Player = GameObject.FindObjectOfType<Player>();
		
		m_OriginalSize = m_Camera.orthographicSize;

		m_IsMoving = false;
	}
	
	void Update () {
		m_OriginalPosition = m_Player.transform.position;
		m_OriginalPosition.z = -10f;

		if(!m_IsMoving) {
			transform.position = m_OriginalPosition;
			m_Camera.orthographicSize = m_OriginalSize;
			return;
		}

		float timeHold = (Time.time - m_StartTimeHold) / m_DurationHold;
		if(timeHold > 1) {

			float time = (Time.time - m_StartTime) / m_Duration;
			if(time > 1) {
				transform.position = Vector3.Lerp(m_Focus, m_OriginalPosition, time);
				m_Camera.orthographicSize = Mathf.Lerp(m_Size, m_OriginalSize, time);
			} else {
				transform.position = m_OriginalPosition;
				m_Camera.orthographicSize = m_OriginalSize;

				m_IsMoving = false;
			}

		}
	}

	public void Focus(Vector3 posFocus, float focusTime) {
		m_Focus = posFocus;
		m_Focus.z = -10f;
		transform.position = m_Focus;

		m_Size = m_OriginalSize - 2f;
		m_Camera.orthographicSize = m_Size;

		m_DurationHold = focusTime;
		m_StartTimeHold = Time.time;

		m_Duration = 1f;
		m_StartTime = m_StartTimeHold + m_DurationHold;

		m_IsMoving = true;
	}
}
