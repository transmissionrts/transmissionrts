using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking;

public class NetworkedPlayer : AbstractPlayer {

	bool recievedMessage;

	NetworkedMessanger messanger;

	// Use this for initialization
	void Start () {

		base.Start();
		messanger = NetworkedMessanger.Instance;
	}
	
	// Update is called once per frame
	void Update () {
//		isPlayingTurn
		if(messanger.lastMessage != null && this.IsPlayingTurn) {

			string[] msg = messanger.lastMessage.Split('-');
			bool isServer = bool.Parse(msg[3]);

			// message not from local player
			if(isServer == false && NetworkServer.active) {
				playNetworkedMove(messanger.lastMessage);
			}
			if(isServer == true && !NetworkServer.active) {
				playNetworkedMove(messanger.lastMessage);
			}


		}

	}
//
//	public void SelectedUnit(SelectableUnit selectedUnit){
//		
//		SoldierController soldier = selectedUnit.GetComponent<SoldierController> ();
//		if (soldier != null) {
//
//			if (soldier.Team != this.playerId) {
//				return;
//			}
//
//			if (this.selectedUnit != null)
//				this.selectedUnit.Deselect ();
//
//			this.selectedUnit = selectedUnit;
//			this.selectedUnit.Select ();
//
//
//			this.gameManager.IssueCommandTo (this.playerId, soldier, this.nextCommand);
//			this.gameManager.EndTurn (this.playerId);
//		}
//	}

	public void playNetworkedMove(string msg) {
		string[] message = msg.Split('-');

//		PlayerId opponentId = message[0] == "PlayerA" ? PlayerId.PlayerA : PlayerId.PlayerB; 
		string soldierID = message[1];
		Direction command = NetworkedDirectionHelper.getDirection(message[2]);
		Debug.Log("NETWORKED MESSAGE!: " + messanger.lastMessage);
		this.gameManager.IssueCommandTo (this.playerId,this.gameManager.getSoilderByID(soldierID), command);
		this.gameManager.EndTurn (this.playerId);
		messanger.lastMessage = null;
	}

	public override void PlayTurn(){

		base.PlayTurn();


//		List<SoldierController> myUnits = this.GetMyUnits ();
//		this.aiData.myScoredUnits = this.ScoreUnits(myUnits, 0);
//
//		this.rootNode.Tick (new TimeData (Time.deltaTime));
//
//		this.gameManager.EndTurn (this.playerId);

//		int selectedMoveIdx = Random.Range (0, posibleMoves.Count);
//		Debug.LogFormat(this, "{0}.selectedMove: {1}", this.name,  posibleMoves [selectedMoveIdx]);
//		this.gameManager.IssueCommandTo (this.playerId, scored.Soldier, posibleMoves [selectedMoveIdx]);
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
