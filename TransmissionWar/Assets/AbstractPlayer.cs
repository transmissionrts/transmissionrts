using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(order: -800)]
public abstract class AbstractPlayer : MonoBehaviour {

	public PlayerId playerId;

	[SerializeField]
	protected GameManager gameManager;

	[SerializeField]
	private BirdMover birdMover;

	public BirdMover BirdMover{ 
		get { return this.birdMover; }
		set { 
			this.birdMover = value; 
			this.birdMover.playerId = this.playerId;
		}
	}

	[SerializeField]
	public bool ReadyForNextTurn;

	[SerializeField]
	private bool isPlayingTurn;

	public bool IsPlayingTurn { get { return this.isPlayingTurn; } }

	protected List<SoldierController> soldiers;

	public virtual void SetUp(Vector3 position, List<Transform> units){
		this.transform.position = position;
		this.soldiers = new List<SoldierController> ();
		foreach (var u in units) {
			SoldierController soldier = u.GetComponent<SoldierController> ();
			if (soldier != null) {
				soldier.Team = this.playerId;
				this.soldiers.Add (soldier);
			}
		}
	}

	protected virtual void Start(){
		Debug.LogWarningFormat ("{0}.Start()", this.name);
		this.gameManager = GameManager.Instance;
		this.gameManager.RegisterPlayer (this);

		if (this.birdMover == null)
			Debug.LogErrorFormat (this, "Player {0} is missing BirdMover", this.name);
	}

	public virtual void PlayTurn (){
		this.isPlayingTurn = true;
	}

	public virtual void TurnEnded(){
		this.isPlayingTurn = false;
	}

	public abstract void ResetTurn ();
}
