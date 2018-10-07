using System;
using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class Avatar : MonoBehaviour
{
    private Camera m_Camera;
	public Boundary boundary;
    // Read inputs from VR and trackpad.
	[SerializeField] private InputC m_MouseLook;

    //FirstPerson moving spead
    private float speed;
	private const float INIT_SPEED = 5f;
	private bool dead;
	private const int FROZEN_TIME = 10;
	private float deadtime;

	// die() is called once shark get the user
	public void die(){
		deadtime = Time.time;
		dead = true;
	}

    // Use this for initialization
    private void Start()
    {
		//Initial set-up
		transform.position = boundary.randomPosition();
        m_Camera = Camera.main;
		speed = INIT_SPEED;
        m_MouseLook.Init(transform, m_Camera.transform);
    }

	// stop() is called once over the border
	private void stop()
	{
		speed = 0.5f;
	}

    private void Update()
    {
		if (dead) {
			// Once the shark get the user, user will be freezed for 10sec 
			if (deadtime + FROZEN_TIME < Time.time)
				dead = false; // Revived from death
			else return;
		}
		RotateView();

		Vector3 moveDirection = transform.forward;
        // over the border
		if (!boundary.inBound(transform.position)) stop();
        // set user's speed to the initial speed
		// if the user could be back into the boundary with double initial speed at the current direction 
		if(boundary.inBound(2 * INIT_SPEED * moveDirection * Time.fixedDeltaTime + transform.position)) speed = INIT_SPEED;

        //Derection setup
		moveDirection.y = m_Camera.transform.forward.y;
		transform.position += speed * moveDirection * Time.fixedDeltaTime;
		m_MouseLook.UpdateCursorLock();
    }

    //Rotate View by reading inputs from VR headset or Trackpad
    private void RotateView(){
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }
}
