using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour {

	int team;
    SelectableUnit selectable;
	public Vector2 Position;

	// Use this for initialization
	void Start () {
        selectable = GetComponent<SelectableUnit>();
    }
	
	// Update is called once per frame
	void Update () {
	}

	public void SetTeam(int teamId) {
		team = teamId;
	}
}
