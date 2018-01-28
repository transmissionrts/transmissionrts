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

	public int turnN = 0;

	public override void PlayTurn(){
		Debug.LogFormat(this, "{0} Playing Turn: {1}", this.name, this.turnN);
		base.PlayTurn ();
		List<SoldierController> myUnits = this.GetMyUnits ();
		this.aiData.myScoredUnits = this.ScoreUnits(myUnits, 0);

		this.rootNode.Tick (new TimeData (Time.deltaTime));

		Debug.LogFormat(this, "{0} Ending Turn: {1}", this.name, this.turnN);
		this.gameManager.EndTurn (this.playerId);
	}

	public override void TurnEnded ()
	{
		Debug.LogFormat(this, "{0} Turn: {1} Ended", this.name, this.turnN);
		this.turnN++;
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

				List<Direction> posibleMoves = new List<Direction> ();
				SoldierController selectedSoldier = null;
				int tryCount = 0;
				while(selectedSoldier == null && tryCount < 25){
					tryCount++;
					foreach (ScoredSoldier scored in this.aiData.myScoredUnits) {

						posibleMoves.Clear();
						int rnd = Random.Range(0, 10);
						if(rnd % 2== 0)
							continue;
						
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
							Debug.LogWarningFormat(scored.Soldier, "{0}[{1}] Has no possible moves", scored.Soldier.name, scored.Soldier.Position);
							continue;
						}
						selectedSoldier = scored.Soldier;
						break;
					}
					if(selectedSoldier != null){
						Debug.LogFormat(selectedSoldier, "{0} SelectedSoldier: {1}; moves: {2}", this.name, selectedSoldier.name, posibleMoves.Count);
						int selectedMove = Random.Range (0, posibleMoves.Count);
						Debug.LogFormat(this, "{0}[t:{1}].selected: {2} Move: {3}", this.name, this.turnN, selectedSoldier.name,  selectedMove);
						this.gameManager.IssueCommandTo (this.playerId, selectedSoldier, posibleMoves [selectedMove]);
						return BehaviourTreeStatus.Success;
					}
					Debug.LogWarningFormat(this, "{0} Cant find soldier with moves", this.name);
			}
			Debug.LogWarningFormat(this, "{0} return Failure", this.name);
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
