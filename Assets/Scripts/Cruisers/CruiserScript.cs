using UnityEngine;

public class CruiserScript : MonoBehaviour
{
    public GameObject[] SpawnPoints;
    public GameObject EnemyFighter;
    public bool EnemyCruiser;
    public GameObject PlayerFighter;
    public GameObject PlayerSpawnPoint;
    //Random rnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void RealStart()
    {
        int AmountOfFighters = Random.Range(1, 5);
        if (!EnemyCruiser)
        {
            //   SpawnPlayer();
        }
        else
        {

            Spawn(AmountOfFighters);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }
    void Spawn(int Amount)
    {
        Debug.Log(Amount);
            for (int i = 0; i < Amount; i++)
        {
            //GameObject Fighter = Instantiate(EnemyFighter, SpawnPoints[1].transform.position, transform.rotation);
            GameObject Fighter = Instantiate (EnemyFighter, SpawnPoints[i].transform);
            Fighter.transform.position = SpawnPoints[i].transform.position;
            Fighter.transform.rotation = Quaternion.identity;
            Fighter.transform.parent = null;
        }
        
    }
    public void SpawnPlayer(float Volume, float sens)
    {
        
        GameObject PlayerShip = Instantiate(PlayerFighter, PlayerSpawnPoint.transform);
        PlayerShip.GetComponent<AudioSource>().volume = Volume;
        PlayerShip.GetComponent<PlayerShipMovement>().masterSens = sens;
        PlayerShip.transform.position = PlayerSpawnPoint.transform.position;
        PlayerShip.transform.rotation = Quaternion.identity;
        PlayerShip.transform.parent = null;
    }
    public void RespawnPlayer()
    {
        //GameObject PlayerShip = Instantiate(PlayerFighter, PlayerSpawnPoint.transform.position, Quaternion.identity);
        PlayerShipMovement.instance.gameObject.transform.position = PlayerSpawnPoint.transform.position;
        PlayerShipMovement.instance.gameObject.transform.rotation = Quaternion.identity;
        PlayerShipMovement.instance.Respawn();//        PlayerShip.GetComponent<PlayerShipMovement>().Respawn();
    }
}
