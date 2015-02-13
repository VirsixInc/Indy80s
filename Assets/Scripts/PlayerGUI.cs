using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {
	public bool flipCam = false;

	Image lap, place;

	public Sprite[] places, laps;

	void Awake() {
		lap = transform.FindChild("Canvas").FindChild("CurLap").GetComponent<Image>();
		place = transform.FindChild("Canvas").FindChild("Place").GetComponent<Image>();

		if (flipCam) {
			transform.parent.GetComponentInChildren<HoverFollowCam>().flipCam = flipCam;
			transform.FindChild("Canvas").localEulerAngles = new Vector3(0f, 0f, 180f);
		}
	}

	public void UpdatePlace(int newPlace) {
		place.sprite = places [newPlace];
	}

	public void UpdateLap(int newLap) {
		lap.sprite = laps [newLap];
	}
}
