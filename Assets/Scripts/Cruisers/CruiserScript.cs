using UnityEngine;

public class CruiserScript : MonoBehaviour
{
    public GameObject[] SpawnPoints;
    public GameObject EnemyFighter;
    //Random rnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int AmountOfFighters = Random.Range(0, 5);
        Spawn(AmountOfFighters);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Spawn(int Amount)
    {
        for (int i = 0; i < Amount; i++)
        {
            GameObject Fighter = Instantiate(EnemyFighter, SpawnPoints[i].transform.position, transform.rotation);

        }
    }
}
