using UnityEngine;

public class AmmunitionMoveScript : MonoBehaviour
{
    [SerializeField] private float speed;
    public string OwnerFaction;
    //private Rigidbody AmmoRB;
    public Vector3 forward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AmmoRB = gameObject.GetComponent<Rigidbody>();
        transform.parent = null;
        Destroy(gameObject, 3);
        //transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //AmmoRB.AddRelativeForce(Vector3.forward * (5 * Time.deltaTime ), ForceMode.Acceleration);
        transform.position += forward * speed * Time.deltaTime;
        //speed = 2.5f;
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Enemy") && coll.gameObject.name.Contains("Buzzer"))
        {
            if (OwnerFaction == coll.gameObject.tag)
            {

            }
            else
            {
                coll.gameObject.GetComponent<BuzzerScript>().TakeDamage(1);
            
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

            }
        }
    }
}
