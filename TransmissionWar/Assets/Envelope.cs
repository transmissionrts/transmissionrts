using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envelope : MonoBehaviour {

	public CommandPayload payload;

	public PlayerId playerId;

	public AudioClip catEFX1;
	public AudioClip catEFX2;
	public AudioClip dogEFX1;
	public AudioClip dogEFX2;

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
				if (this.payload.Solider.Team == PlayerId.PlayerA) {
					SoundManager.instance.RandomizeSfx (catEFX1, catEFX2);// Add efx
				} else {
					SoundManager.instance.RandomizeSfx (dogEFX1, dogEFX2);
				}
				this.payload.Solider.ExecuteCommand (this.payload.direction, this.payload.FinalPosition);
			}
			this.payload = null;
			//GameManager.Instance.PayloadDelivered(this.playerId);
		}
		Destroy (gameObject);
	}
}
