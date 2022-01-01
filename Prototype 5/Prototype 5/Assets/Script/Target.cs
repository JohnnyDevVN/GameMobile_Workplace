using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12f;
    private float maxSpeed = 16f;
    private float maxTorque = 10f;
    private float xRange =4f;
    private float ySpawnPos = -6f;
    public bool isBadObject;
    public int pointValue;

    public ParticleSystem explosionParticle;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(),RandomTorque(),RandomTorque(), ForceMode.Impulse);

        transform.position = RandomSpawnPos();
    }

    void OnMouseDown()
    {   
        if(gameManager.isGameActive)
        {
            gameManager.Sound.PlayOneShot(gameManager.shootSound,1f);
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
            if(gameManager.isGameActive&&!isBadObject)
            {
                gameManager.Sound.PlayOneShot(gameManager.dieSound,1f);
                //triggerDieSound=false;
            }
            gameManager.GameOver(isBadObject);
        }    
    }
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed,maxSpeed);
    }
    float RandomTorque()
    {
        return Random.Range(-maxTorque,maxTorque);
    }
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange,xRange), ySpawnPos);
    }
}

