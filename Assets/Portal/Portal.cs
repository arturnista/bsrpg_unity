using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Player pl = coll.GetComponent<Player>();
		if(pl) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);    
		}
	}
}
