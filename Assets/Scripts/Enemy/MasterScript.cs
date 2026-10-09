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
    public GameObject TargetedOverlay;
    public float speed;
    public GameObject EnemyForward;
    public bool Buzzer, Engineer, Bomber;
    //private GameObject Player;
    //private PlayerShipMovement PlayerShipMovement;
    // public GameObject Spinner;
    void Start()
    {
        // Player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        //Below code is commented to stop movement for testing 
        if (Buzzer)
        {
            transform.position = Vector3.MoveTowards(transform.position, EnemyForward.transform.position, speed * 2);

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
            }
            if (PlayerShipMovement.instance.TargetedFighter == gameObject)
            {
                if (!CanvasUIHolder.instance.TargetFighterStill.activeSelf)
                {
                    CanvasUIHolder.instance.TargetFighterStill.SetActive(true);
                }
                CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = TargetFighterStill;
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
            CanvasUIHolder.instance.TargetFighterHealth.text = (Health/2).ToString();
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
    public void DeTarget() // Handles sprite deletion and resetting targeting
    {
        //Debug.Log("What");


        //if (CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite == TargetFighterStill)
        //{
        CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = null;
        //}
        CanvasUIHolder.instance.TargetFighterHealth.text = null;
        CanvasUIHolder.instance.TargetFighterDistance.text = null;
        PlayerShipMovement.instance.TargetedFighter = null;
        PlayerShipMovement.instance.Targeted = false;

    }
}
