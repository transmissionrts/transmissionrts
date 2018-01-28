using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {
    Camera cam;

    public Transform pigeon;

	public LocalPlayer localPlayer;

	// Use this for initialization
	void Start () {
		this.localPlayer = GameObject.FindObjectOfType<LocalPlayer> ();

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
        RaycastHit hit;
        Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit);
        Vector3 point = hit.point;

        Transform hitTransform = hit.transform;
        if (hitTransform != null)
            if (hitTransform.name.Contains("Soldier")) {
                Debug.Log("hit");

                SelectableUnit su = hitTransform.GetComponent<SelectableUnit>();
				this.localPlayer.SelectedUnit (su);
            }
        }
    }
}
