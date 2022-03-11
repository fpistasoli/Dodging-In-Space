using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonStaticEnemy : EnemyController
{
    [SerializeField] float chaseSpeed;
    [SerializeField] float flySpeed;
    private Vector3 playerDirection;
    private float distancePlayer;
    private Rigidbody enemyRb;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = gameObject.transform.GetComponent<Rigidbody>();
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
    }

    protected override void CollisionHandler(Collision collision)
    {
        //Debug.Log("DISPARE A UN DYNAMIC ENEMY");
  
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("PlayerCollision");
            explosionParticle.Play();
            StartCoroutine(Destroy());
         
        }

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
}
