using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableUnit : MonoBehaviour {

    public bool selected;

    Transform selectIndicator;

	// Use this for initialization
	void Start () {
        TransformUtils tu = new TransformUtils();
        selectIndicator = tu.GetChildByName("SelectIndicator", transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
