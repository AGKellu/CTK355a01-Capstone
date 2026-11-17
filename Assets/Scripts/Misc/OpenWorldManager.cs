using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
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
    //private string lastStationName;

    public string lastStationName;
    public GameObject OptionsCanvas;
    public Transform questListContent;
    public GameObject questEntryPrefab;
    //public List<QuestProgress> newQuests = new();
    public List<QuestProgress> activateQuests = new();
    //show available quests in openworld manager 
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
        //int AmountofFighters = Random.Range(0, 6);

        int AmountofFighters = 6;
        Vector3 PlayerPos = PlayerShipMovement.instance.gameObject.transform.position;
        for (int i = 0; i < AmountofFighters; i++)
        {


            if (i == 0)
            {
                //Above 50 places
                GameObject Fighter = Instantiate(EnemyFighter, new Vector3(PlayerPos.x, PlayerPos.y + 50f, PlayerPos.z), Quaternion.identity);

            }
            else if (i == 1)
            {
                //Below 50 places
                GameObject Fighter = Instantiate(EnemyFighter, new Vector3(PlayerPos.x, PlayerPos.y - 50f, PlayerPos.z), Quaternion.identity);

            }
            else if (i == 2)
            {
                //To the right 50 places
                GameObject Fighter = Instantiate(EnemyFighter, new Vector3(PlayerPos.x + 50f, PlayerPos.y, PlayerPos.z), Quaternion.identity);

            }
            else if (i == 3)
            {
                //To the left 50 places
                GameObject Fighter = Instantiate(EnemyFighter, new Vector3(PlayerPos.x - 50f, PlayerPos.y, PlayerPos.z), Quaternion.identity);

            }
            else if (i == 4)
            {
                //In front 50 places
                GameObject Fighter = Instantiate(EnemyFighter, new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z + 50f), Quaternion.identity);

            }
            else if (i == 5)
            {
                //Behind 50 places
                GameObject Fighter = Instantiate(EnemyFighter, new Vector3(PlayerPos.x, PlayerPos.y, PlayerPos.z - 50f), Quaternion.identity);

            }
            //Instantiate each fighter 50m away from player in each direction  
            //figure out where you want to spawn fighters when in combat 
        }
    }
    //give the activequestlist to this script 
    public void SwapQuests(List<QuestProgress> activeQuests)
    {
        activateQuests.Clear();
        foreach (QuestProgress quest in activeQuests)
        {
            activateQuests.Add(quest);
            //quest.Add(new activateQuests)
        }
    }
    public void ShowCurrentQuests()
    {

        //foreach (Transform child in questListContent)
        //{
        //  Destroy(child.gameObject);
        //}
        if (questListContent.childCount != activateQuests.Count)
        {
            Debug.Log("show a quest");
            foreach (var quest in activateQuests)
            {

                GameObject entry = Instantiate(questEntryPrefab, questListContent);
                entry.GetComponent<QuestController>().quest = quest.quest;
                entry.GetComponent<QuestController>().ShowQuest();


            }
        }
        else
        {
            for (int i = 0; i < questListContent.childCount; i++)
            {
                
                if (IsQuestActive(questListContent.transform.GetChild(i).gameObject.GetComponent<QuestController>().quest.questID))
                {
                    Quest quest = questListContent.transform.GetChild(i).gameObject.GetComponent<QuestController>().quest;
                    foreach(var objective in quest.objectives)
                    {
                        
                    Debug.Log(objective.currentAmount);
                    }
                }
               // Debug.Log(questListContent.transform.GetChild(i).gameObject.GetComponent<QuestController>().quest.questName);
            }
            //foreach (GameObject child in questListContent)
            //{
                //if child.questcontroller.quest.quest == activatequest.
            //}
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
                    //Debug.Log("encounter now");
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
        lastStationName = StationName;
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InStationScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            loadingScene.SetActive(true);
            loadingBarFill.fillAmount = asyncLoad.progress;
            yield return null;
        }
        loadingScene.SetActive(false);
        SceneManager.UnloadSceneAsync(currentScene);
        PlayerShipMovement.instance.gameObject.GetComponent<AudioSource>().Pause();
        Cursor.lockState = CursorLockMode.Confined;
        PlayerShipMovement.instance.gameObject.SetActive(false);
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
        PlayerShipMovement.instance.gameObject.SetActive(true);
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
    public void GiveQuest(Quest quest)
    {
        activateQuests.Add(new QuestProgress(quest));
    }
    public void QuestObjectiveChecker(string QuestID)
    {
        //Debug.Log()
        foreach (var quest in activateQuests)
        {
            //if (.QuestID == QuestID)
            //{
            //  Debug.Log(quest.quest.name);
            for (int i = 0; i < questListContent.childCount; i++)
            {
                
                if (IsQuestActive(questListContent.transform.GetChild(i).gameObject.GetComponent<QuestController>().quest.questID))
                {
                    Quest questPrefab = questListContent.transform.GetChild(i).gameObject.GetComponent<QuestController>().quest;
                    foreach(var objective in questPrefab.objectives)
                    {
                        objective.currentAmount++;  
                    //Debug.Log(objective.currentAmount);
                    }
                }
               // Debug.Log(questListContent.transform.GetChild(i).gameObject.GetComponent<QuestController>().quest.questName);
            }
           /* foreach (var objective in quest.objectives)
            {
                Debug.Log(objective.objectiveID);
                Debug.Log(QuestID);
                if (objective.objectiveID == QuestID)
                {
                    objective.currentAmount++;
                    ShowCurrentQuests();
                    //Debug.Log(objective.currentAmount + " / " + objective.requiredAmount);
                    if (objective.isCompleted)
                    {
                        Debug.Log("Killed enough pirates");
                    }
                }
            }*/

            //} 
        }
    }
    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exiting Game...");
    }
}
