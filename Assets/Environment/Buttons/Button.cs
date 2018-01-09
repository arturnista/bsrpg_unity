using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public enum Action {
		Toggle,
		Activate,
		Deactivate
	}

	[SerializeField]
	private List<ButtonAct> m_Interactables;
	[SerializeField]
	private Interactable.Status m_InitialStatus = Interactable.Status.Deactivated;
	[SerializeField]
	private Action m_ActionWhenUse = Action.Activate;
	
	private TimeController m_TimeController;
	private CameraBehaviour m_CameraBehaviour;
	private SpriteRenderer m_Sprite;
	
	private Interactable.Status m_Status;

	void Awake () {
		m_Sprite = GetComponentInChildren<SpriteRenderer>();
		
		m_TimeController = GameObject.FindObjectOfType<TimeController>();
		m_CameraBehaviour = GameObject.FindObjectOfType<CameraBehaviour>();

		if(m_InitialStatus == Interactable.Status.Deactivated) {
			this.Deactivate(true);
		} else {
			this.Activate(true);
		}
	}
	
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		BoomerangBehaviour boom = coll.GetComponent<BoomerangBehaviour>();
		if(boom) {
			switch (m_ActionWhenUse)
			{
				case Action.Activate:
					if(m_Status != Interactable.Status.Activated) this.Activate();
					break;
				case Action.Deactivate:
					if(m_Status != Interactable.Status.Deactivated) this.Deactivate();
					break;
				case Action.Toggle:
					if(m_Status == Interactable.Status.Deactivated) this.Activate();
					else this.Deactivate();
					break;
			}
		}
	}

	public void Activate(bool isInitial = false) {
		if(!isInitial) {
			foreach (ButtonAct bAction in m_Interactables) {
				switch (bAction.actionWhenUse) {
					case Action.Activate:
						if(bAction.interactable != null) bAction.interactable.Activate();		
						break;
					case Action.Deactivate:
						if(bAction.interactable != null) bAction.interactable.Deactivate();		
						break;
					case Action.Toggle:
						if(bAction.interactable != null) bAction.interactable.Toggle();		
						break;
				}
			}
		}

		m_TimeController.SlowTime(0.2f, .3f);
		m_Sprite.color = Color.yellow;
		m_Status = Interactable.Status.Activated;
	}

	public void Deactivate(bool isInitial = false) {
		if(!isInitial) {
			foreach (ButtonAct bAction in m_Interactables) {
				switch (bAction.actionWhenUse) {
					case Action.Activate:
						if(bAction.interactable != null) bAction.interactable.Deactivate();		
						break;
					case Action.Deactivate:
						if(bAction.interactable != null) bAction.interactable.Activate();		
						break;
					case Action.Toggle:
						if(bAction.interactable != null) bAction.interactable.Toggle();		
						break;
				}
			}
		}

		m_Sprite.color = Color.white;
		m_Status = Interactable.Status.Deactivated;
	}
}
