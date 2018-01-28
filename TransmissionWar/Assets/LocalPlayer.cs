﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : AbstractPlayer {

	private SelectableUnit selectedUnit;
	private Transform pigeon;
	private int nextCommand;

	public CommandSelectorButton commandSelectorButton;

	protected override void Start ()
	{
		base.Start ();
		this.commandSelectorButton = GameObject.FindObjectOfType<CommandSelectorButton> ();
	}

	public void SelectedCommand(int command)
	{
		this.nextCommand = command;
	}

	public void SelectedUnit(SelectableUnit selectedUnit){
		SoldierController soldier = selectedUnit.GetComponent<SoldierController>();
		if (soldier != null) {
			
			if (soldier.Team != GameManager.Instance.MyTeam) {
				return;
			}

			if (this.selectedUnit != null)
				this.selectedUnit.Deselect ();
		
			this.selectedUnit = selectedUnit;
			this.selectedUnit.Select ();
			///????

			this.gameManager.IssueCommandTo (this.playerId, soldier, this.nextCommand);
			this.gameManager.EndTurn (this.playerId);
		}
	}

	public override void ResetTurn(){
		if (this.selectedUnit != null)
			this.selectedUnit.Deselect ();
		this.commandSelectorButton.UnselectAll ();
	}


}