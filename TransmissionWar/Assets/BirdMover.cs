using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMover : MonoBehaviour {

	public Transform target;
	Vector3 targetPosition;
	public float speed;
	public float flapForce;
	public float flapInterval;

	enum FlightPhase {ToTarget, BackHome};

	FlightPhase phase = FlightPhase.ToTarget;
	Vector3 initialPosition;

	Rigidbody rb;
	Animator anim;
	float lastFlap = 0f;
	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		targetPosition = target.position;
		rb = GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void FixedUpdate () {
		//FlapIfNeeded ();

		Vector3 towardsTarget = targetPosition - rb.position;
		Vector3 delta = towardsTarget.normalized * Time.deltaTime * speed;
		rb.MovePosition(rb.position + delta);
		rb.MoveRotation (Quaternion.LookRotation (towardsTarget));

		switch (phase) {
		case FlightPhase.ToTarget:
			if (towardsTarget.magnitude < .1) {
				dropPayload ();
				this.phase = FlightPhase.BackHome;
				targetPosition = initialPosition;
			}
			return;
		case FlightPhase.BackHome:
			if (towardsTarget.magnitude < .1) {
				GetComponent<BirdMover> ().enabled = false;
			}
			break;
		}

	}
		
	void FlapIfNeeded() {
		if (Time.time - lastFlap > flapInterval) {
			lastFlap = Time.time;
			rb.AddForce (new Vector3 (0, flapForce));
		}
	}

	void dropPayload () {
		print ("reached target, delivering payload");
	}
}
