using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shark : MonoBehaviour {

	private Avatar target;
	AudioSource scream;
	public float rotationSpeed;
	private float moveSpeed;
	public Animation anim;
	private Vector3 targetR;
	private const int EAT = 15; // Distance to eat
	private const int FAST = 500; // Distance to accelerate
    private const int SLOW = 250; // Distance to accelerate

    // initialize the shark following the Avatar
    public void setShark(Avatar target) {
		anim = GetComponent<Animation>();
        scream = GetComponent<AudioSource>();
        moveSpeed = 2f;
		this.target = target;
	}

	// Update is called once per frame
	void Update () {
		
		if (scream.isPlaying) {
			// the shark got the Avatar
			return;
		}

		targetR = target.transform.position - transform.position;

		float distance = targetR.x * targetR.x + targetR.y * targetR.y + targetR.z * targetR.z;

		if (distance < EAT) {
			anim.CrossFade ("eat");
		}
		else if(distance < FAST && distance > SLOW){
			moveSpeed = 3f;
			anim.CrossFade ("fastswim");
		}
        else{
			moveSpeed = 1f;
			anim.CrossFade ("swim");
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetR), rotationSpeed * Time.deltaTime);
		Vector3 velocity = transform.forward * moveSpeed;
		transform.position += velocity * Time.deltaTime;
	}
    
	void OnTriggerEnter(Collider hit){
		// check if the shark hits the FirstPerson
		if (hit.tag == "Player") {
			scream.Play ();
			target.die();
		}
	}
}
