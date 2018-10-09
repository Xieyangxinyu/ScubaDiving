using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    private string nametime; //used to name the save file
    private int totalNumOfBubbles = 10;
	private Bubble[] bubbles = new Bubble[10+2];
	public GameObject[] fishes;
    // 10 kinds of fish models
	private const int FISH_MODEL = 10;
	public Boundary boundary;
	public Bubble sample;
	public Shark shark;
    private Shark thisShark;
	public GameObject jellyfish;
	public GameObject particles;
	public Avatar target;
    private string[] list; //list to save data to a text file
    private string template;

    void Start () {
        nametime = System.DateTime.Now.ToString("hh-mm-ss-tt-MM-dd-yyyy");
        list = new string[] { "Keycode        Time           X-Position     Y-Position     Z-Position     X-Rotation     Y-Rotation     Z-Rotation     " };
        template = "                    ";

        for (int i = 0; i < totalNumOfBubbles; i ++){
			int model = i % FISH_MODEL;
			// Instantiate returns bubble Object the instantiated clone.
			bubbles[i] = Instantiate(sample, boundary.randomPosition(), Quaternion.identity);
			bubbles[i].setBubble(fishes[model], boundary);
		}
		thisShark = Instantiate(shark, boundary.randomPosition(), Quaternion.identity);
		thisShark.setShark(target);
		Instantiate(jellyfish, boundary.randomPosition(), Quaternion.identity);
		Instantiate(particles, boundary.randomPosition(), Quaternion.identity);
	}

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.S)) //saves data on command, emergency save function
        {
            System.IO.File.WriteAllLines(@"C:\Data Collection\" + nametime + "-data.txt", list);
        }
    }

    void LateUpdate()
    {
        Vector3 position = target.transform.position;
        Quaternion rotation = target.transform.rotation;

        string realtime = System.DateTime.Now.ToString("hh:mm:ss tt");
        string px = position.x.ToString();
        string py = position.y.ToString();
        string pz = position.z.ToString();
        string rx = rotation.eulerAngles.x.ToString();
        string ry = rotation.eulerAngles.y.ToString();
        string rz = rotation.eulerAngles.z.ToString();

        string addtolist = "p" + template.Substring(0, 14) + realtime + template.Substring(0, (15 - realtime.Length)) + px + template.Substring(0, (15 - px.Length)) + py + template.Substring(0, (15 - py.Length)) + pz + template.Substring(0, (15 - pz.Length)) + rx + template.Substring(0, (15 - rx.Length)) + ry + template.Substring(0, (15 - ry.Length)) + rz + template.Substring(0, (15 - rz.Length));
        System.Array.Resize(ref list, list.Length + 1);
        list[list.Length - 1] = addtolist;

        position = thisShark.transform.position;
        rotation = thisShark.transform.rotation;

        realtime = System.DateTime.Now.ToString("hh:mm:ss tt");
        px = position.x.ToString();
        py = position.y.ToString();
        pz = position.z.ToString();
        rx = rotation.eulerAngles.x.ToString();
        ry = rotation.eulerAngles.y.ToString();
        rz = rotation.eulerAngles.z.ToString();

        addtolist = "s" + template.Substring(0, 14) + realtime + template.Substring(0, (15 - realtime.Length)) + px + template.Substring(0, (15 - px.Length)) + py + template.Substring(0, (15 - py.Length)) + pz + template.Substring(0, (15 - pz.Length)) + rx + template.Substring(0, (15 - rx.Length)) + ry + template.Substring(0, (15 - ry.Length)) + rz + template.Substring(0, (15 - rz.Length));
        System.Array.Resize(ref list, list.Length + 1);
        list[list.Length - 1] = addtolist;


        for (int i = 0; i < totalNumOfBubbles; i++)
        {
            position = bubbles[i].transform.position;
            realtime = System.DateTime.Now.ToString("hh:mm:ss tt");
            px = position.x.ToString();
            py = position.y.ToString();
            pz = position.z.ToString();

            addtolist = "b" + template.Substring(0, 14) + realtime + template.Substring(0, (15 - realtime.Length)) + px + template.Substring(0, (15 - px.Length)) + py + template.Substring(0, (15 - py.Length)) + pz + template.Substring(0, (15 - pz.Length));
            System.Array.Resize(ref list, list.Length + 1);
            list[list.Length - 1] = addtolist;
        }
    }

    void OnApplicationQuit() //save function that activates when the game is over
    {
        System.IO.File.WriteAllLines(@"C:\Data Collection\" + nametime + "-data.txt", list);
    }
}
