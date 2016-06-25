using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

    public Sprite flagClosed;
    public Sprite flagOpen;
    public bool checkpointActive;

    private SpriteRenderer mySpriteRenderer;

	// Use this for initialization
	void Start () {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            mySpriteRenderer.sprite = flagOpen;
            checkpointActive = true;
        }
    }
}
