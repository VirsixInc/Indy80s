using UnityEngine;
using System.Collections;

public class FallOffTrack : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){



	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == ("Player")){
			Rigidbody body = other.GetComponent<Rigidbody>();
			other.GetComponent<MotoPhysics>().enabled = false;
			other.enabled = false;
			print(body.velocity);
			body.AddForce(other.GetComponent<Rigidbody>().velocity);
			body.drag = 0f;
			StartCoroutine("DelayedRespawn", other);
			
		}
	}

	//coroutine waits a couple seconds, turns on HoverCarControl, Calls Respawn. 
	IEnumerator DelayedRespawn(Collider other){
		yield return new WaitForSeconds(.5f);
		other.GetComponent<MotoPhysics>().enabled = true;
		other.GetComponent<Rigidbody>().drag = 3f;
		other.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
		other.GetComponent<Respawn>().TriggerExplode();

	}
}
