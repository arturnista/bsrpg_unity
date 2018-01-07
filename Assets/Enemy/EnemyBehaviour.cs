using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	[SerializeField]
	private float m_MoveSpeed = 100f;
	[SerializeField]
	private float m_MaximumLife = 100f;
	[SerializeField]
	private float m_Defense = 1f;
	[SerializeField]
	private float m_AttackRate = 1f;
	
	private Player m_Player;
	private Rigidbody2D m_Rigidbody;

	private float m_Life;
	private bool m_ChasingPlayer = true;
	private float m_LastAttackTime = 0f;
	private float m_AttackTime = 1f;

	private Vector3 m_MoveVelocity;
	private Vector3 m_KnockbackVelocity;

	
	void Awake () {
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Player = GameObject.FindObjectOfType<Player>();

		m_KnockbackVelocity = Vector3.zero;
	}

	void Start () {
		m_Life = m_MaximumLife;
		m_AttackTime = 1 / m_AttackRate;
	}
	
	void Update () {
		m_ChasingPlayer = Vector2.Distance(m_Player.transform.position, transform.position) <= 5f;

		if(m_ChasingPlayer) {
			Vector3 lookingDirection = Vector3.Normalize(m_Player.transform.position - transform.position);
			float angle = Mathf.Atan2(lookingDirection.y, lookingDirection.x) * Mathf.Rad2Deg;
			
			transform.localEulerAngles = new Vector3(0, 0, angle);

			if(Time.time - m_LastAttackTime > m_AttackTime) {
				m_LastAttackTime = Time.time;
				// m_Player.Attack(10f);
			}
		}

		m_MoveVelocity = transform.right * m_MoveSpeed;

		m_Rigidbody.velocity = (m_MoveVelocity + m_KnockbackVelocity) * Time.deltaTime;
		this.ReduceKnockback();
	}

	void ReduceKnockback() {
		if(m_KnockbackVelocity.magnitude == 0) return;
		
		float subValue = 100f * Time.deltaTime;
		m_KnockbackVelocity = m_KnockbackVelocity.normalized * (m_KnockbackVelocity.magnitude - subValue);
	}
	
	public void Attack(float dmg, bool isCritical, Vector3 damageDir) {
		float defense = m_Defense;
		if(isCritical) defense = 1f;

		float damageTaken = dmg * (1 / defense);

		m_KnockbackVelocity = damageDir * damageTaken * 10f;

		m_Life -= damageTaken;
		if(m_Life <= 0) {
			Destroy(this.gameObject);
		}
	}
}
