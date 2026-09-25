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
    //From ShipAttack Script
    private InputAction Shoot;
    private InputAction AltFire;

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
    public bool Targeted;
    

    [Header("Player Components")]
    private Rigidbody playerRB;
    //From ShipAttack Script
    [SerializeField] private CinemachineVirtualCamera Camera;
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
    [SerializeField] private Image speedRadialUI;
    public GameObject TargetedFighter;
    //[SerializeField] public GameObject TargetedFighterStill;
    public static PlayerShipMovement instance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
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
        Shoot.performed += ctx => ShootLaser();
        AltFire = InputSystem.actions.FindAction("Attacks/Zoom");
        AltFire.performed += ctx => Target();
        //AltFire.performed += ctx => Zoom();
        //AltFire.canceled += ctx => ZoomCancel();
        DeathCamera.GetComponent<DeathCameraScript>().Player = gameObject;
        DeathCamera.LookAt = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
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
            if (Targeted)
            {
                if (TargetedFighter.name.Contains("Buzzer"))
                {
                    Debug.Log(TargetedFighter.GetComponent<BuzzerScript>().Health);
                
                }
                Debug.Log(Vector3.Distance(TargetedFighter.transform.position, transform.position).ToString("F2"));
            }
            if (rolling)
            {

                //DO SMOOTH step with everything below
                if (rotato.x < -0.1)
                {
                    //if (rotato.x > -360)
                    //{
                        //Yrotat -= 1;
                        Yrotat = Mathf.SmoothStep(Yrotat, Yrotat - Ysens, 1f);
                    //}
                    //Debug.Log("Move up");
                }
                else if (rotato.x > 0.1)
                {
                   //if (rotato.x < 360)
                    //{
                        //Yrotat += 1;
                        Yrotat = Mathf.SmoothStep(Yrotat, Yrotat + Ysens, 1f);
                    //}
                    //Debug.Log("Move down");
                }
                if (rotato.y < -0.1)
                {
                    //if (rotato.y > -360)
                    //{
                        //Xrotat += 1;
                        Xrotat = Mathf.SmoothStep(Xrotat, Xrotat + Xsens, 1f);
                    //}
                    //Debug.Log("Move right");
                }
                else if (rotato.y > 0.1)
                {
                   // if (rotato.y < 360)
                    //{
                        //Xrotat -= 1;
                        Xrotat = Mathf.SmoothStep(Xrotat, Xrotat - Xsens, 1f);
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
                        speedRadialUI.fillAmount = (speed /5);
                }
            }
            else if (slowingDown)
            {
                if (speed > 0)
                {
                        speed -= 0.05f;
                        speedRadialUI.fillAmount = (speed /5);
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
            playerRB.AddRelativeForce(transform.forward * ((speed * 50) * Time.deltaTime), ForceMode.Acceleration);
        }
    }
    void AddSpeed()
    {
        //if incremental is true, set the following code in an if method
        //if (speed < 5)
        //{
        //  speed += 1;
        //}
        if(IncrementalSpeed)
        {
             if (speed < 5)
        {
          speed += 1;
        }
        }
        else
        {
            
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
    void ShootLaser()
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
       GameObject Laser1 = Instantiate(LaserPrefab, LaserPoint1.transform.position, Quaternion.Euler(90, Yrotat, 0));
       Laser1.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
       GameObject Laser2 = Instantiate(LaserPrefab, LaserPoint2.transform.position, Quaternion.Euler(90, Yrotat, 0));
        Laser2.GetComponent<AmmunitionMoveScript>().forward = transform.forward;
    }
    void Zoom()
    {
        Camera.Priority = 0;
        ZoomCamera.Priority = 1;
        DeathCamera.Priority = 0;
    }
    void ZoomCancel()
    {
        Camera.Priority = 1;
        ZoomCamera.Priority = 0;
        DeathCamera.Priority = 0;
    }
    void Target()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),out hit, 1000))
        {
            
            //if allies are made, just comment the below if statement
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                TargetedFighter = hit.collider.gameObject;
                
                Targeted = true;
                //if (hit.collider.gameObject.name.Contains("Buzzer"))
                //{
                    //Debug.Log(hit.collider.gameObject.GetComponent<BuzzerScript>().Health);
                    //Debug.Log(Vector3.Distance(transform.position, hit.collider.gameObject.transform.position));
                //}
            }
            //Debug.Log(hit.collider.gameObject.name);
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
        Camera.Priority = 0;
        ZoomCamera.Priority = 0;
        DeathCamera.Priority = 1;
        GetComponent<PlayerInput>().actions.FindActionMap("Movement").Disable();
        CanvasUIHolder.instance.gameObject.SetActive(false);
        //CrosshairUI.SetActive(false);
        //DeathCamera.gameObject.SetActive(true);

        //Put rotate around player in update of deathcam script
        //Make sure to setactive(false) when respawning
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Asteroid"))
        {
            //Health-=1;
            TakeDamage(1);
        }
    }
}
