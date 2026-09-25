using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//Make all of these individual scripts into one big enemy script, that passes arguments based on bools
public class BuzzerScript : MonoBehaviour
{
    private GameObject Player;
    public int Health;
    [SerializeField] private GameObject LaserPrefab;
    [SerializeField] private GameObject LaserPoint;
    [SerializeField] private int cooldown;
    private bool onCoolDown;
    [SerializeField] private float TriggerDistance;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private Sprite TargetFighterStill;
    //[SerializeField] private GameObject LaserPoint2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * .01f;
        //transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, .01f);
        Vector3 newRotate = Vector3.RotateTowards(transform.forward, (Player.transform.position - transform.position), 1f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newRotate);
        //        float distanceToPlayer = Vector3.distance(transform.position, Player.transform.position);
        if ((Vector3.Distance(transform.position, Player.transform.position)) < TriggerDistance)
        {
            if (!onCoolDown)
            {
                if (!PlayerShipMovement.instance.dead)
                {

                    Shoot();
                }
                // Debug.Log("Should shoot");
            }
        }
        if (PlayerShipMovement.instance.TargetedFighter = gameObject)
        {
            if (!CanvasUIHolder.instance.TargetFighterStill.activeSelf)
            {
                CanvasUIHolder.instance.TargetFighterStill.SetActive(true);
            }
            CanvasUIHolder.instance.TargetFighterStill.GetComponent<Image>().sprite = TargetFighterStill;
            CanvasUIHolder.instance.TargetFighterStill.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
            //Canvas.GetComponent<CanvasUIHolder>().TargetedFighterStill.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
            //TargetedFighterStill.transform.rotation.z = transform.rotation.z;
           // PlayerShipMovement.instance.TargetedFighterStill.transform.rotation.z = transform.rotation.z;
        }
    }
    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
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
        yield return new WaitForSecondsRealtime(Seconds);
        onCoolDown = false;
    }
}
