﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerId{
	PlayerA = 0,
	PlayerB = 1
}


[DefaultExecutionOrder(order: -900)]
public class GameManager : MonoBehaviour {
	public enum SoldierMovement {
		Won,
		Lost,
	}
	public enum GameState {
		InProgress,
		TeamAWon,
		TeamBWon,
		Draw,
	}

	public AbstractPlayer[] players;

	public AbstractPlayer PlayerA { get { return this.players [0]; } }
	public AbstractPlayer PlayerB  { get { return this.players [1]; } }

	private List<SoldierController> teamA;
	private List<SoldierController> teamB;

    private bool teamAFinished = false;
	private bool teamBFinished = false;
	private List<SoldierController> soldiers;
	public GridCreator gridCreator;

	[SerializeField]
	private LogicalGrid logicalGrid;
	public LogicalGrid GetGrid(){
		return this.logicalGrid;
	}


	[SerializeField]
	private BirdMover birdMoverPrefab;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private GameState currentGameState = GameState.InProgress;
    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }

	public int MyTeam;

    // Use this for initialization
    void Start () {
		this.gridCreator = GameObject.FindObjectOfType<GridCreator>();
		this.logicalGrid = this.gridCreator.GetGrid();
	}

	void Awake(){
		if (instance != null) {
			Debug.LogErrorFormat (this, "Duplicate GameManager instances [{0}, {1}]", this.name, instance.name);
			return;
		}
		this.players = new AbstractPlayer[2];
		instance = this;//registering self to static instance
	}

	void Finish() {
		SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
	}

	public bool SoldierUpdate(PlayerId team, SoldierController soldier, Vector2 nextPosition) {
		bool soldierOk = true;

		var soldierPos = soldier.Position;
		var enemy = this.logicalGrid.EncounterEnemy(team, nextPosition);

		if (enemy != null) {
			int rnd = Random.Range(0, 10);
			int winner = rnd % 2;
			if (PlayerIdToInt(team) == winner) {
				enemy.Kill();
			} else {
				soldier.Kill();
				soldierOk = false;
			}
		}

		if (soldierPos.y == gridCreator.gridHeight && PlayerIdToInt(team) == 0) {
			teamBFinished = true;
		} else if (soldierPos.y == 0 && PlayerIdToInt(team) == 1) {
			teamAFinished = true;
		}

		// update game state
		if (teamAFinished && teamBFinished) {
			currentGameState = GameState.Draw;
		} else if (teamAFinished) {
			currentGameState = GameState.TeamAWon;
		} else if (teamBFinished) {
			currentGameState = GameState.TeamBWon;
		}

		if (currentGameState != GameState.InProgress) {
			this.Finish();
		}

		return soldierOk;
	}

	public int PlayerIdToInt(PlayerId playerId){
		int idx = (int)playerId;
		if (idx < 0)
			idx = 0;
		if (idx > 1)
			idx = 1;
		return idx;
	}

	public AbstractPlayer GetPlayer(PlayerId playerId){
		int idx = this.PlayerIdToInt (playerId);
		return this.players [idx];
	}

	public SoldierController getSoilderByID(string id ) {
		SoldierController s = PlayerB.getSoilderByID(id);
		if(s != null) {
			int pos = PlayerB.getSoildersPos(id);
			pos = 4 - pos;
			s = PlayerA.getSoilderByPos(pos);
			return s;
		}

		SoldierController s2 = PlayerA.getSoilderByID(id);
		if(s2 != null) {
			int pos = PlayerA.getSoildersPos(id);
			pos = 4 - pos;
			s2 = PlayerB.getSoilderByPos(pos);
			return s2;
		}
		return null;
	}

	public void IssueCommandTo(PlayerId playerId, SoldierController soldier, Direction movementDirection){
		if (soldier.Team == playerId && this.logicalGrid.CanMakeMove(soldier, movementDirection)) {
			var targetPos = this.logicalGrid.GetTargetPos(soldier.Position, movementDirection);

			Debug.LogFormat("{0}:: MAKE MOVE from {1} to {2}", playerId, soldier.Position, targetPos);
			CommandPayload command = new CommandPayload () {
				Target = soldier.transform,
				Solider = soldier,
				direction = movementDirection,
				FinalPosition = targetPos
			};

			var player = this.GetPlayer (playerId);
			player.BirdMover.SetCommand (command);
		} else {
			Debug.LogFormat("CANNOT MAKE MOVE {0}, {1}", soldier.Position, movementDirection);
		}
	}

	public void EndTurn(PlayerId playerId){
		bool endTurn = true;
		foreach (var player in this.players) {
			if (player == null)
				continue;
			if (!player.BirdMover.HasPayload ())
				endTurn = false;
		}
		if (endTurn) {
			foreach (var player in this.players) {
				if (player == null)
					continue;
				player.TurnEnded ();
				player.BirdMover.Go();
			}
		}
    }

	public void RegisterPlayer(AbstractPlayer player){
		if (player == null)
			return;
		int idx = this.PlayerIdToInt (player.playerId);
		if (this.players [idx] == null) {
			this.players [idx] = player;
			if (idx == 0) {
				player.SetUp (this.logicalGrid.KingATransform.position, this.gridCreator.teamA);
			}
			if (idx == 1) {
				player.SetUp (this.logicalGrid.KingBTransform.position, this.gridCreator.teamB);
			}
			player.BirdMover = GameObject.Instantiate (this.birdMoverPrefab, player.transform.position, Quaternion.identity);

			this.StartCoroutine (this.doDelayedStart (player));
			return;
		}
		Debug.LogErrorFormat (player, "Duplicate player '{0}' [{1}, {2}]", idx, player.name, this.players[idx].name);
	}

	private IEnumerator doDelayedStart(AbstractPlayer player){
		yield return null;
		player.PlayTurn ();
	}

	public void TurnExecuted(PlayerId playerId) {
		var player = this.GetPlayer (playerId);

		this.GoToNextTurn ();
	}

	public void GoToNextTurn(){
		bool goToNextTurn = true;
		foreach (var player in this.players) {
			if (player == null)
				continue;
			Debug.LogFormat (player, "GoToNextTurn: {0}.ReadyForNextTurn = {1}", player.name, player.ReadyForNextTurn);
			if (!player.ReadyForNextTurn){
				goToNextTurn = false;
				break;
			}
		}
		Debug.LogFormat (this, "GoToNextTurn: {0}", goToNextTurn);
		if(goToNextTurn){
			foreach (var player in this.players) {
				if (player == null)
					continue;
				player.ResetTurn ();
				player.PlayTurn ();
			}
		}
	}
}
