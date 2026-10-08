using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Cinemachine;
using UnityEngine.UI;
public class PlayerShipMovement : MonoBehaviour
{
    [Header("Input Actions")]
    private InputAction ThrottleUp;
    private InputAction ThrottleDown;
    private InputAction RollLeftRight;
    private InputAction PitchForwardBackward;
    private InputAction PitchLeft;
    private InputAction PitchRight;
    private InputAction Shoot;
    private InputAction AltFire;
    private InputAction FireMissile;
    private InputAction FireSmoke;

    [Header("Variables")]
    private Vector3 pos;
    public float speed;
    private Vector2 rotato;
    [SerializeField] private float Zrotat;
    [SerializeField] private float Yrotat;
    [SerializeField] private float Xrotat;
    private bool rolling;
    //From ShipAttack Script
    public bool dead;
    private int NormFOV = 60;
    private int ZoomFOV = 30;
    public int Health;
    private bool speedingUp;
    private bool slowingDown;
    public float Ysens;
    public float Xsens;
    public float masterSens;
    public bool Targeted;
    public int MissileCount;
    //After 3 consecutive seconds of having the targeted ship in the viewport, set the target of the missile to targeted fighter before firing, set the missile transform.movetowards to the target
    //if the target has smoke behind them(a box trigger that turns off after 1 second), missile randomly rotates in any 4 cardinal directions (towards up/down, towards left/right) from 30 to 90, stops movetowards, then keeps going for .5 seconds, then explodes 
    //else, have the missile explode after 1 seconds when shot; missiles are half as fast as lasers 
    public int MaxMissiles;
    public bool MissileLoaded;
    public GameObject DeathPanel; 


    [Header("Player Components")]
    private Rigidbody playerRB;
    //public Rigidbody playerRB;
    //From ShipAttack Script
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private CinemachineVirtualCamera ZoomCamera;
    [SerializeField] private CinemachineVirtualCamera DeathCamera;

    [Header("Misc")]
    //[SerializeField] private GameObject CrosshairUI;
    public bool IncrementalSpeed;
    //If this is true, pressing W or S would add or subtract speed incrementally by 1 or by -1, respectively

    [SerializeField] private GameObject FrameOfReference;
    [SerializeField] private GameObject LaserPrefab;
    [SerializeField] private GameObject LaserPoint1;
    [SerializeField] private GameObject LaserPoint2;
    [SerializeField] private GameObject MissilePoint;
    [SerializeField] private GameObject PlayerMissile;
    [SerializeField] private Image speedRadialUI;
    public GameObject TargetedFighter;
    [SerializeField] private float TargetedSeconds;
    [SerializeField] private int Target60Seconds;
    [SerializeField] private Image MissileTargetRadialUI;
    //[SerializeField] public GameObject TargetedFighterStill;
    public static PlayerShipMovement instance;
    [SerializeField] private GameObject PlayerForward;
    //Make it so that the player has 3 seconds to get back to the playable space (beacons, big space station, etc) 
    //Make things on the top of the cockpit (hula people) to make it feel lived in
    //Change the circle to a radar 
    //Manually make AI instead of using Navmesh 
        //Fake being erratic, roll a d12, move for x spaces, roll a d8 to pick a direction (up, down, left, right, forward, backward), roll a d12, move for x spaces, repeat

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
        Application.targetFrameRate = 60;
        playerRB = gameObject.GetComponent<Rigidbody>();
        ThrottleUp = InputSystem.actions.FindAction("Movement/ThrottleUp");
        ThrottleUp.performed += ctx => AddSpeed();
        ThrottleUp.canceled += ctx => StopSpeeding();
        ThrottleDown = InputSystem.actions.FindAction("Movement/ThrottleDown");
        ThrottleDown.performed += ctx => SubtractSpeed();
        ThrottleDown.canceled += ctx => StopSlowing();
        PitchRight = InputSystem.actions.FindAction("Movement/PitchRight");
        PitchLeft = InputSystem.actions.FindAction("Movement/PitchLeft");
        RollLeftRight = InputSystem.actions.FindAction("Movement/RollLR");
        RollLeftRight.performed += ctx => Roll();
        RollLeftRight.canceled += ctx => StopRolling();
        Cursor.lockState = CursorLockMode.Locked;
        //From ShipAttack Script
        Shoot = InputSystem.actions.FindAction("Attacks/Laser");
        Shoot.performed += ctx => ShootLaser("Laser");
        AltFire = InputSystem.actions.FindAction("Attacks/Zoom");
        AltFire.performed += ctx => Target();
        //AltFire.performed += ctx => Zoom();
        //AltFire.canceled += ctx => ZoomCancel();
        FireMissile = InputSystem.actions.FindAction("Attacks/FireMissile");
        FireMissile.performed += ctx => ShootLaser("Missile");
        FireSmoke = InputSystem.actions.FindAction("Attacks/FireSmokeScreen");
        FireSmoke.performed += ctx => SmokeScreen();
        DeathCamera.GetComponent<DeathCameraScript>().Player = gameObject;
        DeathCamera.LookAt = gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        

