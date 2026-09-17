using UnityEngine;

public class AmmunitionMoveScript : MonoBehaviour
{
    //private Rigidbody AmmoRB;
    public Vector3 forward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AmmoRB = gameObject.GetComponent<Rigidbody>();
        transform.parent = null;
        //transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //AmmoRB.AddRelativeForce(Vector3.forward * (5 * Time.deltaTime ), ForceMode.Acceleration);
        transform.position += forward * .5f * Time.deltaTime; 
    }
}
