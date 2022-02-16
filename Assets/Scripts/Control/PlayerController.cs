using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform followCamera;
    [SerializeField] private int lives;
    [SerializeField] private float speed;
    [SerializeField] private float xRange; //positivo siempre
    [SerializeField] private float yRange; //positivo siempre

    private float translationVertical;
    private float translationHorizontal;
    private float translationForward;
    private Vector3 translationVelocity;
    private Rigidbody rbPlayer;

    //Eventos
    public static event Action onGoalReached; //uso un evento para no tener que chequear en el update del HUDController todo el tiempo si llegó o no a la meta


    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    void Update()
    {
        InteractWithMovement();
        RestrictMovement();

    }

    private void RestrictMovement()
    {
        Vector3 currentPosition = transform.position;

        if (currentPosition.x >= xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        else if (currentPosition.x <= -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (currentPosition.y >= yRange)
        {
            transform.position = new Vector3(transform.position.x, yRange, transform.position.z);
        }
        else if (currentPosition.y <= -yRange)
        {
            transform.position = new Vector3(transform.position.x, -yRange, transform.position.z);
        }

    }

    private void InteractWithMovement()
    {
        //player input
        translationVertical = Input.GetAxis("Vertical") * speed;
        translationHorizontal = Input.GetAxis("Horizontal") * speed;

        //update velocity vector (z-coordinate is constantly 'speed' units)
        translationVelocity = new Vector3(translationHorizontal, translationVertical, speed ) * Time.deltaTime;
        transform.Translate(translationVelocity);
    }

    private void OnTriggerEnter(Collider other) //choque con un non static enemy
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Player choco con Enemy");
            int enemyDamage = other.gameObject.GetComponent<EnemyController>().GetDamage();
            TakeDamage(enemyDamage);
        }

        if (other.CompareTag("Goal"))
        {
            onGoalReached?.Invoke(); //llamo al evento "onGoalReached"
        }

    }

    private void OnCollisionEnter(Collision collision) //choque con un static enemy
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player choco con Enemy");
            int enemyDamage = collision.gameObject.GetComponent<EnemyController>().GetDamage();
            TakeDamage(enemyDamage);
        }
    }



    private void TakeDamage(int damage)
    {
        lives -= damage;
        if (lives <= 0)
        {
            lives = 0;
            //Die();
        }
    }

    private void Die()
    {
      
    }

    public void Heal(int boost)
    {
        lives += boost;
    }

    public int GetLives()
    {
        return lives;
    }

    public float GetXRange()
    {
        return xRange;
    }

    public float GetYRange()
    {
        return yRange;
    }



}