        /* if (Input.GetAxis("Mouse X") > 0.1)
         {
             if (Yrotat < 360)
             {
                 //Yrotat += 1;
                 Yrotat = Mathf.SmoothStep(Yrotat, Yrotat + 2, 1f);
             }
         }
         if (Input.GetAxis("Mouse X") < -0.1)
         {
             if (Yrotat > -360)
             {
                 //Yrotat -= 1;
                 Yrotat = Mathf.SmoothStep(Yrotat, Yrotat - 2, 1f);
             }
         }
         if (Input.GetAxis("Mouse Y") > 0.1)
         {
             if (Xrotat > -360)
             {
                 //Xrotat -= 1;
                 Xrotat = Mathf.SmoothStep(Xrotat, Xrotat - 2, 1f);
             }
         }
         if (Input.GetAxis("Mouse Y") < -0.1)
         {
             if (Xrotat < 360)
             {
                 //Xrotat += 1;
                 Xrotat = Mathf.SmoothStep(Xrotat, Xrotat + 2, 1f);
             }
         }*/
        if (!dead)
        {
            //if (Targeted)
            //{
            //if (TargetedFighter.name.Contains("Buzzer"))
            //{
            //}
            if (TargetedFighter != null && !MissileLoaded && (MissileCount < MaxMissiles))
        {
            if (TargetedSeconds < 3)
            {
                
                //if (!TargetedFighter.transform.GetChild(0).GetComponent<Renderer>().isVisible)
                Vector3 viewPos = Camera.main.WorldToViewportPoint(TargetedFighter.transform.position);
                //camera.current
                if (viewPos.x  > 1|| viewPos.y > 1|| viewPos.x < 0 || viewPos.y < 0)
                {
                    MissileTargetRadialUI.fillAmount = 0;
                    Target60Seconds = 0;
                    TargetedSeconds = 0;
                    Debug.Log("Missile stopped locking on!");
                }
                else 
                {
                    if (Target60Seconds < 60)
                {
                    //MissileTargetRadialUI.fillAmount = Target60Seconds / 60f;
                    Target60Seconds++;
                    
                    //MissileTargetRadialUI.fillAmount = Target60Seconds/(60f / TargetedSeconds);
                }
                else if (Target60Seconds == 60)
                {
                    TargetedSeconds++;
                    MissileTargetRadialUI.fillAmount = TargetedSeconds/2f;
                    
                    Debug.Log("Missile Second Checked");
                    Target60Seconds = 0;
                    //MissileCount++;

                }
                }
                //TargetedSeconds += ((1 / 60) * Time.deltaTime);
                //Debug.Log(TargetedSeconds);
            }
            if (TargetedSeconds >= 3)
            {
                MissileLoaded = true;
                Debug.Log("Missile Loaded, put on the crosshairUI \nPut Missiles on the dashboard UI");

                TargetedSeconds = 0;
            }
            //Debug.Log("The targeted seconds should be incrementing by 1/60 (due to 60 fps and max at 3 seconds)");
        }
            if (rolling)
            {

                //DO SMOOTH step with everything below
                if (rotato.x < -0.1)
                {
                    //if (rotato.x > -360)
                    //{
                    //Yrotat -= 1;
                    Yrotat = Mathf.SmoothStep(Yrotat, Yrotat - (Ysens * masterSens), 1f);
                    //}
                    //Debug.Log("Move up");
                }
                else if (rotato.x > 0.1)
                {
                    //if (rotato.x < 360)
                    //{
                    //Yrotat += 1;
                    Yrotat = Mathf.SmoothStep(Yrotat, Yrotat + (Ysens * masterSens) , 1f);
                    //}
                    //Debug.Log("Move down");
                }
                if (rotato.y < -0.1)
                {
                    //if (rotato.y > -360)
                    //{
                    //Xrotat += 1;
                    Xrotat = Mathf.SmoothStep(Xrotat, Xrotat + (Xsens * masterSens), 1f);
                    //}
                    //Debug.Log("Move right");
                }
                else if (rotato.y > 0.1)
                {
                    // if (rotato.y < 360)
                    //{
                    //Xrotat -= 1;
                    Xrotat = Mathf.SmoothStep(Xrotat, Xrotat - (Xsens * masterSens), 1f);
                    // }
                    //Debug.Log("Move left");
                }
            }

            if (PitchLeft.IsPressed())
            {
                if (Zrotat < 180)
                {

                    Zrotat += 1;
                }
            }
            if (PitchRight.IsPressed())
            {
                if (Zrotat > -180)
                {

                    Zrotat -= 1;
                }
            }

            //following code is to be if incremental is false, put an if method here
            if (IncrementalSpeed)
            {

            }
            else
            {
                if (speedingUp)
                {
                    if (speed < 5)
                    {
                        speed += 0.05f;
                        speedRadialUI.fillAmount = (speed / 5);
                    }
                }
                else if (slowingDown)
                {
                    if (speed > 0)
                    {
                        speed -= 0.05f;
                        speedRadialUI.fillAmount = (speed / 5);
                        if (speed < 0)
                        {
                            speed = 0;
                            playerRB.linearVelocity = new Vector3(0, 0, 0);

                        }

                    }
                }
            }

            transform.rotation = Quaternion.Euler(Xrotat, Yrotat, Zrotat);
            FrameOfReference.transform.localRotation = Quaternion.Euler(Xrotat, Yrotat, Zrotat);
            // playerRB.AddRelativeForce(Vector3.forward * ((speed * 50) * Time.deltaTime), ForceMode.Acceleration);
            //playerRB.AddForce(transform.forward * ((speed * 50) * Time.deltaTime), ForceMode.Acceleration);

            //Previous
            //playerRB.AddRelativeForce(transform.forward * ((speed * 50) * Time.deltaTime), ForceMode.Acceleration);
            
            transform.position = Vector3.MoveTowards(transform.position, PlayerForward.transform.position, speed / 20);
        }
    }
    void AddSpeed()
    {
        //if incremental is true, set the following code in an if method
        //if (speed < 5)
        //{
        //  speed += 1;
        //}
        if (IncrementalSpeed)
        {
            if (speed < 5)
            {
                speed += 1;
            }
        }
        else
        {
            speedRadialUI.gameObject.GetComponent<AudioSource>().Play();
            speedingUp = true;
        }
    }
    void SubtractSpeed()
    {
        //if incremental is true, set the following code in an if method
        //if (speed > 0)
        //{
        //  speed -= 1;
        //}
        //if (speed == 0)
        //{
        //  playerRB.linearVelocity = new Vector3(0, 0, 0);
        //}
        if (IncrementalSpeed)
        {
            if (speed > 0)
            {
                speed -= 1;
            }
            if (speed == 0)
            {
                playerRB.linearVelocity = new Vector3(0, 0, 0);
            }
        }
        else
        {

            slowingDown = true;
        }

    }
    void StopSpeeding()
    {
        speedingUp = false;
    }
    void StopSlowing()
    {
        slowingDown = false;
    }
    void OnEnable()
    {

    }
    void Roll()
    {
        rolling = true;
        rotato = RollLeftRight.ReadValue<Vector2>();
        //Debug.Log(rotato.x + "\n" + rotato.y);
        /*if (rotato.x < -0.1)
        {
            if (rotato.x > -360)
            {
                //Yrotat -= 1;
                Yrotat -= .5f;
            }
            //Debug.Log("Move up");
        }
        else if (rotato.x > 0.1)
        {
            if (rotato.x < 360)
            {
                //Yrotat += 1;
                Yrotat += .5f;
            }
            //Debug.Log("Move down");
        }
        if (rotato.y < -0.1)
        {
            if (rotato.y > -360)
            {
                //Xrotat += 1;
                Xrotat += .5f;
            }
            //Debug.Log("Move right");
        }
        else if (rotato.y > 0.1)
        {
            if (rotato.y < 360)
            {
                //Xrotat -= 1;
                Xrotat -= .5f;
            }
            //Debug.Log("Move left");
        }
        */
    }
    void StopRolling()
    {
        rolling = false;
    }
    void ShootLaser(string Projectile)
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        //{
        //Debug.Log("Hit something");
        //if (hit.collider.gameObject.CompareTag("Enemy"))
        //{
        //  Destroy(hit.collider.gameObject);
        // Debug.Log("Hit enemy\nDo damage");
        //}
        // }
        if (Projectile == "Laser")
        {

            //GameObject Laser1 = Instantiate(LaserPrefab, LaserPoint1.transform.position, Quaternion.Euler(Xrotat, 0, 0));
            GameObject Laser1 = Instantiate(LaserPrefab, LaserPoint1.transform.position, LaserPoint1.transform.rotation);
            Laser1.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
            //GameObject Laser2 = Instantiate(LaserPrefab, LaserPoint2.transform.position, Quaternion.Euler(Xrotat, 0, 0));
            GameObject Laser2 = Instantiate(LaserPrefab, LaserPoint2.transform.position, LaserPoint2.transform.rotation);
            Laser2.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
        }
        else if (Projectile == "Missile")
        {
            if (MissileCount < MaxMissiles)
            {
                //TargetedFighter
                GameObject Missile = Instantiate(PlayerMissile, MissilePoint.transform.position, MissilePoint.transform.rotation);
                //Missile.GetComponent<AmmunitionMoveScript>().target = 
                Missile.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
                if (MissileLoaded)
                {
                    Missile.GetComponent<AmmunitionMoveScript>().target = TargetedFighter;
                    
                }
                MissileTargetRadialUI.fillAmount = 0;
                //MissileLoaded = false;
                MissileCount++;

            }

        }
    }
    void SmokeScreen()
    {

    }
    void Zoom()
    {
        VirtualCamera.Priority = 0;
        ZoomCamera.Priority = 1;
        DeathCamera.Priority = 0;
    }
    void ZoomCancel()
    {
        VirtualCamera.Priority = 1;
        ZoomCamera.Priority = 0;
        DeathCamera.Priority = 0;
    }
    void Target()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, .25f, transform.TransformDirection(Vector3.forward), out hit, 1000))
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1000))
        {

            //if allies are made, just comment the below if statement
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                if (hit.collider.gameObject != TargetedFighter) // Might have hotswapping targets or targetting nothing if hitting void
                {
                    TargetedFighter = hit.collider.gameObject;

                    Targeted = true;
                }
                else
                {
                    //if (TargetedFighter.name.Contains("Buzzer"))
                    //{

                    TargetedFighter.GetComponent<MasterScript>().DeTarget();
                    //}
                    //TargetedFighter = null;
                    //Debug.Log(TargetedFighter.name);
                    //Targeted = false;
                }

                //if (hit.collider.gameObject.name.Contains("Buzzer"))
                //{
                //Debug.Log(hit.collider.gameObject.GetComponent<BuzzerScript>().Health);
                //Debug.Log(Vector3.Distance(transform.position, hit.collider.gameObject.transform.position));
                //}
            }
            //Debug.Log(hit.collider.gameObject.name);
        }
        else
        //if (!Physics.SphereCast(transform.position, .25f, transform.TransformDirection(Vector3.forward), out hit, 1000))
        {
            if (TargetedFighter != null)
            {
                //if (TargetedFighter.name.Contains("Buzzer"))
                //{

                    TargetedFighter.GetComponent<MasterScript>().DeTarget();
                //}
                TargetedFighter = null;
                Targeted = false;
            }

        }
    }
    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            DeathCam();
        }
    }
    void DeathCam()
    {
        //Make Normal cam priority --
        //Make DeathCam priority++ and setactive true
        dead = true;
        playerRB.constraints = RigidbodyConstraints.FreezeAll;
        //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
        speed = 0;
        playerRB.linearVelocity = new Vector2(0, 0);
        VirtualCamera.Priority = 0;
        ZoomCamera.Priority = 0;
        DeathCamera.Priority = 1;
        GetComponent<PlayerInput>().actions.FindActionMap("Movement").Disable();
        GetComponent<PlayerInput>().actions.FindActionMap("Attacks").Disable();
        CanvasUIHolder.instance.gameObject.SetActive(false);
        FrameOfReference.SetActive(false);
        DeathPanel.SetActive(true);
        //CrosshairUI.SetActive(false);
        //DeathCamera.gameObject.SetActive(true);

        //Put rotate around player in update of deathcam script
        //Make sure to setactive(false) when respawning
    }
    public void Respawn()
    {
        GameObject PlayerCruiser = GameObject.Find("PlayerCruiser");
        transform.position = PlayerCruiser.GetComponent<CruiserScript>().PlayerSpawnPoint.transform.position;
        //playerRB.constraints = RigidbodyConstraints.None;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //speed = 
        VirtualCamera.Priority = 1;
        ZoomCamera.Priority = 0;
        DeathCamera.Priority = 0;
        CanvasUIHolder.instance.gameObject.SetActive(true);
        FrameOfReference.SetActive(true);
        GetComponent<PlayerInput>().actions.FindActionMap("Movement").Enable();
        GetComponent<PlayerInput>().actions.FindActionMap("Attacks").Enable();
        //transform.position = SpawnPosition
        dead = false;
        DeathPanel.SetActive(false);
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Asteroid"))
        {
            //Health-=1;
            //This is usually 1
            TakeDamage(1);
        }
    }
}
