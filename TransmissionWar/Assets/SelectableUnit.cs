using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour {

    public bool selected;

    Transform selectIndicator;

    MoveableUnit moveableUnit;
    bool isMoveable;

	// Use this for initialization
	void Start () {
        TransformUtils tu = new TransformUtils();
        selectIndicator = tu.GetChildByName("SelectIndicator", transform);
        selectIndicator.gameObject.SetActive(false);

        moveableUnit = GetComponent<MoveableUnit>();
        if (moveableUnit != null) {
            isMoveable = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select()
    {
        selected = true;
        selectIndicator.gameObject.SetActive(true);
        if (isMoveable) {
            moveableUnit.OnSelect();
        }
    }
    public void Deselect()
    {
        selected = false;
        selectIndicator.gameObject.SetActive(false);
    }
}
