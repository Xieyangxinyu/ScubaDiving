using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR;

public class Avatar : MonoBehaviour
{
    private Camera m_Camera;
    private CharacterController m_CharacterController;
	public Boundary boundary;
	[SerializeField] private InputC m_MouseLook;

    //FirstPerson moving spead
    private float speed;
	private bool dead;
	private float deadtime;

	public void die(){
		deadtime = Time.time;
		dead = true;
	}

    // Use this for initialization
    private void Start()
    {
		//Initial set-up
		transform.position = boundary.randomPosition();
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
		speed = 10f;
        m_MouseLook.Init(transform, m_Camera.transform);
    }

	private void stop(){
		speed *= -1;
	}

    private void Update()
    {
		if (dead) {
			if (deadtime + 10 < Time.time)
				dead = false;
			return;
		}
		RotateView();

		//Boundaries of limited swimming spaces
		if (!boundary.inBound(transform.position))
			stop();
		else if (speed < 0) speed = 10f;

        //Derection setup
		Vector3 moveDirection = transform.forward;
		moveDirection.y = m_Camera.transform.forward.y;
		m_CharacterController.Move(speed * moveDirection * Time.fixedDeltaTime);
		m_MouseLook.UpdateCursorLock();
    }

    //Rotate View by reading inputs from VR headset or Trackpad
    private void RotateView(){
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }
}
