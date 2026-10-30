using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public string lastStationName;
    public static SaveLoad instance;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Save(string StationName)
    {
        lastStationName = StationName;
        SaveObject saveObject = new SaveObject
        {
            lastStationName = lastStationName,
            savedMaxMissiles = PlayerShipMovement.instance.MaxMissiles,
            savedMaxSpeed = PlayerShipMovement.instance.maxSpeed,
            savedMoney = PlayerShipMovement.instance.money,
            savedHealth = PlayerShipMovement.instance.Health  

        };
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }
    public void Load()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            PlayerShipMovement.instance.MaxMissiles = saveObject.savedMaxMissiles;
            PlayerShipMovement.instance.maxSpeed = saveObject.savedMaxSpeed;
            PlayerShipMovement.instance.money = saveObject.savedMoney;
            PlayerShipMovement.instance.Health = saveObject.savedHealth;
        }
        else
        {
            Debug.Log("No Save Found!");
        }
    }
    public string SetString()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            return saveObject.lastStationName;
        }
        else
        {
            return null;
        }
    }
    private class SaveObject
    {
        public string lastStationName;
        public int savedMaxMissiles;
        public float savedMaxSpeed;
        public int savedMoney;
        public int savedHealth;
    }
}
