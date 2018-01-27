using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid{

	public int Width;
	public int Height;

	public bool IsValidPos(Vector2 pos){
		if (pos.x < 0 || pos.x > this.Width)
			return false;
		if (pos.y < 0 || pos.y > this.Height)
			return false;
		return true;
	}

	public bool IsTileFree(Vector2 pos){
		if (!this.IsValidPos (pos))
			return false;
		//TODO
		return true;
	}

	public bool CanMakeMove(Soldier soldier, int movementDirection){
		Vector2 soldierPos = soldier.Position;
		Vector2 targetPos = soldierPos;
		switch(movementDirection){
		case Direction.UP:
			targetPos.y += 1;
			break;
		case Direction.DOWN:
			targetPos.y -= 1;
			break;
		case Direction.LEFT:
			targetPos.x -= 1;
			break;
		case Direction.RIGHT:
			targetPos.x += 1;
			break;
		}
		return this.IsTileFree (targetPos);
	}
}

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
    private bool teamAFinished = false;
	private bool teamBFinished = false;
	private List<SoldierController> soldiers;
	private GridCreator grid;

	public Grid GetGrid(){
		return new Grid ();//TODO
	}

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
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

    // Use this for initialization
    void Start () {
		// TODO: initialize soldier & code	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void SoliderUpdate(int team, Vector2 soldierPos) {
		if (System.Math.Abs(soldierPos.y) == grid.gridHeight || soldierPos.y == 0) {
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

	void Finish() {
		SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
	}

	public void IssueCommandTo(PlayerId playerId, Soldier soldier, int movementDirection){
		//TODO
	}

	public void EndTurn(PlayerId playerId){
		//TODO
	}
}