using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float xRange;
    [SerializeField] float yRange;
    [SerializeField] float goalPos;
    [SerializeField] float force;
    [SerializeField] float speed;
    [SerializeField] float visibilityDistance;
    private Vector3 playerPos;
    private Vector3 playerDirection; 
    private float distancePlayer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = RandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.Find("Player").transform.position;
        playerDirection = (playerPos - gameObject.transform.position);
        distancePlayer = Vector3.Distance(playerPos, gameObject.transform.position);

        if (distancePlayer < visibilityDistance)
        {
            ChasePlayer();
        }
        else
        {
            Fly();
        }

        if (gameObject.transform.position.z < playerPos.z)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 RandomPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), goalPos); 
    }

    private void ChasePlayer()
    {
        gameObject.transform.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + playerDirection.normalized * speed * Time.deltaTime);
    }

    private void Fly()
    {
        gameObject.transform.GetComponent<Rigidbody>().AddForce(playerDirection.normalized * force, ForceMode.Acceleration);
    }

}
