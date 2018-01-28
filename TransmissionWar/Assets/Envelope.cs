using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envelope : MonoBehaviour {

	public CommandPayload payload;

	public PlayerId playerId;

	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter (Collision col) {
	 string name = col.gameObject.name;
		if (col.gameObject.GetComponent<BirdMover>() == null) {	// not the pigeon itself
			print ("Delivering message to " + col.gameObject.name);
			deliverMessage ();
		}
	}

	void deliverMessage() {
		if (this.payload != null) {
			if (this.payload.Solider != null) {
				this.payload.Solider.ExecuteCommand (this.payload.Direction);
			}
			this.payload = null;
			GameManager.Instance.PayloadDelivered(this.playerId);
		}
		Destroy (gameObject);
	}
}
