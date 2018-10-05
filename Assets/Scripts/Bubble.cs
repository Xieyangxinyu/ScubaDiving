using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    //Life Cycle is the duration of a bubble
    //lifeTime recorded the last Time the bubble's position is updated
	private float lifeTime, lifeCycle;

	private Boundary boundary;

	public void setBubble(GameObject fish, Boundary boundary){
		lifeCycle = Random.Range(100f, 150f);
		this.boundary = boundary;
		lifeTime = Time.time;
		Instantiate(fish, transform.position, Quaternion.identity).transform.SetParent(this.transform);
	}

	private void positionSwap(){
		transform.position = boundary.randomPosition();
		lifeTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		//check if the bubble is expired
		if (Time.time > lifeCycle + lifeTime) positionSwap();
	}

	//check if the bubble is popped by the avatar
	private void OnTriggerEnter(Collider hit){
		if(hit.tag == "Player"){
			positionSwap();
		}
	}
}
