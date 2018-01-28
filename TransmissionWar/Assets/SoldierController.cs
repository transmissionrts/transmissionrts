using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(order: 100)]
public class SoldierController : MonoBehaviour {
	PlayerId team;
	public PlayerId Team {
		get {return team;}
		set {team = value;}
	}

    SelectableUnit selectable;
	public Vector2 Position;



	// Use this for initialization
	void Start () {
        selectable = GetComponent<SelectableUnit>();

    }
		
	public void Select() {
		this.selectable.Select();
	}
	public void Deselect() {
		this.selectable.Deselect();
	}

	public void ExecuteCommand(int direction, Vector2 nextPosition){
		this.Position = nextPosition;
		this.GetComponent<MoveableUnit> ().ExecuteCommand (direction);
		this.GetComponent<SelectableUnit> ().Deselect ();//???
	}
}