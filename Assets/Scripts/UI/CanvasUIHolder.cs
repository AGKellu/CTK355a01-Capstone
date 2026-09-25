using UnityEngine;
using UnityEngine.UI;
public class CanvasUIHolder : MonoBehaviour
{
    public GameObject CrosshairUI;
    //public Image speedRadialUI;
    public GameObject TargetFighterStill;
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
