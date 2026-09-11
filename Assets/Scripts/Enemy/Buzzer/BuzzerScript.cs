using UnityEngine;

public class BuzzerScript : MonoBehaviour
{
    private GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, .01f);
        Vector3 newRotate = Vector3.RotateTowards(transform.forward, (Player.transform.position - transform.position), 1f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newRotate);
    }
}
