using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	private int totalNumOfBubbles = 10;
	private Bubble[] bubbles = new Bubble[10 + 10];
	public GameObject[] fishes;
	public Boundary boundary;
	public Bubble sample;
	public Shark shark;
	public Avatar target;

	void Start () {
		for (int i = 0; i < totalNumOfBubbles; i ++){
			int temp = i % 10;
			bubbles[i] = Instantiate(sample, boundary.randomPosition(), Quaternion.identity);
			bubbles[i].setBubble(fishes[temp], boundary);
		}
		Shark thisShark = Instantiate(shark, boundary.randomPosition(), Quaternion.identity);
		thisShark.setShark(target);
	}
}
