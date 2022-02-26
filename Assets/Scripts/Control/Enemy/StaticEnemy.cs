using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : EnemyController
{
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
            explosionParticle.Play();
            StartCoroutine(Destroy());
        }

    }

 /*   private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }*/


}
