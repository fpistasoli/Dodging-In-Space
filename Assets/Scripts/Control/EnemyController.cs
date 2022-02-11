using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float xRange;
    [SerializeField] float yRange;
    [SerializeField] float goalPos;
    [SerializeField] float force;
    [SerializeField] float playerStartPos;
    private Vector3 playerPos; 
 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = RandomPos();
        playerPos = GameObject.Find("Player").transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();

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
        Vector3 playerDirection = (playerPos - gameObject.transform.position).normalized;
        gameObject.transform.GetComponent<Rigidbody>().AddForce(playerDirection * force, ForceMode.Acceleration);
    }

}
