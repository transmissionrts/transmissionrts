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

	public void ExecuteCommand(Direction direction){
		this.Position = nextPosition;
		MoveableUnit moveableUnit = this.GetComponent<MoveableUnit> ();
		moveableUnit.playerId = team;
		moveableUnit.ExecuteCommand (direction);
		this.GetComponent<SelectableUnit> ().Deselect ();//???
	}
}