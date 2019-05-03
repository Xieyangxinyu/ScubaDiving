using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

	private int originX = 100, originY = 2, originZ = 100;
	private int boundaryX = 400, boundaryY = 40, boundaryZ = 400;

	private Vector3 randomPos = new Vector3();


    //return a random position within the boundary
	public Vector3 randomPosition(){
		randomPos.x = Random.Range(originX, boundaryX);
		randomPos.y = Random.Range(originY, boundaryY);
		randomPos.z = Random.Range(originZ, boundaryZ);
		return randomPos;
	}

    //Check if an object is within the boundary
	public bool inBound(Vector3 pos){
		if (pos.x >= boundaryX || pos.x <= originX) return false;
		if (pos.y >= boundaryY || pos.y <= originY) return false;
		if (pos.z >= boundaryZ || pos.z <= originZ) return false;
		return true;
	}
}
