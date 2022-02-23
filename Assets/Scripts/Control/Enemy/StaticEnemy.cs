using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : EnemyController
{

    [SerializeField] private float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected override void CollisionHandler(Collision collision)
    {
        //base.CollisionHandler(collision);
       
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Destroy());
        }

    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
        //animation for static enemy destruction
    }


}
