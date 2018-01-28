using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPayload {
	public Transform Target;
	public SoldierController Solider;
	public int Direction;

}

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


    bool hasTarget;

    enum FlightPhase {ToTarget, BackHome};

	FlightPhase phase = FlightPhase.ToTarget;
	Vector3 initialPosition;

	Rigidbody rb;
	Animator anim;
	float lastFlap = 0f;
	public float cruisingAltitude = 5;

	CommandPayload payload;
	Material withEnvelopeMaterial;
	public Material noEnvelopeMaterial;
	public Transform envelope;

	public PlayerId playerId;

	// Use this for initialization
	void Awake () {
		initialPosition = transform.position;

        SetTarget(target);

        rb = GetComponent<Rigidbody> ();
		descentLength = descentRatio * (cruisingAltitude - dropAltitude);
	}

	void Start() {
		withEnvelopeMaterial = GetComponent<MeshRenderer> ().material;
	}

	public void SetCommand(CommandPayload payload){
		this.payload = payload;
	}

    public void SetTarget(Transform newTarget) {
        target = newTarget;

        if (newTarget != null) {
            targetPosition = target.position;
            phase = FlightPhase.ToTarget;
			if (withEnvelopeMaterial != null) {
				GetComponent<MeshRenderer>().materials = new Material[]{withEnvelopeMaterial};
			}
        }
        else
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
            return;

        Vector3 toTarget = targetPosition - rb.position;
        FlapIfNeeded(toTarget);

        Vector3 horizontalDelta = toTarget;
        horizontalDelta.y = 0;
        //Vector3 delta = toTarget.normalized * Time.deltaTime * speed;
        //rb.MovePosition(rb.position + delta);

        rb.MoveRotation(Quaternion.LookRotation(horizontalDelta));

        switch (phase)
        {
            case FlightPhase.ToTarget:
                if (horizontalDelta.magnitude < 1)
                {
                    dropPayload();
                    this.phase = FlightPhase.BackHome;
                    targetPosition = initialPosition;
                }
                return;
            case FlightPhase.BackHome:
                if (horizontalDelta.magnitude < 1)
                {
                    SetTarget(null);
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
		print ("reached target, dropping envelope");

		GetComponent<MeshRenderer> ().materials = new Material[]{noEnvelopeMaterial};
		Quaternion orientation = Quaternion.Euler(0, 0, 0);
		Vector3 dropPosition = targetPosition;
		dropPosition.y = rb.position.y - 0.1f;
		Vector3 v = rb.velocity;	// don't let the envelope be seen! 
		v.y = 0;
		rb.velocity = v;
		Transform envObj = Instantiate(envelope, position: dropPosition, rotation: orientation);
		Envelope env = envObj.GetComponent<Envelope> ();
		env.payload = payload;
		env.playerId = this.playerId;
	}

	public void Go(){
		if (!this.HasPayload ())
			return;
		this.SetTarget (this.payload.Target);
	}

	public bool HasPayload(){
		return this.payload != null;
	}

	public void ResetPayload(){
		this.payload = null;
	}
}
