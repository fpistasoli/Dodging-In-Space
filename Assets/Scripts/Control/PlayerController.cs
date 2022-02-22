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
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationMaxAngle; //positivo siempre
    [SerializeField] private GameObject projectilePrefab;

    private bool isMovingHorizontally = false;
    private bool isMovingVertically = false;
    private float translationVertical;
    private float translationHorizontal;
    private float translationForward;
    private Vector3 translationVelocity;
    private Rigidbody rbPlayer;

    private float xFixedPos = 0;
    private float yFixedPos = 0;



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
        RotateWithMovement();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectileGO.GetComponent<Projectile>()?.SetInstigator(gameObject);
    }

    private void RotateWithMovement()
    {
      
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            return; 
        } 

        if (translationVelocity.x < 0) //moving left
        {
            RotateHorizontally(-1);
        }
        else if (translationVelocity.x > 0) //moving right
        {
            RotateHorizontally(1);
        }
        else if (translationVelocity.y < 0) //moving down
        {
            RotateVertically(-1);
        }
        else if (translationVelocity.y > 0) //moving up
        {
            RotateVertically(1);
        }

    }

    private void RotateVertically(int orientation)
    {
        FixHorizontalPosition();

        if (orientation > 0)
        {
            if (transform.rotation.x != 0 && (transform.eulerAngles.x < 360 - rotationMaxAngle)) 
            {
                // no roto mas
                return; 
            }
        }
        else if (orientation < 0)
        {
            if (transform.eulerAngles.x >= rotationMaxAngle) { return; }
        }

        transform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed * -orientation, Space.Self);
    }


    private void RotateHorizontally(int orientation)
    {
        FixVerticalPosition();

        if (orientation > 0)
        {
            if ( transform.rotation.z != 0 && (transform.eulerAngles.z < 360 - rotationMaxAngle) ) 
            {
                // no roto mas
                return; 
            }
        }
        else if (orientation < 0)
        {
            if (transform.eulerAngles.z >= rotationMaxAngle) { return; }   
        }

        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed * -orientation, Space.Self);
    }

    private void FixVerticalPosition()
    {
        transform.position = new Vector3(transform.position.x, yFixedPos, transform.position.z); //para que no rote en el eje "y"
    }
    private void FixHorizontalPosition()
    {
        transform.position = new Vector3(xFixedPos, transform.position.y, transform.position.z); // para que no rote en el eje "x"
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

        //disable potential diagonal movement
        if (translationVertical != 0 && translationHorizontal != 0)
        {
            translationVertical = 0; //or Horizontal (either way)
        }

        //update velocity vector (z-coordinate is constantly 'speed' units)
        translationVelocity = new Vector3(translationHorizontal, translationVertical, speed ) * Time.deltaTime;
        transform.Translate(translationVelocity);

        if (translationVertical != 0)
        {
            isMovingVertically = true;
            yFixedPos = transform.position.y;
        }
        else
        {
            isMovingVertically = false;
        }

        if (translationHorizontal != 0)
        {
            isMovingHorizontally = true;
            xFixedPos = transform.position.x; 
        }
        else
        {
            isMovingHorizontally = false;
        }

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



