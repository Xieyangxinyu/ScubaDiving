using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePage : MonoBehaviour {

	public GameObject image;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Time.time % 10 < 0.5)
			image.SetActive (true);
		else
			image.SetActive (false);
	}
}
