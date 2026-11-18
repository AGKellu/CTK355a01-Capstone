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
    public Transform objectiveList;
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
        //MyData genericInstance = ScriptableObject.CreateInstance<MyData>();
        //genericInstance.value = 10;
        //Debug.Log("Generic instance value: " + genericInstance.value);
        //quest = availableQuests[RandomQuestInt];
        //quest = new Quest();
        quest = ScriptableObject.CreateInstance<Quest>();
        QuestObjective questObjective = new QuestObjective();
        ObjectiveType type = ObjectiveType.Custom;
        switch (RandomQuestInt)
        {
            case 0:
                quest.questName = "Go To Location";
                questObjective.objectiveID = "0";
                quest.questPosition = new Vector3(0, 0, 0);
                questObjective.description = "Fly To This Location : " + quest.questPosition;
                type = ObjectiveType.GetToLocation;
                questObjective.type = type;
                questObjective.currentAmount = 0;
                questObjective.requiredAmount = 1;
                //questObjective.objectiveID = Random.Range(0, 255).ToString();

                quest.objectives.Add(questObjective);
                break;
            case 1:
                int MineAmount = Random.Range(6, 13);
                quest.questPosition = new Vector3(0, 0, 0);
                quest.questName = "Clear a Minefield";
                questObjective.objectiveID = "2";
                questObjective.description = "Destroy " + MineAmount + " Mines at " + quest.questPosition;
                type = ObjectiveType.ClearMine;
                questObjective.type = type;
                questObjective.currentAmount = 0;
                questObjective.requiredAmount = MineAmount;
                //questObjective.objectiveID = Random.Range(0, 255).ToString();
                quest.objectives.Add(questObjective);
                break;
            case 2:
                int PirateAmount = Random.Range(1, 10);
                quest.questName = "Clear Pirates";
                questObjective.objectiveID = "1";
                questObjective.description = "Kill " + PirateAmount + " Pirates";
                type = ObjectiveType.KillEnemy;
                questObjective.type = type;
                questObjective.currentAmount = 0;
                questObjective.requiredAmount = PirateAmount;
                //questObjective.objectiveID = Random.Range(0, 255).ToString();
                quest.objectives.Add(questObjective);
                break;
            case 3:
                quest.questName = "NPC Convo";
                questObjective.objectiveID = "4";
                questObjective.description = "Talk To this NPC";
                type = ObjectiveType.TalkToNPC;
                questObjective.type = type;
                questObjective.currentAmount = 0;
                questObjective.requiredAmount = 1;
                //questObjective.objectiveID = Random.Range(0, 255).ToString();
                
                quest.objectives.Add(questObjective);
                break;
        }
        
       // Debug.Log(questObjective.requiredAmount);
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
            //Debug.Log(objective.currentAmount);
            GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
            TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
            objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
        }
    }
    public void UpdateQuest()
    {
        foreach (var objective in quest.objectives)
        {
            //Debug.Log(objective.currentAmount);
            for (int i = 0; i < objectiveList.childCount; i++)
            {
                GameObject objTextGO = objectiveList.GetChild(i).gameObject;
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
                if (objective.isCompleted)
                {
                    if (objective.type == ObjectiveType.KillEnemy)
                    {
                        
                        Debug.Log("Killed enough enemies");
                    }
                }
            }
            
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
