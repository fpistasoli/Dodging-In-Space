using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform followCamera;
    [SerializeField] private int lives;
    [SerializeField] private float speed;

    private float translationVertical;
    private float translationHorizontal;
    private float translationForward;
    private Vector3 translationVelocity;

    void Start()
    {

    }

    void Update()
    {
        InteractWithMovement();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Player choco con Enemy");
            TakeDamage();
            //TakeDamage(other.GetDamage()); //una vez que este programado este metodo en Enemy.cs
        }

        if (other.CompareTag("Goal"))
        {
            Debug.Log("YOU WIN!");
        }


    }



    private void TakeDamage()
    {
        lives--;
        //lives -= other.GetDamage();
        if (lives == 0)
        {
            Die();
            Debug.Log("GAME OVER");
        }
    }

    private void Die()
    {
        //regresar al menu principal
    }
}
