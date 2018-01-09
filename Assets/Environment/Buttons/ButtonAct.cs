using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class ButtonAct {  

    public ButtonAct(Interactable _int, Button.Action _ac) {
        interactable = _int;
        actionWhenUse = _ac;
    }

    public Interactable interactable;
    public Button.Action actionWhenUse;

}