using UnityEngine;
using System.Collections;

public class IntroAnimationConditions : MonoBehaviour {

//	MotoPhysics[] hoverCarControls;
//	float tempAcl;
//	float tempTurn;
//	Animation introAnimation;
//	public GameObject[] cars;
//
//	void Start () {
//		introAnimation = GameObject.FindGameObjectWithTag ("CarCamera").GetComponent<Animation>();
//		hoverCarControls = GetComponentsInChildren<MotoPhysics> ();
//		tempAcl = hoverCarControls [0].forwardThrust; //good have each of these for each car if they had variable speed
//		tempTurn = hoverCarControls [0].turnStrength;
//		StartCoroutine ("FreezeCarControls");
//		cars = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];
//	}
//
//	IEnumerator FreezeCarControls () {
//		while (introAnimation.isPlaying) {
//			foreach (MotoPhysics x in hoverCarControls) {
//				x.forwardThrust = 0;
//				x.turnStrength = 0;
//			}
//			yield return new null;
//		}
//		yield return new WaitForSeconds (0.1f);
//		foreach (MotoPhysics y in hoverCarControls) {
//			y.forwardThrust = tempAcl;
//			y.turnStrength = tempTurn;
//		}
//	}
}
