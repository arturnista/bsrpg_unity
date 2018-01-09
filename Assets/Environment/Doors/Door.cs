using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

	private Transform m_Handler;
	[Range(1, 10)]
	public int amountNecessary = 1;
	public Interactable.Status initialStatus;

	private int m_Amount;

	void Awake () {
		m_Handler = transform.Find("Door");
		m_Amount = 0;
		if(initialStatus == Interactable.Status.Deactivated) {
			Deactivate(true);
		} else {
			Activate(true);
		}
	}
	
	void Update () {
		
	}

	public override void Activate(bool force = false) {
		m_Status = Interactable.Status.Activated;
		if(amountNecessary > 1) {
			if(force || ++m_Amount >= amountNecessary) {
				m_Handler.gameObject.SetActive(false);
			}
		} else {
			m_Handler.gameObject.SetActive(false);			
		}
	}

	public override void Deactivate(bool force = false) {
		m_Status = Interactable.Status.Deactivated;
		m_Handler.gameObject.SetActive(true);	
		if(!force) m_Amount--;
	}

	public override void Toggle() {
		if(m_Status == Interactable.Status.Deactivated) this.Activate();
		else this.Deactivate();
	}
}
