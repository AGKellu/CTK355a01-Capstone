using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
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

    //open bounty board, accept mission, get players current mission, add mission to players mission array, add to mission array in UI 
    //if player unchecks mission on the menu, look through the player's array, check what element
    //for int i =  0, blah, blah, if int is element number, dont show 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SwitchToBounty()
    {
        
    }
    public void Leave()
    {
        OpenWorldManager.instance.LeaveStation();
        //StartCoroutine(OpenWorldManager.instance.LeaveStation());
       // StartCoroutine(LeaveRun());
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
