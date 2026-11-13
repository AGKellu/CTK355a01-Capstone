using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class OpenWorldManager : MonoBehaviour
{
    public GameObject EnemyFighter;
    public GameObject loadingScene;
    public Image loadingBarFill;
    public static OpenWorldManager instance;
    public int TimeFromStation;
    private int encounterTimer;
    public bool inEncounter;
    private string lastStationName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        encounterTimer = Random.Range(600, 7200);
    }
    public void SpawnEnemy()
    {
        int AmountofFighters = Random.Range(1, 7);
        for (int i = 0; i < AmountofFighters; i++)
        {
            //figure out where you want to spawn fighters when in combat 
            //GameObject Fighter = Instantiate(EnemyFighter, )
        }
    }
    public void RespawnPlayer()
    {
        //SaveLoad.instance.Load();
        //Load
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerShipMovement.instance != null)
        {
            if (!PlayerShipMovement.instance.dead && !inEncounter)
            {
                if (TimeFromStation >= encounterTimer)
                {
                    Debug.Log("encounter now");
                    TimeFromStation = 0;
                    Roll();
                }
                else
                {
                    TimeFromStation++;
                }
            }
        }

    }
    public void GoToStation(string stationName)
    {
        StartCoroutine(StationLoad(stationName));
    }
    public void LeaveStation()
    {
        StartCoroutine(StationUnload());
    }

    IEnumerator StationLoad(string StationName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InStationScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            loadingScene.SetActive(true);
            loadingBarFill.fillAmount = asyncLoad.progress;
            yield return null;
        }
        lastStationName = StationName;
        loadingScene.SetActive(false);
        SceneManager.UnloadSceneAsync(currentScene);
        PlayerShipMovement.instance.gameObject.GetComponent<AudioSource>().Pause();
        Cursor.lockState = CursorLockMode.Confined;
        //StationUIMasterScript SUIMS = GameObject.FindGameObjectWithTag("Canvas").GetComponent<StationUIMasterScript>();
    }
    public IEnumerator StationUnload()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ProtoScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            loadingScene.SetActive(true);
            loadingBarFill.fillAmount = asyncLoad.progress;
            yield return null;
        }
        loadingScene.SetActive(false);
        SceneManager.UnloadSceneAsync(currentScene);
        GameObject[] Stations = GameObject.FindGameObjectsWithTag("Station");
        foreach (GameObject Station in Stations)
        {
            if (Station.name == lastStationName)
            {
                PlayerShipMovement.instance.gameObject.transform.position = Station.GetComponent<StationScript>().spawnPosition;
            }
        }
        PlayerShipMovement.instance.gameObject.GetComponent<AudioSource>().Play();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Roll()
    {

        int rolledInt = Random.Range(0, 4);
        switch (rolledInt)
        {
            case 0:
                Debug.Log("nothing happens");
                break;

            case 1:
                inEncounter = true;
                Debug.Log("pirate ambush!");
                break;
            case 2:
                inEncounter = true;
                Debug.Log("Protect the engineer at this location!");
                break;
            case 3:
                inEncounter = true;
                Debug.Log("Race to location!");
                break;
                //races, clearing minefields, clearing pirate nest, and escorting transports can all be redone by agreeing to them at the bounty board
        }
    }
}
