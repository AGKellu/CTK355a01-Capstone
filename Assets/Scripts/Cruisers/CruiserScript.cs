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

    }
    void Spawn(int Amount)
    {
        //Debug.Log(Amount);
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
        //Debug.Log("Hello");
        //GameObject PlayerShip = Instantiate(PlayerFighter, transform);
       // PlayerShip.transform.parent = null;
        //GameObject PlayerShip = Instantiate(PlayerFighter, PlayerSpawnPoint.transform.position, Quaternion.identity);
        //PlayerShipMovement ShipScript = PlayerShip.GetComponent<PlayerShipMovement>();
        //PlayerShip.GetComponent<PlayerShipMovement>().playerRB = PlayerShip.GetComponent<Rigidbody>();
        //PlayerShip.GetComponent<PlayerShipMovement>().Respawn();
        GameObject PlayerShip = Instantiate(PlayerFighter, PlayerSpawnPoint.transform);
        PlayerShip.GetComponent<AudioSource>().volume = Volume;
        PlayerShip.GetComponent<PlayerShipMovement>().masterSens = sens;
        //PlayerShip.GetComponent<PlayerShipMovement>().Xsens = sens;
        //PlayerShip.GetComponent<PlayerShipMovement>().Ysens = sens;
        //PlayerShip.GetComponent<PlayerShipMovement>().Zsens = sens;
        PlayerShip.transform.position = PlayerSpawnPoint.transform.position;
        PlayerShip.transform.rotation = Quaternion.identity;
        PlayerShip.transform.parent = null;
    }
    public void RespawnPlayer()
    {
        GameObject PlayerShip = Instantiate(PlayerFighter, PlayerSpawnPoint.transform.position, Quaternion.identity);
        PlayerShip.GetComponent<PlayerShipMovement>().Respawn();
    }
}
