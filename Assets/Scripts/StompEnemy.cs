using UnityEngine;
using System.Collections;

public class StompEnemy : MonoBehaviour {

    public GameObject deathSplosionYellow;
    public GameObject deathSplosionGreen;

    public float bounceForce;

    private Rigidbody2D thePlayerRigidBody;

    // Use this for initialization
    void Start () {
        thePlayerRigidBody = transform.parent.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemySpider")
        {
            Destroy(other.gameObject);

            Instantiate(deathSplosionYellow, other.transform.position, other.transform.rotation);
            thePlayerRigidBody.velocity = new Vector3(thePlayerRigidBody.velocity.x, bounceForce, 0f);
        }
        if (other.tag == "EnemyGreenWiggle")
        {
            Destroy(other.gameObject);

            Instantiate(deathSplosionGreen, other.transform.position, other.transform.rotation);
            thePlayerRigidBody.velocity = new Vector3(thePlayerRigidBody.velocity.x, bounceForce, 0f);

        }
    }
}
