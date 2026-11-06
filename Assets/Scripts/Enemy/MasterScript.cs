using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//Make all of these individual scripts into one big enemy script, that passes arguments based on bools
public class MasterScript : MonoBehaviour
{
    //private GameObject Player;
    public int Health;
    [SerializeField] private GameObject LaserPrefab;
    [SerializeField] private GameObject LaserPoint;
    [SerializeField] private int cooldown;
    private bool onCoolDown;
    [SerializeField] private float TriggerDistance;
    //[SerializeField] private GameObject Canvas;
    [SerializeField] private Sprite TargetFighterStill;
    //[SerializeField] private Texture TargetFighterStillTexture;
    public GameObject TargetedOverlay;
    public float speed;
    public GameObject EnemyForward;
    public bool Buzzer, Engineer, Bomber;
    public GameObject Camera;
    public bool seeking;
    //private GameObject Player;
    //private PlayerShipMovement PlayerShipMovement;
    // public GameObject Spinner;
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("EnemyCamera");
        // Player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        //Below code is commented to stop movement for testing
        //Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, transform.position, Time.deltaTime * 20); 
        if (Buzzer)
        {
            //if the distance to the player is less than minimumDistance, rotate towards player, shoot, run away, which means do above while reloading  
            //Fake being erratic, roll a d6 to pick a direction (up, down, left, right, forward, backward)
            //roll between 50 and 100, shoot a raycast for that
            //add distance to transform.forward position to check the end of the ray 
            // move that many spaces forward, repeat
            //if raycast hits something, subtract from that, movetowards 


            /* transform.position = Vector3.MoveTowards(transform.position, EnemyForward.transform.position, speed * 2);

             Vector3 newRotate = Vector3.RotateTowards(transform.forward, (PlayerShipMovement.instance.gameObject.transform.position - transform.position), 1f, 0.0f); // Angle towards the player
             transform.rotation = Quaternion.LookRotation(newRotate);
             if ((Vector3.Distance(transform.position, PlayerShipMovement.instance.gameObject.transform.position)) < TriggerDistance) // If player is within firing distance, shoot
             {
                 if (!onCoolDown)
                 {
                     if (!PlayerShipMovement.instance.dead) // If player is alive
                     {

                       //  Shoot();
                     }
                 }
             }*/
            //transform.Rotate(0, 0, Time.deltaTime, Space.Self);
            if (!seeking)
            {
                Seek();
            }

            if (PlayerShipMovement.instance != null)
            {
                if (PlayerShipMovement.instance.TargetedFighter == gameObject)
                {
                    if (!CanvasUIHolder.instance.TargetFighterStill.activeSelf)
                    {
                        CanvasUIHolder.instance.TargetFighterStill.SetActive(true);
                    }
                    Camera.SetActive(true);
                    //Camera.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
                    //CanvasUIHolder.instance.TargetFighterStill.GetComponent<RawImage>().texture = TargetFighterStillTexture;
                    //CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = TargetFighterStill;
                    float dist = Mathf.Round(Vector3.Distance(PlayerShipMovement.instance.gameObject.transform.position, transform.position)) * 10f / 10f;

                    CanvasUIHolder.instance.TargetFighterDistance.text = dist.ToString("0.0");

                    if (!TargetedOverlay.activeSelf)
                    {
                        TargetedOverlay.SetActive(true);
                    }
                }
                else if (PlayerShipMovement.instance.TargetedFighter != gameObject)
                {
                    if (TargetedOverlay.activeSelf)
                    {
                        TargetedOverlay.SetActive(false);
                    }
                }
            }

        }
        //if (Engineer)
        //{

        //}

    }
    public void TakeDamage(int Damage)
    {
        //Debug.Log(Damage);
        Health -= Damage;
        //Debug.Log(Health);
        if (PlayerShipMovement.instance.TargetedFighter == gameObject)
        {
            CanvasUIHolder.instance.TargetFighterHealth.text = (Health / 2).ToString();
            //Debug.Log(Health);

        }
        if (Health <= 0)
        {
            if (PlayerShipMovement.instance.TargetedFighter == gameObject) // Go to PlayerShipMovement.cs and check if TargetedFighter is an object
            {
                DeTarget(); // Destroy enemy sprites and reset targeting
            }
            Destroy(gameObject);
        }
    }
    public void Shoot()
    {

        GameObject Laser1 = Instantiate(LaserPrefab, LaserPoint.transform.position, Quaternion.Euler(90, transform.rotation.y, 0));
        Laser1.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
        onCoolDown = true;
        StartCoroutine(Wait(cooldown));
    }
    private IEnumerator Wait(int Seconds)
    {
        yield return new WaitForSecondsRealtime(Seconds); // Wait for a number of seconds set in Engine
        onCoolDown = false; // Resets cooldown and allow enemy to fire again
    }
    void Seek()
    {
        seeking = true;
        int Rotation = Random.Range(0, 5);
        switch (Rotation)
        {
            case 0:
                //dont rotate
                break;
            case 1:
                //move up
                transform.Rotate(new Vector3(-90, 0, 0), Space.World);
                break;
            case 2:
                //move down
                transform.Rotate(new Vector3(90, 0, 0), Space.World);
                break;
            case 3:
                //move left
                transform.Rotate(new Vector3(0, -90, 0), Space.World);
                break;
            case 4:
                //move right
                transform.Rotate(new Vector3(0, 90, 0), Space.World);
                break;
            case 5:
                //move backward
                transform.Rotate(new Vector3(0, 180, 0), Space.World);
                break;
        }
        RaycastHit hit;
        Ray myRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        float maxDist = Random.Range(0, 190);
        if (Physics.Raycast(myRay, out hit, maxDist))
        {
            Debug.Log(hit.collider.gameObject.name);

        }
        else
        {
            Vector3 newPos = myRay.origin + myRay.direction;
            Debug.Log(newPos);
            //transform.position= Vector3.MoveTowards(transform.position, )
        }
    }
    public void DeTarget() // Handles sprite deletion and resetting targeting
    {
        //Debug.Log("What");



        //CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = null;
        //CanvasUIHolder.instance.TargetFighterStill.GetComponent<RawImage>().texture = null;
        Camera.SetActive(false);
        CanvasUIHolder.instance.TargetFighterHealth.text = null;
        CanvasUIHolder.instance.TargetFighterDistance.text = null;
        PlayerShipMovement.instance.TargetedFighter = null;
        PlayerShipMovement.instance.Targeted = false;

    }
}
