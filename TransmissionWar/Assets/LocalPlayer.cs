using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(order: -799)]
public class LocalPlayer : AbstractPlayer {

	private SelectableUnit selectedUnit;
	private Transform pigeon;
	public int nextCommand = Direction.NONE;

	public CommandSelectorButton commandSelectorButton;

	protected override void Start ()
	{
		base.Start ();
		this.commandSelectorButton = GameObject.FindObjectOfType<CommandSelectorButton> ();
	}

	public bool IsValidCommand(int direction){
		return direction >= 0 && direction < 4;
	}

	public void SelectedCommand(int command)
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
