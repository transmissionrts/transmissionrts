using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;

/*public class Soldier {
	public Vector2 Position;
}/*/

public struct ScoredSoldier{
	public SoldierController Soldier;
	public float LikelyToWin;
	public int MinMovesToWin;
}

public class AIOpponent : AbstractPlayer {

	public PlayerId playerId;


	IBehaviourTreeNode rootNode;

	GameManager gameManager;

	List<ScoredSoldier> ScoreUnits(List<SoldierController> units, int targetGridRow){

		LogicalGrid grid = this.gameManager.GetGrid ();

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

	public List<SoldierController> GetMyUnits(){
		return new List<SoldierController> ();//TODO
	}


	private class AIData{
		public List<ScoredSoldier> myScoredUnits;
	}

	private AIData aiData = new AIData();

	void PlayTurn(){
		List<SoldierController> myUnits = this.GetMyUnits ();
		this.aiData.myScoredUnits = this.ScoreUnits(myUnits, 0);

		this.rootNode.Tick (new TimeData (Time.deltaTime));

		this.gameManager.EndTurn (this.playerId);
	}

	IBehaviourTreeNode BuildSimpleAI(){
		BehaviourTreeBuilder treeBuilder = new BehaviourTreeBuilder ();
		IBehaviourTreeNode node = treeBuilder
			.Sequence ("simpleAI", true)
			.Do ("DoWeHaveUnits", t => {
			if (this.aiData != null && this.aiData.myScoredUnits != null && this.aiData.myScoredUnits.Count > 0)
				return BehaviourTreeStatus.Success;
			return BehaviourTreeStatus.Failure;
		})
			.Do ("SendCommandTo", t => {
			LogicalGrid grid = this.gameManager.GetGrid ();

			foreach (ScoredSoldier scored in this.aiData.myScoredUnits) {
				List<int> posibleMoves = new List<int> ();
				if (grid.CanMakeMove (scored.Soldier, Direction.UP))
					posibleMoves.Add (Direction.UP);
				if (grid.CanMakeMove (scored.Soldier, Direction.DOWN))
					posibleMoves.Add (Direction.DOWN);
				if (grid.CanMakeMove (scored.Soldier, Direction.LEFT))
					posibleMoves.Add (Direction.LEFT);
				if (grid.CanMakeMove (scored.Soldier, Direction.RIGHT))
					posibleMoves.Add (Direction.RIGHT);

				if (posibleMoves.Count == 0) {
					//Dam!
					continue;
				}

				int selectedMove = Random.Range (0, posibleMoves.Count - 1);

				this.gameManager.IssueCommandTo (this.playerId, scored.Soldier, posibleMoves [selectedMove]);
				return BehaviourTreeStatus.Success;
			}
			return BehaviourTreeStatus.Failure;
		})
			.End ()
			.Build ();
		return node;
	}

	protected override void Start ()
	{
		base.Start ();

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

	public override void ResetTurn ()
	{
		
	}
}
