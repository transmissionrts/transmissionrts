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

[DefaultExecutionOrder(order: -801)]
public class AIOpponent : AbstractPlayer {

	IBehaviourTreeNode rootNode;

	List<ScoredSoldier> ScoreUnits(List<SoldierController> units, int targetGridRow){

		if (units == null) {
			Debug.LogWarningFormat (this, "{0} units is empty", this.name);
			return new List<ScoredSoldier> ();
		}

		LogicalGrid grid = this.gameManager.GetGrid ();

		List<ScoredSoldier> scoredSoldiers = new List<ScoredSoldier>();
		for (int i = 0; i < units.Count; i++) {
			ScoredSoldier scored = new ScoredSoldier ();
			scored.Soldier = units [i];

			float minMovesLeft = Mathf.Abs(targetGridRow - scored.Soldier.Position.y);
			scored.MinMovesToWin = (int)minMovesLeft;
			scored.LikelyToWin = minMovesLeft / grid.Height;
			scoredSoldiers.Add (scored);

			Debug.LogFormat (scored.Soldier, "ScoreUnits[{0}]: minMovesLeft: {1}; ", scored.Soldier.name, scored.MinMovesToWin, scored.LikelyToWin);
		}

		//not optimal//
		scoredSoldiers.Sort((x, y) => x.LikelyToWin.CompareTo(y.LikelyToWin));

		return scoredSoldiers;
	}

	public List<SoldierController> GetMyUnits(){
		return this.soldiers;
	}


	private class AIData{
		public List<ScoredSoldier> myScoredUnits;
	}

	private AIData aiData = new AIData();

	public override void PlayTurn(){
		base.PlayTurn ();
		List<SoldierController> myUnits = this.GetMyUnits ();
		this.aiData.myScoredUnits = this.ScoreUnits(myUnits, 0);

		this.rootNode.Tick (new TimeData (Time.deltaTime));

		this.gameManager.EndTurn (this.playerId);
	}

	public override void TurnEnded ()
	{
		base.TurnEnded ();
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
					int rnd = Random.RandomRange(0, 10);
					if(rnd % 2== 0)
						continue;
				List<Direction> posibleMoves = new List<Direction> ();
					List<string> posibleMovesStr = new List<string> ();
					if (grid.CanMakeMove (scored.Soldier, Direction.UP)){
						posibleMoves.Add (Direction.UP);
						posibleMovesStr.Add("Up");
					}
					if (grid.CanMakeMove (scored.Soldier, Direction.DOWN)){
						posibleMoves.Add (Direction.DOWN);
						posibleMovesStr.Add("Down");
					}
					if (grid.CanMakeMove (scored.Soldier, Direction.LEFT)){
						posibleMoves.Add (Direction.LEFT);
						posibleMovesStr.Add("Left");
					}
					if (grid.CanMakeMove (scored.Soldier, Direction.RIGHT)){
						posibleMoves.Add (Direction.RIGHT);
						posibleMovesStr.Add("Right");
					}

					Debug.LogFormat("{0}:: posibleMoves: {1}", scored.Soldier.name,  string.Join(", ", posibleMovesStr.ToArray()));
				if (posibleMoves.Count == 0) {
					//Dam!
					continue;
				}

				int selectedMove = Random.Range (0, posibleMoves.Count);
				Debug.LogFormat(this, "{0}.selectedMove: {1}", this.name,  selectedMove);
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
		base.Start ();
	}

	public override void ResetTurn ()
	{
		base.ResetTurn ();
	}
}
