using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour {
	public float waterLevel;
	private UnityEngine.Color underwaterColor;

	// Use this for initialization of fog color
	void Start () {
		underwaterColor = new UnityEngine.Color(0.22f, 0.65f, 0.77f, 0.5f);
	}

	// Show fog if the FirstPerson is under water level
	void Update () {
		if(transform.position.y < waterLevel)
			setUnderwater ();
		else
			RenderSettings.fog = false;
	}

	void setUnderwater(){
		RenderSettings.fog = true;
		RenderSettings.fogColor = underwaterColor;
		RenderSettings.fogDensity = 0.03f;
	}
}
