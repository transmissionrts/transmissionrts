using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour {
	int team;
	public int Team {
		get {return team;}
		set {team = value;}
	}
    SelectableUnit selectable;

	// Use this for initialization
	void Start () {
        selectable = GetComponent<SelectableUnit>();
    }
	
	// Update is called once per frame
	void Update () {
	}

	public void Select() {
		this.selectable.Select();
	}
	public void Deselect() {
		this.selectable.Deselect();
	}
}
