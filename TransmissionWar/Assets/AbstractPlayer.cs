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
				soldier.id = u.position.ToString();
				this.soldiers.Add (soldier);
			}
		}
		//this.gameManager.GetGrid ().SetUpSoliders (soldiers);
	}
//	// not getting right teams soldienrs
	public SoldierController getSoilderByID(string id ) {
		foreach (SoldierController s in soldiers) {
			if(s.id == id) {
				return s;
			}
		}
		return null;
	}

	public int getSoildersPos(string id ) {
		int pos = 0;
		foreach (SoldierController s in soldiers) {
			
			if(s.id == id) {
				return pos;
			}
			pos++;
		}
		return -1;
	}
	public SoldierController getSoilderByPos(int pos ) {
		return soldiers[pos];
	}

	protected virtual void Start(){
		Debug.LogWarningFormat (this, "{0}.Start()", this.name);
		this.gameManager = GameManager.Instance;
		this.gameManager.RegisterPlayer (this);

		if (this.birdMover == null)
			Debug.LogErrorFormat (this, "Player {0} is missing BirdMover", this.name);
	}

	public virtual void PlayTurn (){
		Debug.LogFormat (this, "{0}.PlayTurn()", this.name);
		this.ReadyForNextTurn = false;
		this.isPlayingTurn = true;
	}

	public virtual void TurnEnded(){
		Debug.LogFormat (this, "{0}.TurnEnded()", this.name);
		this.isPlayingTurn = false;
	}

	public virtual void ResetTurn (){
		this.ReadyForNextTurn = false;
		this.isPlayingTurn = false;
		this.birdMover.ResetPayload ();
	}
}
