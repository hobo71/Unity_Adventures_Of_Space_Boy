using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public float waitToRespawnAtDeath;
    public float waitToRespawnAfterDeath;

    public PlayerController thePlayer;

    public int coinCount;
    public Text coinText;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public int maxHealth;
    public int healthCount;

    public bool invincible;

    private bool isRespawning;

    // Array to contain objects that can be reset once player dies
    public ResetOnRespawn[] objectsToReset;


    public GameObject deathSplosion;

	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<PlayerController>();
        coinText.text = ": " + coinCount;

        healthCount = maxHealth;

        objectsToReset = FindObjectsOfType<ResetOnRespawn>();
	}
	
	// Update is called once per frame
	void Update () {
        if (healthCount <= 0 && !isRespawning)
        {
            Respawn();
            isRespawning = true;
        }
    }

    public void Respawn()
    {
        StartCoroutine("RespawnCo");
    }

    // Co-routine used to delay the respawn of player upon death
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
        
        // Set health back to max
        healthCount = maxHealth;
        isRespawning = false;
        UpdateHeartMeter();

        // If dead coins reset to 0
        coinCount = 0;
        coinText.text = ": " + coinCount;

        // Activate the player
        thePlayer.gameObject.SetActive(true);

        // Respawn items/enemies
        for (int i = 0; i < objectsToReset.Length; i++)
        {
            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObject();
        }
        
    }

    public void AddCoins(int coinsToAdd)
    {
        coinCount += coinsToAdd;
        coinText.text = ": " + coinCount;
    }

    public void HurtPlayer(int damageToTake)
    {
        if (!invincible)
        {
            healthCount -= damageToTake;
            UpdateHeartMeter();

            thePlayer.Knockback();
        }

    }

    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;
            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                return;
            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                return;
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                return;
            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
        }
    }
}
