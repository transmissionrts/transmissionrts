﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(order: -799)]
public class LocalPlayer : AbstractPlayer {

	private SelectableUnit selectedUnit;
	private Transform pigeon;
	public Direction nextCommand = Direction.NONE;

	public CommandSelectorButton commandSelectorButton;

	[SerializeField]
	private bool isNetworkedScene;
	private NetworkedMessanger messanger;

	protected override void Start ()
	{
		base.Start ();
		this.commandSelectorButton = GameObject.FindObjectOfType<CommandSelectorButton> ();
		if (this.isNetworkedScene)
			messanger = NetworkedMessanger.Instance;
	}

	public bool IsValidCommand(Direction direction){
		return direction != Direction.NONE;
	}

	public void SelectedCommand(Direction command)
	{
		if (!this.IsValidCommand (command))
			return;
		this.nextCommand = command;
	}

	public void SelectedUnit(SelectableUnit selectedUnit){
		if (!this.IsValidCommand (this.nextCommand)) {
			Debug.LogWarningFormat (this, "{0} wont move. cause {1} has invalid command {2}", selectedUnit.name, this.name, nextCommand);
			return;
		}
		SoldierController soldier = selectedUnit.GetComponent<SoldierController> ();
		if (soldier != null) {
				
			if (soldier.Team != this.playerId) {
				return;
			}

			if (this.selectedUnit != null)
				this.selectedUnit.Deselect ();
			
			this.selectedUnit = selectedUnit;
			this.selectedUnit.Select ();


			this.gameManager.IssueCommandTo (this.playerId, soldier, this.nextCommand);
			if (this.isNetworkedScene && messanger != null)
				messanger.SendMessage (this.playerId.ToString () + "-" + soldier.id + "-" + this.nextCommand.ToString ());
			this.gameManager.EndTurn (this.playerId);
		}
	}

	public override void PlayTurn ()
	{
		base.PlayTurn ();
	}

	public override void TurnEnded ()
	{
		base.TurnEnded ();
	}

	public override void ResetTurn(){
		if (this.selectedUnit != null)
			this.selectedUnit.Deselect ();
		this.commandSelectorButton.UnselectAll ();
		this.nextCommand = Direction.NONE;
		base.ResetTurn ();
	}
}
