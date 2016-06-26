using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {

    public float objectLifeTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        objectLifeTime = objectLifeTime - Time.deltaTime;

        if (objectLifeTime <= 0f) 
        {
            // Destroy the object which this script is attached to
            Destroy(gameObject);
        }
	}
}
