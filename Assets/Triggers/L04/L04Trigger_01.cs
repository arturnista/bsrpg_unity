using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L04Trigger_01 : MonoBehaviour {

	public GameObject goDoor;
	private CameraBehaviour m_Camera;

	void Awake () {
		m_Camera = GameObject.FindObjectOfType<CameraBehaviour>();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Player pl = coll.GetComponent<Player>();
		if(pl) {
			Door d = goDoor.GetComponent<Door>();
			d.Deactivate();
			m_Camera.Focus(goDoor.transform.position, 2f);
			
			Destroy(this.gameObject);
		}
	}

}
