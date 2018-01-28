using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : MonoBehaviour {

	public PlayerId playerId;

	[SerializeField]
	protected GameManager gameManager;

	[SerializeField]
	protected BirdMover birdMover;

	public BirdMover BirdMover{ get { return this.birdMover; } }

	protected virtual void Start(){
		Debug.LogWarningFormat ("{0}.Start()", this.name);
		this.gameManager = GameManager.Instance;
		this.gameManager.RegisterPlayer (this);

		if (this.birdMover == null)
			Debug.LogErrorFormat (this, "Player {0} is missing BirdMover", this.name);
	}

	public abstract void ResetTurn ();
}
