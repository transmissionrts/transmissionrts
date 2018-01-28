using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {
    Camera cam;

    public Transform selectedUnit, pigeon;
    public int nextCommand;

	// Use this for initialization
	void Start () {
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
                SoldierController soldierController;
                if (selectedUnit != null) {
                    soldierController = selectedUnit.GetComponent<SoldierController>();
                    soldierController.Deselect();
                } 

                selectedUnit = hitTransform;
                soldierController = selectedUnit.GetComponent<SoldierController>();

                if (soldierController.Team == GameManager.Instance.MyTeam) {
                    soldierController.Select();
                    MoveableUnit unit = selectedUnit.GetComponent<MoveableUnit>();
                    unit.command = nextCommand;
                    pigeon.GetComponent<BirdMover>().SetTarget(selectedUnit);
                }
            }
        }
    }
}
