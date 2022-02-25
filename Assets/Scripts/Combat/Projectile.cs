using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float visibilityDistance;

    private Vector3 startPoint;
    private GameObject instigator; //game object that shoots the projectile

    //private Rigidbody rb;


    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
    }

    private void Shoot()
    {
        //rb.AddForce(transform.forward * speed, ForceMode.Force);

        transform.Translate(transform.forward * speed * Time.deltaTime);


    }

    private void Update()
    {
        DestroyIfOutOfSight();
        //Debug.Log(rb.velocity.magnitude);

    }

    private void DestroyIfOutOfSight()
    {
        if ((transform.position - startPoint).magnitude >= visibilityDistance)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Shoot();
    }

    public void SetInstigator(GameObject instigator)
    {
        this.instigator = instigator;
    }

    public GameObject GetInstigator()
    {
        return instigator;
    }








}
