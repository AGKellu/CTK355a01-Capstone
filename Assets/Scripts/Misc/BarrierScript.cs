using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] private GameObject Sun;
    public bool Inner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            PlayerShipMovement.instance.outside = true;
            PlayerShipMovement.instance.lastInsidePosition = Sun.transform.position;
            PlayerShipMovement.instance.ableToMove = false;
            PlayerShipMovement.instance.ableToAttack = false;
            //   PlayerShipMovement.instance.gameObject.transform.Rotate(new Vector3(0, PlayerShipMovement.instance.gameObject.transform.eulerAngles.y + 180,0), Space.Self);

        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (Inner)
            {
                
            PlayerShipMovement.instance.outside = false;
            PlayerShipMovement.instance.ableToMove = true;
            PlayerShipMovement.instance.ableToAttack = true;
            }
        }
    }
}
