using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

	public float explosionTime = 0, heightAboveTrackForRespawn = 100f;
	public GameObject explosion;
	public Transform spawnPosition;
//	GameObject tempExplosion;
	float tempAcl;
	float tempTurn;
	bool isRespawning = false;
	
	void Awake () {
		//the time that the explosion lasts
//		explosionTime = explosion.particleSystem.duration;
		tempAcl = GetComponent<MotoPhysics> ().forwardThrust;
		tempTurn = GetComponent<MotoPhysics> ().turnStrength;
//		explosion = Resources.Load("MikeAssets/ExplosionMaterial/ExplosionParent.prefab") as GameObject;
	}

	IEnumerator Explode () 
	{
		isRespawning = true;
		print ("Explode");
		Instantiate(explosion, transform.position+new Vector3(0,4,0), Quaternion.identity);
//		tempExplosion = (GameObject)Instantiate(explosion);
		rigidbody.velocity = new Vector3(0,0,0);
		GetComponent<MotoPhysics> ().forwardThrust = 0;
		GetComponent<MotoPhysics> ().turnStrength = 0;
		foreach (MeshRenderer x in GetComponentsInChildren<MeshRenderer>())
			x.enabled = false;
		yield return new WaitForSeconds (1);
//		Destroy (tempExplosion);
		Transform spawnPosition = gameObject.GetComponent<CarData> ().lastWayPoint.transform;
		transform.position = new Vector3 (spawnPosition.position.x, spawnPosition.position.y + 20, spawnPosition.position.z) ;
		transform.rotation = spawnPosition.rotation;
		foreach (MeshRenderer x in GetComponentsInChildren<MeshRenderer>())
			x.enabled = true;
		isRespawning = false;
		gameObject.collider.enabled = true;
		yield return new WaitForSeconds (1);
		GetComponent<MotoPhysics> ().forwardThrust = tempAcl;
		GetComponent<MotoPhysics> ().turnStrength = tempTurn;
	}


	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "InstaKill" && isRespawning == false)
			StartCoroutine("Explode");
	}

	public void TriggerExplode(){
		StartCoroutine("Explode");
	}

	void Update () {
		float zRotation = gameObject.transform.rotation.eulerAngles.z;
		if ((zRotation > 50f && zRotation < 140f) || (zRotation > 220 && zRotation < 310f)) {
			if (isRespawning == false)
				StartCoroutine("Explode");		
		} 
	}
}
