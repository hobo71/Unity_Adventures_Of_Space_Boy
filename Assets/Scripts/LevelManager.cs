using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public float waitToRespawnAtDeath;
    public float waitToRespawnAfterDeath;
    public PlayerController thePlayer;

    public GameObject deathSplosion;

	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<PlayerController>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Respawn()
    {
        StartCoroutine("RespawnCo");
    }

    // Co-routine used to delay the respawn of player
    public IEnumerator RespawnCo()
    {
        thePlayer.gameObject.SetActive(false);

        // Add the deathsplosion particles to the players last location and using the players rotation
        Instantiate(deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);
        
        // Wait some seconds after death
        yield return new WaitForSeconds(waitToRespawnAtDeath);

        // Move to respawn position
        thePlayer.transform.position = thePlayer.respawnPosition;

        // Wait while camera gets in position
        yield return new WaitForSeconds(waitToRespawnAfterDeath);

        // Activate the player
        thePlayer.gameObject.SetActive(true);
        
    }
}
