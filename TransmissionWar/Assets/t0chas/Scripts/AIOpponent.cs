using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;

public class Soldier {
	public Vector2 Position;
}

public struct ScoredSoldier{
	public Soldier Soldier;
	public float LikelyToWin;
	public int MinMovesToWin;
}

public class AIOpponent : MonoBehaviour {

	IBehaviourTreeNode rootNode;

	GameManager gameManager;

	List<ScoredSoldier> ScoreUnits(List<Soldier> units, int targetGridRow){

		Grid grid = this.gameManager.GetGrid ();

		List<ScoredSoldier> scoredSoldiers = new List<ScoredSoldier>();
		for (int i = 0; i < units.Count; i++) {
			ScoredSoldier scored = new ScoredSoldier ();
			scored.Soldier = units [i];

			float minMovesLeft = Mathf.Abs(targetGridRow - scored.Soldier.Position.y);
			scored.MinMovesToWin = (int)minMovesLeft;
			scored.LikelyToWin = minMovesLeft / grid.Height;
			scoredSoldiers.Add (scored);
		}

		//not optimal//
		scoredSoldiers.Sort((x, y) => x.LikelyToWin.CompareTo(y.LikelyToWin));

		return scoredSoldiers;
	}

	public List<Soldier> GetMyUnits(){
		return new List<Soldier> ();//TODO
	}


	private class AIData{
		public List<ScoredSoldier> myScoredUnits;
	}

	private AIData aiData = new AIData();

	void PlayTurn(){
		List<Soldier> myUnits = this.GetMyUnits ();
		this.aiData.myScoredUnits = this.ScoreUnits(myUnits, 0);

		this.rootNode.Tick (new TimeData (Time.deltaTime));
	}

	IBehaviourTreeNode BuildSimpleAI(){
		BehaviourTreeBuilder treeBuilder = new BehaviourTreeBuilder ();
		IBehaviourTreeNode node = treeBuilder
			.Sequence ("simpleAI", true)
			.Do ("DoWeHaveUnits", t => {
				if(this.aiData != null && this.aiData.myScoredUnits != null && this.aiData.myScoredUnits.Count > 0)
					return BehaviourTreeStatus.Success;
				return BehaviourTreeStatus.Failure;
			})
			.Do("SendCommandTo", t=>{
				Grid grid = this.gameManager.GetGrid ();

				foreach(ScoredSoldier scored in this.aiData.myScoredUnits){
					List<Movement> posibleMoves = new List<Movement>();
					if(grid.CanMakeMove(scored.Soldier, Movement.Up))
						posibleMoves.Add(Movement.Up);
					if(grid.CanMakeMove(scored.Soldier, Movement.Down))
						posibleMoves.Add(Movement.Down);
					if(grid.CanMakeMove(scored.Soldier, Movement.Left))
						posibleMoves.Add(Movement.Left);
					if(grid.CanMakeMove(scored.Soldier, Movement.Right))
						posibleMoves.Add(Movement.Right);

					if(posibleMoves.Count == 0){
						//Dam!
						continue;
					}

					int selectedMove = Random.Range(0, posibleMoves.Count -1);

					this.gameManager.IssueCommandTo(scored.Soldier, posibleMoves[selectedMove]);
					return BehaviourTreeStatus.Success;
				}
				return BehaviourTreeStatus.Failure;
			})
			.End ()
			.Build ();
		return node;
	}

	void Awake(){

		this.gameManager = GameManager.Instance;

		BehaviourTreeBuilder treeBuilder = new BehaviourTreeBuilder ();
		this.rootNode = treeBuilder.Selector ("SomeSelector", true)
			.Do ("some-action-1", t => {
				return BehaviourTreeStatus.Success;
			})
			.Do ("some-action-2", t => {
				return BehaviourTreeStatus.Success;
			})
			.End ()
			.Build ();

		this.rootNode = this.BuildSimpleAI ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
