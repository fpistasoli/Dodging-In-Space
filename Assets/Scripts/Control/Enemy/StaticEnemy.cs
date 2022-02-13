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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
