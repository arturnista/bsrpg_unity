﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public enum Status {
		Activated,
		Deactivated
	}
	protected Interactable.Status m_Status;

	public virtual void Activate(bool force = false) {

	}

	public virtual void Deactivate(bool force = false) {
		
	}

	public virtual void Toggle() {
		
	}
}
