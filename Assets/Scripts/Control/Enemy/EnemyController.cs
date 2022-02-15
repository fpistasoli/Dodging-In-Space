using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float visibilityDistance;
    [SerializeField] protected int damage;
    private float xRange;
    private float yRange;
    private float goalPos;
    protected Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        xRange = GameObject.FindWithTag("RightBound").transform.position.x;
        yRange = GameObject.FindWithTag("UpperBound").transform.position.y;
        goalPos = GameObject.FindWithTag("Goal").transform.position.z;

        gameObject.transform.position = RandomPos();
    }

    // Update is called once per frame
    void Update()
    {
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

    public int GetDamage()
    {
        return damage;
    }
}