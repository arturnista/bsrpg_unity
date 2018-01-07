using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceDisplay : MonoBehaviour {

	private LineRenderer m_Line;
	private Player m_Player;

	private int m_Size;
	Vector3[] m_Positions;

	private Vector3 m_InitialPoint;
	private Vector3 m_FinalPoint;
	private Vector3 m_FarPoint;

	void Awake () {
		m_Player = GameObject.FindObjectOfType<Player>();
		m_Line = GetComponent<LineRenderer>();
		m_Size = 60;
		m_Positions = new Vector3[m_Size];
	}
	
	// Update is called once per frame
	void Update () {
		m_InitialPoint = m_Player.transform.position;
		m_FinalPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector3 center = (m_InitialPoint + m_FinalPoint) * 0.5F;
		
		Vector3 riseRelCenter = m_InitialPoint - center;
		Vector3 setRelCenter = m_FinalPoint - center;
		
		
		for (int i = 0; i < m_Size; i++) {
			Vector3 newPos = Vector3.Slerp(riseRelCenter, setRelCenter, (i * 1.0f) / m_Size);
			m_Positions[i] = newPos;
			m_Positions[i] += center;
			// m_Positions[i].z = 0f;
		}

		m_Line.positionCount = m_Size;
		m_Line.SetPositions(m_Positions);
	}
}
