using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

	private Transform m_Handler;
	[Range(1, 10)]
	public int amountNecessary = 1;

	private int m_Amount;

	void Awake () {
		m_Handler = transform.Find("Door");
		m_Amount = 0;
		m_Status = Interactable.Status.Deactivated;
	}
	
	void Update () {
		
	}

	public override void Activate() {
		m_Status = Interactable.Status.Activated;
		if(++m_Amount >= amountNecessary) {
			m_Handler.gameObject.SetActive(false);
		}
	}

	public override void Deactivate() {
		m_Status = Interactable.Status.Deactivated;
		m_Handler.gameObject.SetActive(true);	
		m_Amount--;
	}
}
