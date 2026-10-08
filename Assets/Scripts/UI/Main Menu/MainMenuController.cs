using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    [Header("VolumeSetting")]
    [SerializeField] private TMP_Text volumeTextVolume = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = .5f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSensTextValue = null;
    [SerializeField] private Slider controllerSensSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSens = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("ConfirmationPrompt")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    //[SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("LoadingScreen")]
    public GameObject loadingScreen;
    public Image loadingBarFill;

    //public SaveLoad SaveLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();   
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex =i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        StartCoroutine(LoadNewGameScene());
    }
    IEnumerator LoadNewGameScene()
    {
        //IEnumerator LoadSceneAsync()
        //{
            Scene currentScene = SceneManager.GetActiveScene();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_newGameLevel, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                loadingBarFill.fillAmount = asyncLoad.progress;
                yield return null;

            }

            //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            //GameObject rotators = GameObject.FindGameObjectWithTag("Rotator");
           // SceneManager.MoveGameObjectToScene(rotators, SceneManager.GetSceneByName(_newGameLevel));
            //foreach (GameObject player in players)
            //{
                //if (player.name == "Isabelle")
                //{
                // Debug.Log(player.gameObject.name);
               // SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(_newGameLevel));
                if (_newGameLevel == "ProtoScene")
                {
                   // if (player.name == "Isabelle")
                   // {
                       // Player playerScript = player.GetComponent<Player>();
                     
                    
                
                }
               
            
            SceneManager.UnloadSceneAsync(currentScene);

    }
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exiting Game...");
    }
    public void SetVolume(float volume)
    {
        
        AudioListener.volume = volume;
        volumeTextVolume.text = volume.ToString("0.0");
    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }
    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
        }
        else 
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
        }
        PlayerPrefs.SetFloat("masterSen", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }
    public void SetFullscreen(bool isfullscreen)
    {
        _isFullScreen = isfullscreen;
    }
    public void SetQuality(int QualityIndex)
    {
        _qualityLevel = QualityIndex;
    }
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(ConfirmationBox());
    }
    public void SetControllerSens(float sensitivity)
    {
        mainControllerSens = Mathf.RoundToInt(sensitivity);
        ControllerSensTextValue.text = sensitivity.ToString("0");
    }
    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume; 
            volumeSlider.value = defaultVolume;
            volumeTextVolume.text = defaultVolume.ToString("0.0");
            VolumeApply ();
        }
        if(MenuType == "Gameplay")
        {
            ControllerSensTextValue.text = defaultSen.ToString("0");
            controllerSensSlider.value = defaultSen;
            mainControllerSens = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }
        if(MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            //resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
        
    }
     public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
