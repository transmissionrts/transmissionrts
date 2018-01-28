using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : AbstractPlayer {

	private SelectableUnit selectedUnit;
	private Transform pigeon;
	private int nextCommand;

	protected override void Start ()
	{
		base.Start ();
	}

	public void SelectedCommand(int command)
	{
		this.nextCommand = command;
	}

	public void SelectedUnit(SelectableUnit selectedUnit){
		this.selectedUnit = selectedUnit;
		///????
		SoldierController soldier = this.selectedUnit.GetComponent<SoldierController>();
		if (soldier != null) {
			this.gameManager.IssueCommandTo (this.playerId, soldier, this.nextCommand);
			this.gameManager.EndTurn (this.playerId);
		}
	}


}
