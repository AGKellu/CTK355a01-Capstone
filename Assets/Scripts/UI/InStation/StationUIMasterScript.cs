using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class StationUIMasterScript : MonoBehaviour
{
    //Go to the bounty board
    //change the sprite of the Selected Window in the middle
    //Make a list of "available" bounties; show; update when player does them 
    //Show the station name 
    //change the station name
    //Trade with the stock market 
    //FUCK NO (yet)
    //Talk with others 
    //Change selected window to the bar 
    //Leave 
    //Leave, set PlayerShipMovement.instance.TimeFromStation = 0
    public GameObject BountyBoard;
    public TMP_Text StationNameText;
    public Transform questListContent;
    public GameObject questEntryPrefab;
    //public GameObject objectiveTextPrefab;
    public Quest testQuest;
    public int testQuestAmount;
    private List<QuestProgress> testQuests = new();
    public List<QuestProgress> activateQuests = new();
    //public static StationUIMasterScript instance;
    //Below is for NPCs that give quests, make it so that buttons do the same 
    /*private enum QuestState { NotStarted, InProgress, Completed }
    private QuestState questState = QuestState.NotStarted;
    public int questInProgressIndex;
    public int questCompletedIndex;
    public Quest quest;*/

    //open bounty board, accept mission, get players current mission, add mission to players mission array, add to mission array in UI 
    //if player unchecks mission on the menu, look through the player's array, check what element
    //for int i =  0, blah, blah, if int is element number, dont show 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }*/
    void Start()
    {
        //Time.timeScale = 1;
        for (int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }
        StationNameText.text = OpenWorldManager.instance.lastStationName;
        //UpdateQuestUI();
    }
    //public void SetName(string stationName)
    //{
      //  StationNameText = stationName;
    //}
    // Update is called once per frame
    public void SwitchToBounty()
    {

    }
    public void UpdateQuestUI()
    {
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }
        /*foreach (var quest in activateQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");
            questNameText.text = quest.quest.name;
            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
            }
        }*/
    }
    public void Leave()
    {
        PlayerShipMovement.instance.InDock = false;
        OpenWorldManager.instance.SwapQuests(activateQuests);
        OpenWorldManager.instance.LeaveStation();
        //StartCoroutine(OpenWorldManager.instance.LeaveStation());
        // StartCoroutine(LeaveRun());
    }
    public void ShowAvailableQuests()
    {
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }
        //List of current Quests and list of available quests are different 
        foreach (var quest in testQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            entry.GetComponent<QuestController>().Roll();
            
            /*TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");
            questNameText.text = quest.quest.name;
            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
            }*/
        }
    }
    
    public void AcceptQuest(Quest quest)
    {
        if (OpenWorldManager.instance.IsQuestActive(quest.questID))
        {
            return;
        }
        activateQuests.Add(new QuestProgress(quest));
        //UpdateQuestUI();
        ShowAvailableQuests();
    }
    /*IEnumerator LeaveRun()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ProtoScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            loadingScene.SetActive(true);
            loadingBarFill.fillAmount = asyncLoad.progress;
            yield return null;
        }
        PlayerShipMovement.instance.TimeFromStation = 0;
        SceneManager.UnloadSceneAsync(currentScene);
    }*/
}
