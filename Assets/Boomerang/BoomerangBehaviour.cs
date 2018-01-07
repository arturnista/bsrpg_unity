using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : MonoBehaviour {

	[SerializeField]
	private float m_MaxMoveSpeed;

	private Rigidbody2D m_Rigidbody;
	private Transform m_Sprite;	

	private float m_SpriteAngle;

	private float m_DistanceTraveled;
	private float m_Distance;

	private Vector3 m_Direction;

	enum Status {
		Forward,
		Backward,
		Stopped
	}
	private Status m_Status;

	void Awake () {
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Sprite = transform.Find("Sprite");
		m_SpriteAngle = 0f;
		m_Distance = 10f;
	}

	public void Throw(Vector3 initial, Vector3 final) {
		m_Direction = Vector3.Normalize(final - initial);
	}

	void Update () {
		if(m_Status == Status.Stopped) return;

		m_SpriteAngle = (m_SpriteAngle + 10f) % 360;
		m_Sprite.eulerAngles = new Vector3(0f, 0f, m_SpriteAngle);

		float travel = Time.deltaTime * m_MaxMoveSpeed;
		if (m_Status == Status.Forward) {
			transform.Translate(m_Direction * travel); // moves object
			m_DistanceTraveled += travel; // update distance
			m_Status = m_DistanceTraveled >= m_Distance ? Status.Backward : Status.Forward; // goes back if distance reached
		} else if (m_Status == Status.Backward) {
			transform.Translate(m_Direction * -travel); // moves object
			m_DistanceTraveled -= travel; // update distance;
			m_Status = m_DistanceTraveled < -3f ? Status.Stopped : Status.Backward; // turning off when done
		}
	}

	public void Pick() {
		// m_Status = Status.Stopped;
		Destroy(this.gameObject);
	}
}
