using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : MonoBehaviour {

	public PlayerId playerId;

	protected GameManager gameManager;

	protected virtual void Start(){
		this.gameManager = this.gameManager;
	}
}
