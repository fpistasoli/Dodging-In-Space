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
}
