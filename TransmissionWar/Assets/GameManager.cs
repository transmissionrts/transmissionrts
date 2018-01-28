using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerId{
	PlayerA = 0,
	PlayerB = 1
}

public class GameManager : MonoBehaviour {
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

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

	void Awake(){
		if (instance != null) {
			Debug.LogErrorFormat (this, "Duplicate GameManager instances [{0}, {1}]", this.name, instance.name);
			return;
		}
		instance = this;//registering self to static instance
		this.gridCreator = GameObject.FindObjectOfType<GridCreator>();
		this.players = new AbstractPlayer[2];
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
		// TODO: initialize soldier & code
		this.logicalGrid = this.gridCreator.GetGrid();
	}

	// Update is called once per frame
	void Update () {
	}

	void Finish() {
		SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
	}

	void SoliderUpdate(int team, Vector2 soldierPos) {
		if (System.Math.Abs(soldierPos.y) == gridCreator.gridHeight || soldierPos.y == 0) {
			if (team == 0) {
				teamAFinished = true;
			} else {
				teamBFinished = true;
			}
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
	}

	public int PlayerIdToInt(PlayerId playerId){
		int idx = (int)playerId;
		if (idx < 0)
			idx = 0;
		if (idx > 1)
			idx = 1;
		return idx;
	}

	public void IssueCommandTo(PlayerId playerId, SoldierController soldier, int movementDirection){
		int idx = this.PlayerIdToInt (playerId);
		CommandPayload command = new CommandPayload () {
			Target = soldier.transform,
			Solider = soldier,
			Direction = movementDirection
		};
		this.players [idx].BirdMover.SetCommand (command);
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
			return;
		}
		Debug.LogErrorFormat (player, "Duplicate player '{0}' [{1}, {2}]", idx, player.name, this.players[idx].name);
	}

	public void PayloadDelivered(){
		foreach (var player in this.players) {
			if (player == null)
				continue;
			player.ResetTurn ();///?????
		}
	}
}
