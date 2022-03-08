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
    [SerializeField] private float projectileSpawnCooldownTime;
    [SerializeField] private float slerpRatio;

    private bool canShootProjectile = true;
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
        InteractWithCombat();
        RotateWithMovement();
        RestrictMovement();

        Debug.Log("IS MOVING VERT " + isMovingVertically);
        Debug.Log("IS MOVING HORIZ " + isMovingHorizontally);

        UpdateTranslationVelocity();


        //Debug.Log("TRANSLATION VELOCITY: " + previousTranslationVelocity);
    }

    private void UpdateTranslationVelocity()
    {
        previousTranslationVelocity = translationVelocity / Time.deltaTime;
    }

    private void InteractWithCombat()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canShootProjectile)
            {
                canShootProjectile = false;
                StartCoroutine(ShootProjectile());
            }
            
        }
    }

    private IEnumerator ShootProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity); //CAMBIAR LA POSICION (spawnpoint)
        projectileGO.GetComponent<Projectile>()?.SetInstigator(gameObject);
        yield return new WaitForSeconds(projectileSpawnCooldownTime);
        canShootProjectile = true;
    }

    private void RotateWithMovement()
    {
        
        if (!IsMoving() || HasSwitchedOppositeDirections())
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), slerpRatio);
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

    private bool HasSwitchedOppositeDirections() //gone from right to left OR from up to down
    {
        //precondition: player is moving

       if (isMovingHorizontally) //only horizontally OR horizontally and vertically
       {
            return (previousTranslationVelocity.x + translationVelocity.x / Time.deltaTime == 0) || //opposite vectors
                (previousTranslationVelocity.x == 0 && translationVelocity.x / Time.deltaTime != 0); //going horiz, then vert, and then horiz again
       }
       else //only moving vertically
       {
            return (previousTranslationVelocity.y + translationVelocity.y / Time.deltaTime == 0); //opposite vectors
               

       }    
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
            if (transform.eulerAngles.x != 0 && (transform.eulerAngles.x < 360 - rotationMaxAngle || transform.eulerAngles.x <= -rotationMaxAngle) ) 
            {
                //Debug.Log("EULER ANGLES X: " + transform.eulerAngles.x);
                // no roto mas
                return; 
            }
        }
        else if (orientation < 0)
        {
            if (transform.eulerAngles.x != 0 && transform.eulerAngles.x >= rotationMaxAngle) {  return; }
        }

        transform.Rotate(Vector3.right, Time.deltaTime * rotationSpeed * -orientation, Space.Self);
    }


    private void RotateHorizontally(int orientation)
    {
        FixVerticalPosition();

        if (orientation > 0)
        {
            if ( transform.eulerAngles.z != 0 && (transform.eulerAngles.z < 360 - rotationMaxAngle || transform.eulerAngles.z <= -rotationMaxAngle) ) 
            {
                //Debug.Log("ENTRA ACA");
                //Debug.Log("EULER ANGLES Z: " + transform.eulerAngles.z);

                // no roto mas
                return; 
            }
        }
        else if (orientation < 0)
        {
            if (transform.eulerAngles.z != 0 && transform.eulerAngles.z >= rotationMaxAngle) { return; }   
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
        float newXPos = currentPosition.x;
        float newYPos = currentPosition.y;

        if (currentPosition.x >= xRange)
        {
            newXPos = xRange;
        }
        else if (currentPosition.x <= -xRange)
        {
            newXPos = -xRange;
        }

        if (currentPosition.y >= yRange)
        {
            newYPos = yRange;
        }
        else if (currentPosition.y <= -yRange)
        {
            newYPos = -yRange;
        }

        transform.position = new Vector3(newXPos, newYPos, transform.position.z);

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
            //Debug.Log("Player choco con Enemy");
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
            //Debug.Log("Player choco con Enemy");
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



