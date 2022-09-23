using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Attributes")]
    public static Turret instance;
    public Transform Shootpoint;
    public GameObject bullet;

    [Header("Turret properties")]
    public int lives = 5;
    public int CurrentLives = 5;
    public float ViewDistance;
    public float FireRate;
    public float force;
    public float speed;
    public bool isChildTurret = false;

    private float nextTimeToFire = 0;
    private Transform Target;
    private Vector3 CloseEnemyRef;
    private Vector2 Direction;
    private bool Detected = false;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Live config
        if(CurrentLives <= 0)
        {
            CurrentLives = 0;
            Destroy(gameObject);
            if (!isChildTurret)
            {
                Time.timeScale = 0;
                //SceneManagement.instance.ChangeToMainScene();
                return;
            }
        }

        // Enemy detection
        if(Target != null)
        {
            Vector2 targetPos = Target.position;
            Direction = targetPos - (Vector2)transform.position;
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, ViewDistance);
            if (rayInfo)
            {
                if(rayInfo.collider.gameObject.tag == "Unit")
                {
                    if(Detected == false)
                    {
                        Detected = true;
                    }
                }
                else
                {
                    if(Detected == true)
                    {
                        Detected = false;
                    }
                }
                if (Detected)
                {
                    //Gun.transform.up = Direction;
                    if(Time.time > nextTimeToFire)
                    {
                        nextTimeToFire = Time.time + 1 / FireRate;
                        shoot();
                    }
                }
            }
        }
        
    }

    void shoot()
    {
        //LevelManager.instance.CalculateCriticalFactor();
        GameObject BulletIns = Instantiate(bullet, Shootpoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * force * speed);
        //LevelManager.instance.Damage = LevelManager.instance.oldDamage;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw View Distance
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ViewDistance);
        // Draw View Draw Collision Area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, .25f);
        // Draw View Draw Enemy distance from player
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, CloseEnemyRef);
    }

    void UpdateTarget()
    {
        // Set a reference to the list of enemies in the game
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Unit");
        float shortDistance = Mathf.Infinity;
        GameObject closeEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            // Get the distance from the enemy position and the turret positon
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Destroy baby turret
            if (isChildTurret && distance <= 0.35f)
            {
                Destroy(gameObject);
            }

            // Check what enemy is closer and set it as the principal enemy
            if (distance < shortDistance)
            {
                shortDistance = distance;
                closeEnemy = enemy;
                CloseEnemyRef = enemy.transform.position;
            }

            // Check that the enemy that is closer to the turret  became de main target
            if(closeEnemy != null && shortDistance <= ViewDistance)
            {
                Target = closeEnemy.transform;
            }
            else
            {
                Target = null;
            }
        }
    }
}
