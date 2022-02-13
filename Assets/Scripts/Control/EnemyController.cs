using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float chaseSpeed;
    [SerializeField] float flySpeed;
    [SerializeField] float visibilityDistance;
    [SerializeField] int damage; 

    private float xRange;
    private float yRange;
    private float goalPos;
    private Vector3 playerPos;
    private Vector3 playerDirection; 
    private float distancePlayer;
    private Rigidbody enemyRb; 

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = gameObject.transform.GetComponent<Rigidbody>(); 

        xRange = GameObject.FindWithTag("RightBound").transform.position.x;
        yRange = GameObject.FindWithTag("UpperBound").transform.position.y;
        goalPos = GameObject.FindWithTag("Goal").transform.position.z;

        gameObject.transform.position = RandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position; 
        distancePlayer = Vector3.Distance(playerPos, gameObject.transform.position);

        // enemy esta cerca y adelante del player 
        if (distancePlayer < visibilityDistance && gameObject.transform.position.z >= playerPos.z)
        {
            ChasePlayer();
        }
        else
        {
            Fly(); 
        }

        if (gameObject.transform.position.z < playerPos.z - visibilityDistance)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), 
            Random.Range(playerPos.z + visibilityDistance, goalPos)); 
    }

    private void ChasePlayer()
    {
        playerDirection = (playerPos - gameObject.transform.position);
        enemyRb.MovePosition(gameObject.transform.position + playerDirection.normalized * chaseSpeed * Time.deltaTime);
    }

    private void Fly()
    {
        gameObject.transform.Translate(Vector3.back * flySpeed * Time.deltaTime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject); 
        }
    }

    public int GetDamage()
    {
        return damage;
    }
}
