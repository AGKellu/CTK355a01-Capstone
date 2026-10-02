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
   // public GameObject Spinner;

    // Update is called once per frame
    void Update()
    {
        //Below code is commented to stop it from moving during testing
        transform.position += transform.forward * .01f;
        Vector3 newRotate = Vector3.RotateTowards(transform.forward, (PlayerShipMovement.instance.gameObject.transform.position - transform.position), 1f, 0.0f); // Angle towards the player
        transform.rotation = Quaternion.LookRotation(newRotate);
        //        float distanceToPlayer = Vector3.distance(transform.position, Player.transform.position);
        if ((Vector3.Distance(transform.position, PlayerShipMovement.instance.gameObject.transform.position)) < TriggerDistance) // If player is within firing distance, shoot
        {
            if (!onCoolDown)
            {
                if (!PlayerShipMovement.instance.dead) // If player is alive
                {

                    Shoot();
                }
            }
        }
        //Spinner.transform.RotateAround(LaserPoint.transform.position, Vector3.up, 20 * Time.deltaTime);
        if (PlayerShipMovement.instance.TargetedFighter == gameObject)
        {
            if (!CanvasUIHolder.instance.TargetFighterStill.activeSelf)
            {
                CanvasUIHolder.instance.TargetFighterStill.SetActive(true);
            }
            CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = TargetFighterStill;
            //CanvasUIHolder.instance.TargetFighterStill.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
            if (!TargetedOverlay.activeSelf)
            {
                TargetedOverlay.SetActive(true);
            }
           // Debug.Log(Health);
           // Debug.Log(Vector3.Distance(gameObject.transform.position, PlayerShipMovement.instance.gameObject.transform.position).ToString("F2"));
        }
        else if (PlayerShipMovement.instance.TargetedFighter != gameObject)
        {
           // Debug.Log("Why is this still added");
            if (TargetedOverlay.activeSelf)
            {
                TargetedOverlay.SetActive(false);
            }
        }
    }
    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            if (PlayerShipMovement.instance.TargetedFighter = gameObject) // Go to PlayerShipMovement.cs and check if TargetedFighter is an object
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
        // GameObject Laser2 = Instantiate(LaserPrefab, LaserPoint2.transform.position, Quaternion.Euler(90, transform.rotation.y, 0));
        //Laser2.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
    }
    private IEnumerator Wait(int Seconds)
    {
        yield return new WaitForSecondsRealtime(Seconds); // Wait for a number of seconds set in Engine
        onCoolDown = false; // Resets cooldown and allow enemy to fire again
    }
    public void DeTarget() // Handles sprite deletion and resetting targeting
    {
        PlayerShipMovement.instance.TargetedFighter = null;
        PlayerShipMovement.instance.Targeted = false;
        if (CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite == TargetFighterStill)
        {
            CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = null;
        }
        
    }
}
