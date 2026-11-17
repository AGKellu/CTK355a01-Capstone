using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
public class QuestController : MonoBehaviour
{
    // public static QuestController instance;
    //public List<QuestProgress> activateQuests = new();
    public int questInProgressIndex;
    public int questCompletedIndex;
    public Quest quest;
    [SerializeField] private int RandomQuestInt;
    [SerializeField] private Quest[] availableQuests;

    private enum QuestState { NotStarted, InProgress, Completed }
    private QuestState questState = QuestState.NotStarted;
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private Transform objectiveList;
    [SerializeField] private GameObject objectiveTextPrefab;
    void Start()
    {
        //int RandomQuestInt = Random.Range(0, 4);
        SyncQuestState();
        /*switch (RandomQuestInt)
        {
            case 0:
                quest = availableQuests[0];
                break;
            case 1:
                quest = avaialableQuests[1];
                break;
            case 2:
                quest = availableQuests[2];
                break;
            case 3:

                break;
        }*/


    }
    public void Roll()
    {

        RandomQuestInt = Random.Range(0, 4);
        quest = availableQuests[RandomQuestInt];
        //Debug.Log(RandomQuestInt);
        //TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
        //Transform objectiveList = entry.transform.Find("ObjectiveList");
        questNameText.text = quest.name;
        foreach (var objective in quest.objectives)
        {
            GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
            TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
            objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
        }
    }
    public void ShowQuest()
    {
        questNameText.text = quest.name;
        
        foreach (var objective in quest.objectives)
        {
            Debug.Log(objective.currentAmount);
            GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
            TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
            objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
        }
    }
    public void SyncQuestState()
    {
        string questID = quest.questID;
        //StationUIMasterScript MasterScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<StationUIMasterScript>();
        

        if (OpenWorldManager.instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;

        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }
    public void Accept()
    {
        //StationUIMasterScript.instance.AcceptQuest(quest);
        StationUIMasterScript MasterScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<StationUIMasterScript>();
        MasterScript.AcceptQuest(quest);
        questState = QuestState.InProgress;
    }
    //private QuestUI questUI;
    /*private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
            //questUI = FindObjectOfType<StationUIMasterScript>();
        }
    }*/
    /*public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questID))
        {
            return;
        }
        activateQuests.Add(new QuestProgress(quest));
        StationUIMasterScript.instance.UpdateQuestUI();
    }
    public bool IsQuestActive(string questID) => activateQuests.Exists(q => q.QuestID == questID);
    */
}
