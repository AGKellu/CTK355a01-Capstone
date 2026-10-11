using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CanvasUIHolder : MonoBehaviour
{
    public GameObject CrosshairUI;
    //public Image speedRadialUI;
    public GameObject TargetFighterStill;
    public TMP_Text TargetFighterDistance;
    public TMP_Text TargetFighterHealth;
    public static CanvasUIHolder instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
