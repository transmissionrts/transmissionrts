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

	public void IssueCommandTo(PlayerId playerId, SoldierController soldier, int movementDirection){
		//TODO
	}

	public void EndTurn(PlayerId playerId){
        // TODO
    }


	public AbstractPlayer PlayerA;
	public AbstractPlayer PlayerB;
	public void RegisterPlayer(AbstractPlayer player){
		if (player == null)
			return;
		if (player.playerId == PlayerId.PlayerA)
			this.PlayerA = player;
		if (player.playerId == PlayerId.PlayerB)
			this.PlayerB = player;
	}

	public void PayloadDelivered(){
		if (this.PlayerA != null)
			this.PlayerA.ResetTurn ();
		if (this.PlayerB != null)
			this.PlayerA.ResetTurn ();
	}
}
