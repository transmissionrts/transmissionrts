using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
	// Use this for initialization
	public int power = 1;
	private Vector2 destination;
	private Vector2 grid;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		GameObject grid = GameObject.Find("Grid");
		this.transform.position = Vector3.Lerp(
			this.transform.position, 
			grid.GridToWorld(this.grid), 
			Time.deltaTime);
	}

	void Move(Vector2 destGrid, Vector2 destWorld) {
		this.grid = destGrid;
	}
}
