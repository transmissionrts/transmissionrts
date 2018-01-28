using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMover : MonoBehaviour {

	public Transform target;
	Vector3 targetPosition;
	public float speed;
	public float flapForceMax;
	public float flapForce;
	public float flapInterval;
	public float descentRatio;
	public float dropAltitude;
	float descentLength;

	enum FlightPhase {ToTarget, BackHome};

	FlightPhase phase = FlightPhase.ToTarget;
	Vector3 initialPosition;

	Rigidbody rb;
	Animator anim;
	float lastFlap = 0f;
	public float cruisingAltitude = 5;
	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		targetPosition = target.position;
		rb = GetComponent<Rigidbody> ();
		descentLength = descentRatio * (cruisingAltitude - dropAltitude);
	}


	// Update is called once per frame
	void FixedUpdate () {
		Vector3 toTarget = targetPosition - rb.position;
		FlapIfNeeded (toTarget);

		Vector3 horizontalDelta = toTarget;
		horizontalDelta.y = 0;
		//Vector3 delta = toTarget.normalized * Time.deltaTime * speed;
		//rb.MovePosition(rb.position + delta);

		rb.MoveRotation (Quaternion.LookRotation (horizontalDelta));

		switch (phase) {
		case FlightPhase.ToTarget:
			if (horizontalDelta.magnitude < 1) {
				dropPayload ();
				this.phase = FlightPhase.BackHome;
				targetPosition = initialPosition;
			}
			return;
		case FlightPhase.BackHome:
			if (horizontalDelta.magnitude < 1) {
				GetComponent<BirdMover> ().enabled = false;
			}
			break;
		}

	}
		
	void FlapIfNeeded(Vector3 toTarget) {
		// calculate right altitude
		float targetAltitude;
		float distance = toTarget.magnitude; 
		if (distance < descentLength) {	//TODO
			float progress = distance / descentLength;
			targetAltitude = dropAltitude + (cruisingAltitude - dropAltitude) * progress;
		} else {
			targetAltitude = cruisingAltitude;
		}

		if (canFlap()) {
			lastFlap = Time.time;
			float deltaAltitude = targetAltitude - rb.position.y;
			if (deltaAltitude > 0) {
				Vector3 v = rb.velocity;
				v.y = 0;
				rb.velocity = v;
				rb.AddForce (new Vector3 (0, Mathf.Min(deltaAltitude * flapForce, flapForceMax), 0));
			}
		}
		// lateral movement
		toTarget.y = 0;
		Vector3 delta = toTarget.normalized * speed;
		delta.y = rb.velocity.y;	// don't affect vertical movement
		rb.velocity = delta; 
		rb.AddForce(new Vector3(delta.x, 0, delta.z));

	}

	bool canFlap() {
		return (Time.time - lastFlap > flapInterval);
	}

	void dropPayload () {
		print ("reached target, delivering payload");
	}
}
