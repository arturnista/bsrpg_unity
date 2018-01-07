using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : MonoBehaviour {

	private Rigidbody2D m_Rigidbody;
	private Transform m_Sprite;	

	private float m_SpriteAngle;

	private float m_DistanceTraveled;
	private float m_Distance;

	private bool m_EaseCurve;

	private Vector3 m_FinalPosition;
	private Vector3 m_InitialPosition;
	private Vector3 m_CenterPosition;
	private Vector3 m_DiffPosition;
	private float m_InitialAngle;
	private float m_AngleOffset;
	private float m_Range;
	private float m_StartTime;
	private float m_Duration;

	private Vector3 m_Direction;

	enum Status {
		None,
		Flying,
		Stopped
	}
	private Status m_Status;

	void Awake () {
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Sprite = transform.Find("Sprite");
		m_SpriteAngle = 0f;
		m_Status = Status.None;
	}

	public void Throw(Vector3 initial, Vector3 final, bool ease) {
		m_Status = Status.Flying;

		m_Direction = Vector3.Normalize(final - initial);
		m_StartTime = Time.time;

		m_InitialPosition = initial;
		m_FinalPosition = final;

		m_InitialPosition.z = 0f;
		m_FinalPosition.z = 0f;

		m_DiffPosition = m_FinalPosition - m_InitialPosition;
		m_CenterPosition = m_DiffPosition * 0.5f; 
		m_Range = m_CenterPosition.magnitude;
		m_Duration = m_Range / 2;

		m_AngleOffset = Mathf.Atan2(m_DiffPosition.normalized.y, m_DiffPosition.normalized.x) * Mathf.Rad2Deg;
		m_AngleOffset += 90;

		m_InitialAngle = (1 / m_Range) * 100f;

		float angle = m_InitialAngle - m_AngleOffset;
		Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
		Vector3 newPosition = m_CenterPosition + m_InitialPosition + ( dir * m_Range );
		transform.position = newPosition;

		m_EaseCurve = ease;
	}

	void Update () {
		if(m_Status != Status.Flying) return;

		m_SpriteAngle = (m_SpriteAngle + 10f) % 360;
		m_Sprite.eulerAngles = new Vector3(0f, 0f, m_SpriteAngle);

		float time = (Time.time - m_StartTime) / m_Duration;
		float angle = Mathf.Deg2Rad * (Mathf.Lerp(m_InitialAngle, 360f, time) - m_AngleOffset);

		Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

		if(m_EaseCurve) {
			float dTime = time * 2;
			float relTime = time >= .5 ? 2 - dTime : dTime;
			float fTime = relTime >= .5 ? relTime : 1 - relTime;
			dir *= fTime;
		}

		Vector3 newPosition = m_CenterPosition + m_InitialPosition + ( dir * m_Range );

		transform.position = newPosition;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.layer == LayerMask.NameToLayer("Wall")) {
			m_Status = Status.Stopped;
		}
	}

	public void Pick() {
		if(m_Status == Status.None) return;
		Destroy(this.gameObject);
	}
}
