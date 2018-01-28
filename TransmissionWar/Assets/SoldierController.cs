using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(order: 100)]
public class SoldierController : MonoBehaviour {
	PlayerId team;
	public string id;
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

	public void ExecuteCommand(Direction direction, Vector2 nextPosition){
		// deselects the unit
		this.GetComponent<SelectableUnit> ().Deselect ();

		// first update the logical state with game manager
		if (GameManager.Instance.SoldierUpdate(Team, nextPosition)) {
			MoveableUnit moveableUnit = this.GetComponent<MoveableUnit> ();

			moveableUnit.playerId = team;
			moveableUnit.ExecuteCommand(direction);
			GameManager.Instance.GetGrid().RegisterSoldier(this, nextPosition);

			// update succeeded, update the actual game unit; this needs to be done LAST
			this.Position = nextPosition;
		} else {
		}
	}
}