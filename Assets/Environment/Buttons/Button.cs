using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public enum Action {
		Toggle,
		Activate,
		Deactivate
	}

	public GameObject goInteractable;
	public Action actionWhenUse = Action.Activate;
	
	[SerializeField]
	private Interactable.Status m_InitialStatus = Interactable.Status.Deactivated;
	
	private Interactable m_Interactable;
	private SpriteRenderer m_Sprite;
	
	private Interactable.Status m_Status;

	void Awake () {
		if(goInteractable == null) {
			Debug.LogError("No goInteractable selected");
		}

		m_Interactable = goInteractable.GetComponent<Interactable>();
		m_Sprite = GetComponentInChildren<SpriteRenderer>();
		
		if(m_InitialStatus == Interactable.Status.Deactivated) {
			this.Deactivate(false);
		} else {
			this.Activate(false);
		}
	}
	
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		BoomerangBehaviour boom = coll.GetComponent<BoomerangBehaviour>();
		if(boom) {
			switch (actionWhenUse) {
				case Action.Activate:
					this.Activate();
					break;
				case Action.Deactivate:
					this.Deactivate();
					break;
				case Action.Toggle:
					if(m_Status == Interactable.Status.Deactivated) this.Activate();
					else this.Deactivate();
					break;
			}
		}
	}

	public void Activate(bool interactable = true) {
		if(interactable && m_Status != Interactable.Status.Activated) m_Interactable.Activate();		
		
		m_Sprite.color = Color.yellow;
		m_Status = Interactable.Status.Activated;
	}

	public void Deactivate(bool interactable = true) {
		if(interactable && m_Status != Interactable.Status.Deactivated) m_Interactable.Deactivate();		
		
		m_Sprite.color = Color.white;
		m_Status = Interactable.Status.Deactivated;
	}
}
