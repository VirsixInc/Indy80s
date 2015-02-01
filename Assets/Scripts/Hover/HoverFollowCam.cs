using UnityEngine;
using System.Collections;

public class HoverFollowCam : MonoBehaviour
{
//  float m_camHeight;
//  float m_camDist;
  public GameObject m_player;
  int m_layerMask;
  
  public bool isFlipped = false;

  public GameObject lookDir; 
	public GameObject lookDir2;

	Vector3 worldUpForCamera;

	public Vector3 offset;

  void Start()
  {
	//m_player = GetComponentInParent<Transform> ().gameObject;
//    Vector3 offsetCam = transform.position - m_player.transform.position;
//    m_camHeight = offsetCam.y;
//    m_camDist = Mathf.Sqrt(
//      offsetCam.x * offsetCam.x + 
//      offsetCam.z * offsetCam.z);

    m_layerMask = 1 << LayerMask.NameToLayer("Characters");
    m_layerMask = ~m_layerMask;
		if(isFlipped){
			worldUpForCamera = -transform.up;
		}
		else{
			worldUpForCamera = transform.up;
		}

  }
	
  void Update()
  {	

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
	transform.LookAt(m_player.transform.position, worldUpForCamera);

  }
	void LateUpdate(){

		//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
	}
}
