using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject boomerangBrefab;

	[SerializeField]
	private float m_MoveSpeed = 40f;
	[SerializeField]
	private float m_MaximumLife = 100f;
	
	private bool m_HasBoomerang;
	
	private Rigidbody2D m_Rigidbody;

	private float m_Life;

	void Awake () {
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_HasBoomerang = true;
	}

	void Start () {
		m_Life = m_MaximumLife;
	}
	
	void Update () {
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector3 moveVelocity = new Vector3(horizontal, vertical) * m_MoveSpeed;

		m_Rigidbody.velocity = Vector3.ClampMagnitude(moveVelocity, m_MoveSpeed);

		Vector3 lookingDirection = Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;
		
		transform.localEulerAngles = new Vector3(0, 0, angle);

		if(Input.GetMouseButtonDown(0)) {
			this.Throw(lookingDirection);
		}
	}

	void Throw(Vector3 lookingDirection) {
		if(!m_HasBoomerang) return;

		Vector3 boomPos = (lookingDirection * 3f) + transform.position;
		BoomerangBehaviour boom = Instantiate(boomerangBrefab, boomPos, Quaternion.identity).GetComponent<BoomerangBehaviour>();
		boom.Throw(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		
		m_HasBoomerang = false;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		BoomerangBehaviour boom = coll.GetComponent<BoomerangBehaviour>();
		if(boom) {
			boom.Pick();
			m_HasBoomerang = true;
		}
	}

}
