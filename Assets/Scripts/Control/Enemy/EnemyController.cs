using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float visibilityDistance;
    [SerializeField] protected int damage;
    [SerializeField] protected ParticleSystem explosionParticle;
    [SerializeField] protected float destroyTime;
    [SerializeField] float offset;
    private float xRange;
    private float yRange;
    private float goalPos;
    protected Vector3 playerPos;
    protected bool gameOver; 
    
    // Start is called before the first frame update
    void Start()
    {
        xRange = GameObject.FindWithTag("Player").GetComponent<PlayerController>().GetXRange();
        yRange = GameObject.FindWithTag("Player").GetComponent<PlayerController>().GetYRange();

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
        return new Vector3(Random.Range(-xRange + offset, xRange - offset), 
            Random.Range(-yRange + offset, yRange - offset), 
            Random.Range(playerPos.z + offset, goalPos));
    }

    public int GetDamage()
    {
        return damage;
    }

    protected void IncreaseScore()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().AddPoints(damage); 
    }


    private void OnCollisionEnter(Collision collision)
    {
        CollisionHandler(collision);
    }

    protected virtual void CollisionHandler(Collision collision)
    {
        GameObject collisionGO = collision.gameObject;
        GameObject projectileInstigator = collisionGO.GetComponent<Projectile>()?.GetInstigator();

        gameOver = GameObject.Find("GameManager").GetComponent<GameManager>().isGameOver;

        if (collisionGO.CompareTag("Projectile") && projectileInstigator.CompareTag("Player") && !gameOver)
        {
            FindObjectOfType<AudioManager>().Play("EnemyExplosion");
            explosionParticle.Play(); 

            IncreaseScore();
            Debug.Log("SUMO PUNTOS");
            Destroy(collisionGO);
            StartCoroutine(Destroy());
        }
    }

    protected IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
