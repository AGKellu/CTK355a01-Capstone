using UnityEngine;

public class AmmunitionMoveScript : MonoBehaviour
{
    [SerializeField] private float speed;
    public string OwnerFaction;
    //private Rigidbody AmmoRB;
    public Vector3 forward;
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AmmoRB = gameObject.GetComponent<Rigidbody>();
        transform.parent = null;
        if (target != null)
        {
           // Debug.Log(target.name);
        }
        else 
        {
            
        Destroy(gameObject, 5);
        }
        //transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //AmmoRB.AddRelativeForce(Vector3.forward * (5 * Time.deltaTime ), ForceMode.Acceleration);
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, (target.transform.position- transform.position), speed * Time.deltaTime, 0.0f);
            //Vector3 newDirection = Vector3.RotateTowards(transform.forward, (target.transform.position - transform.position), 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {

            transform.position += forward * speed * Time.deltaTime;
        }
        
        //speed = 2.5f;
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            if (OwnerFaction == coll.gameObject.tag)
            {

            }
            else
            {
                if (target != null)
                {
                    coll.gameObject.GetComponent<MasterScript>().TakeDamage(2);
                    Destroy(gameObject);
                }
                else
                {
                    
                coll.gameObject.GetComponent<MasterScript>().TakeDamage(1);
                Destroy(gameObject);
                }
            
            }
            //Destroy(coll.gameObject);
            //Debug.Log("Shot enemy");
        }
        else if (coll.gameObject.CompareTag("Player"))
        {
            if (OwnerFaction == coll.gameObject.tag)
            {

            }
            else
            {
                coll.gameObject.GetComponent<PlayerShipMovement>().TakeDamage(1);
                Destroy(gameObject);

            }
        }
    }
    void OnDestroy()
    {
        //if (PlayerShipMovement.instance.MissileLoaded)
        //{
          //  PlayerShipMovement.instance.MissileLoaded  = 
        //}
    }
}
