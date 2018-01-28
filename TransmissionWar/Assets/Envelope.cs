using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envelope : MonoBehaviour {

	public CommandPayload payload;

	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude == 0) {
			deliverMessage ();
		}
	}

	void deliverMessage() {
		if (this.payload != null) {
			if (this.payload.Solider != null) {
				this.payload.Solider.ExecuteCommand (this.payload.Direction);
			}
			this.payload = null;
		}
		GameManager.Instance.PayloadDelivered();
		Destroy (gameObject);
	}
}
