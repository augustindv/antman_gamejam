using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetCrates : MonoBehaviour {

    public Transform cratesList;

    private List<Transform> crates;
    private List<Vector3> cratesPos;

    // Use this for initialization
    void Start () {
        crates = new List<Transform>();
        cratesPos = new List<Vector3>();

        crates.AddRange(cratesList.GetComponentsInChildren<Transform>());

        foreach (Transform crate in crates)
        {
            cratesPos.Add(crate.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !Player.objectGrabbed)
        {
            for (int i = 0; i < crates.Count; i++)
            {
                crates[i].rotation = new Quaternion(0, 0, 0, 1);
                crates[i].position = cratesPos[i];
            }
        }
    }
}
