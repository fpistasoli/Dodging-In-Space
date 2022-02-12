using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public enum pickupType { healthBoost }; //se le puede ir agregando tipos de pickups mas adelante

    [SerializeField] private pickupType type;
    [SerializeField] private int boost;

    private GameObject player;
   
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    void Update()
    {
         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().Heal(boost);
            Destroy(gameObject);
        } 
    }

}
