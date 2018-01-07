using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : MonoBehaviour {

	private Rigidbody2D m_Rigidbody;
	private Transform m_Sprite;	
	private Player m_Player;	

	[SerializeField]
	private float m_MaximumDist = 5f;

	[SerializeField]
	private float m_Speed = 5f;

	private float m_SpriteAngle;

	private bool m_EaseCurve;

	/* FLY CIRCLE VARIABLES */
	private Vector3 m_FinalPosition;
	private Vector3 m_InitialPosition;
	private Vector3 m_CenterPosition;
	private Vector3 m_DiffPosition;
	private float m_InitialAngle;
	private float m_AngleOffset;
	private float m_Range;
	private float m_StartTime;
	private float m_Duration;

	/* FLY STRAIGHT VARIABLES */
	private Vector3 m_Direction;

	enum Status {
		None,
		FlyingCircle,
		FlyingStraight,
		Stopped
	}
	private Status m_Status;

	void Awake () {
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Player = GameObject.FindObjectOfType<Player>();
		m_Sprite = transform.Find("Sprite");
		m_SpriteAngle = 0f;
		m_Status = Status.None;
	}

	public void StraightThrow(Vector3 initial, Vector3 final) {
		m_Direction = Vector3.Normalize(final - initial);
		transform.position = initial + (m_Direction * 2.1f);
		m_Rigidbody.velocity = m_Direction * m_Speed;

		m_Status = Status.FlyingStraight;
	}

	public void Throw(Vector3 initial, Vector3 final, bool ease) {
		m_StartTime = Time.time;

		m_InitialPosition = initial;
		m_InitialPosition.z = 0f;
		final.z = 0f;

		m_FinalPosition = Vector2.Distance(m_InitialPosition, final) > m_MaximumDist ? 
			(m_InitialPosition + Vector3.Normalize(final - m_InitialPosition) * m_MaximumDist) : final;

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
		m_Status = Status.FlyingCircle;
	}

	void Update () {
		if(m_Status == Status.None) return;
		if(m_Status == Status.Stopped) {
			m_Rigidbody.velocity = Vector3.zero;
			return;
		}

		m_SpriteAngle = (m_SpriteAngle + (500f * Time.deltaTime)) % 360;
		m_Sprite.eulerAngles = new Vector3(0f, 0f, m_SpriteAngle);

		if(m_Status == Status.FlyingCircle) {
			FlyCircle();
		} else if(m_Status == Status.FlyingStraight) {
			FlyStraight();			
		}
	}

	void FlyCircle() {
		float time = (Time.time - m_StartTime) / m_Duration;
		Vector3 newPosition = transform.position;

		if(time > .9f) {
			Vector3 dir = Vector3.Normalize(m_Player.transform.position - transform.position);
			m_Rigidbody.velocity = dir * ( (2f * Mathf.PI * m_Range) / m_Duration );
		} else {
			float angle = Mathf.Deg2Rad * (Mathf.Lerp(m_InitialAngle, 360f, time) - m_AngleOffset);

			Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

			if(m_EaseCurve) {
				float dTime = time * 2;
				float relTime = time >= .5 ? 2 - dTime : dTime;
				float fTime = relTime >= .5 ? relTime : 1 - relTime;
				dir *= fTime;
			}
			newPosition = m_CenterPosition + m_InitialPosition + ( dir * m_Range );
		}

		transform.position = newPosition;
	}

	void FlyStraight() {

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
