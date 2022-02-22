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


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.gameObject;
        GameObject projectileInstigator = collisionGO.GetComponent<Projectile>()?.GetInstigator();

        if (collisionGO.CompareTag("Projectile") && projectileInstigator.CompareTag("Player"))
        {
            IncreaseScore();
            //collisionGO.GetComponent<Renderer>().enabled = false; //CORREGIR, DEBO PEDIRLE EL RENDERER AL CHILD 
        }

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
