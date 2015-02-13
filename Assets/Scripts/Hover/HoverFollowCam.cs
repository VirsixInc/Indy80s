using UnityEngine;
using System.Collections;

public class HoverFollowCam : MonoBehaviour {
//  float m_camHeight;
//  float m_camDist;
	public GameObject m_player;
	int m_layerMask;
	public GameObject lookDir;
	public GameObject lookDir2;
	[System.NonSerialized]
	public Vector3 offset;

	[HideInInspector]
	public bool flipCam; // Set in PlayerGUI

	void Start() {
		m_layerMask = 1 << LayerMask.NameToLayer("Characters");
		m_layerMask = ~m_layerMask;
	}
	
	void Update() {	

		Vector3 camPos = m_player.transform.position + -m_player.transform.forward * offset.z + m_player.transform.up * offset.y;

		camera.transform.position = camPos;


//    Vector3 camOffset = -m_player.transform.forward;
//		//Debug.DrawRay(m_player.transform.position, -m_player.transform.forward*10f);
//    camOffset = new Vector3(camOffset.x, 0.0f, camOffset.z) * m_camDist
//      + m_player.transform.up * m_camHeight;
//
//		Debug.DrawRay(m_player.transform.position, m_player.transform.up*10f);
//	
//		Debug.DrawRay(m_player.transform.position, camOffset*m_camDist);
//
//    RaycastHit hitInfo;
//    if (Physics.Raycast(m_player.transform.position, camOffset, 
//                       out hitInfo, m_camDist, 
//                       m_layerMask))
//    {
//      transform.position = hitInfo.point;
//    } 
//	else
//    {
//      transform.position = m_player.transform.position + camOffset;
//    }
		transform.LookAt(m_player.transform.position, flipCam ? -transform.up : transform.up);

	}

	void LateUpdate() {

		//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
	}
}
