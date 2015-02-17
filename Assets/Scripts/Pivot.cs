using UnityEngine;
using System.Collections;

public class Pivot : MonoBehaviour {
	
	void OnTriggerEnter (Collider col) {
		if(col.tag == "Player") {
			GetComponent<Animator>().SetTrigger("isTriggered");
		}
	}
	
	void Update () {

	}
}
