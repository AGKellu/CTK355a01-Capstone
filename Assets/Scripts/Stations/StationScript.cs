using UnityEngine;

public class StationScript : MonoBehaviour
{
    public GameObject HangarGO;
    public GameObject EndHangarGoPosition;
    public bool inVicinity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // Debug.Log(Vector3.Distance(transform.position, PlayerShipMovement.instance.gameObject.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime, Space.Self);
        if (inVicinity)
        {
            HangarGO.transform.position = Vector3.MoveTowards(transform.position, EndHangarGoPosition.transform.position, .01f);
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            inVicinity = true;
            PlayerShipMovement.instance.ableToMove = false;
            PlayerShipMovement.instance.speed = 0;
            PlayerShipMovement.instance.ableToAttack = false;
            PlayerShipMovement.instance.speedRadialUI.fillAmount = 0;
        }
    }
}
