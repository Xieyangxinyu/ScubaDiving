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

	public void setShark(Avatar target) {
		anim = GetComponent<Animation>();
        scream = GetComponent<AudioSource>();
        moveSpeed = 2f;
		this.target = target;
	}

	// Update is called once per frame
	void Update () {
		
		if (scream.isPlaying) {
			return;
		}

		targetR = target.transform.position - transform.position;

		float distance = targetR.x * targetR.x + targetR.y * targetR.y + targetR.z * targetR.z;

		if (distance < 15) {
			anim.CrossFade ("eat");
		}
		else if(distance < 500){
			moveSpeed = 3f;
			anim.CrossFade ("fastswim");
		}
		else {
			moveSpeed = 2f;
			anim.CrossFade ("swim");
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetR), rotationSpeed * Time.deltaTime);
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}
	void OnTriggerEnter(Collider hit){
		//if the bubble hits the FirstPerson
		if (hit.tag == "Player") {
			scream.Play ();
			target.die();
		}
	}
}
