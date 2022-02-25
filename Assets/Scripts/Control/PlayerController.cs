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
    [SerializeField] private GameObject projectileSpawnPoint;


    private bool isMovingHorizontally = false;
    private bool isMovingVertically = false;
    private float translationVertical;
    private float translationHorizontal;
    private float translationForward;
    private Vector3 translationVelocity;
    private Vector3 previousTranslationVelocity;

    private Rigidbody rbPlayer;

    private float xFixedPos = 0;
    private float yFixedPos = 0;

    //private float activeRoll, activePitch, activeYaw;

    //Eventos
    public static event Action onGoalReached; //uso un evento para no tener que chequear en el update del HUDController todo el tiempo si llegó o no a la meta


    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    void Update()
    {
        InteractWithMovement();
        RotateWithMovement();
        RestrictMovement();
        InteractWithCombat();
        UpdateTranslationVelocity();

        Debug.Log("TRANSLATION VELOCITY: " + translationVelocity/Time.deltaTime);


        //Debug.Log("HORIZONTAL MOVEMENT: " + isMovingHorizontally);
        //Debug.Log("VERTICAL MOVEMENT: " + isMovingVertically);

        //Debug.Log("X FIXED POS: " + xFixedPos);
        //Debug.Log("Y FIXED POS: " + yFixedPos);
    }

    private void UpdateTranslationVelocity()
    {
        previousTranslationVelocity = translationVelocity / Time.deltaTime;
    }

    private void InteractWithCombat()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity); //CAMBIAR LA POSICION (spawnpoint)
        projectileGO.GetComponent<Projectile>()?.SetInstigator(gameObject);
    }

    private void RotateWithMovement()
    {
       
        if (!IsMoving() || HasSwitchedDirection())
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
        
        if (translationVelocity.y < 0) //moving down
        {
            RotateVertically(-1);
        }
        else if (translationVelocity.y > 0) //moving up
        {
            RotateVertically(1);
        }

    }

    private bool HasSwitchedDirection()
    {
        return ((previousTranslationVelocity.x != translationVelocity.x / Time.deltaTime) || (previousTranslationVelocity.y != translationVelocity.y / Time.deltaTime) );
    }

    private bool IsMoving()
    {
        return (isMovingHorizontally || isMovingVertically);
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
        translationVertical = Input.GetAxisRaw("Vertical") * speed;
        translationHorizontal = Input.GetAxisRaw("Horizontal") * speed;

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


  /* OTRA POSIBLE IMPLEMENTACION DE MOVE + ROTATE
  private void MoveAndRotate()
  {

      transform.position += transform.forward * speed * Time.deltaTime;

      activePitch = Input.GetAxisRaw("Vertical") * rotationSpeed * Time.deltaTime;
      activeRoll = Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;

      transform.Rotate(activePitch, 0, -activeRoll, Space.Self);

  }
  */





}



