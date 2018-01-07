using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceDisplay : MonoBehaviour {

	private LineRenderer m_Line;
	private Player m_Player;

	public int m_Size = 60;
	Vector3[] m_Positions;

	private int m_FullSize;

	private Vector3 m_InitialPosition;
	private Vector3 m_FinalPosition;
	private Vector3 m_CenterPosition;
	private float m_InitialAngle;
	private float m_Range;

	void Awake () {
		m_Player = GameObject.FindObjectOfType<Player>();
		m_Line = GetComponent<LineRenderer>();
		m_Positions = new Vector3[m_Size];
		m_FullSize = Mathf.FloorToInt(m_Size * 2.5f);
	}
	
	void Update () {
		m_InitialPosition = m_Player.transform.position;
		m_FinalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		m_InitialPosition.z = 0f;
		m_FinalPosition.z = 0f;

		Vector3 m_DiffPos = (m_FinalPosition - m_InitialPosition);
		m_CenterPosition = (m_FinalPosition - m_InitialPosition) * 0.5f;
		m_Range = m_CenterPosition.magnitude;

		m_InitialAngle = Mathf.Atan2(m_DiffPos.normalized.y, m_DiffPos.normalized.x) * Mathf.Rad2Deg;
		m_InitialAngle += 90;

		for (int i = 0; i < m_Size; i++) {
			float time = (i * 1.0f) / m_FullSize;
			float angle = Mathf.Deg2Rad * (Mathf.Lerp(0f, 360f, time) - m_InitialAngle);

			Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

			if(m_Player.easeBoomerangCurve) {
				float dTime = time * 2;
				float relTime = time > .5 ? 2 - dTime : dTime;
				float fTime = relTime > .5 ? relTime : 1 - relTime;
				dir *= fTime;
			}
			
			Vector3 newPosition = m_CenterPosition + m_InitialPosition + ( dir * m_Range );
			m_Positions[i] = newPosition;
		}

		m_Line.positionCount = m_Size;
		m_Line.SetPositions(m_Positions);
	}

}
