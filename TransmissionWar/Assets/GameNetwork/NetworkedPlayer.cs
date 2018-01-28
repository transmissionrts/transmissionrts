using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPlayer : AbstractPlayer {

	// Use this for initialization
	void Start () {

		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void PlayTurn(){
		base.PlayTurn ();
//		List<SoldierController> myUnits = this.GetMyUnits ();
//		this.aiData.myScoredUnits = this.ScoreUnits(myUnits, 0);
//
//		this.rootNode.Tick (new TimeData (Time.deltaTime));
//
//		this.gameManager.EndTurn (this.playerId);

//		int selectedMoveIdx = Random.Range (0, posibleMoves.Count);
//		Debug.LogFormat(this, "{0}.selectedMove: {1}", this.name,  posibleMoves [selectedMoveIdx]);
//		this.gameManager.IssueCommandTo (this.playerId, scored.Soldier, posibleMoves [selectedMoveIdx]);
	}

	public override void TurnEnded ()
	{
		base.TurnEnded ();
	}

	public override void ResetTurn ()
	{
		base.ResetTurn ();
	}
}
