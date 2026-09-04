using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
public class PlayerShipMovement : MonoBehaviour
{
    [Header("Input Actions")]
    //PlayerInput input = PlayerInput();
    private InputAction ThrottleUp;
    private InputAction ThrottleDown;
    private InputAction RollLeftRight;
    private InputAction PitchForwardBackward;
    private InputAction PitchLeft;
    private InputAction PitchRight;

    [Header("Variables")]
    private Vector3 pos;
    //private Vector3 rotat;
    public float speed;
    private Vector2 rotato;
    [SerializeField] private float Zrotat;
    [SerializeField] private float Yrotat;
    [SerializeField] private float Xrotat;
    private bool rolling;

    [Header("Player Components")]
    private Rigidbody playerRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        playerRB = gameObject.GetComponent<Rigidbody>();
        //input.actions.FindActionMap("Movement").Enable();
        ThrottleUp = InputSystem.actions.FindAction("Movement/ThrottleUp");
        ThrottleUp.performed += ctx => AddSpeed();
        ThrottleDown = InputSystem.actions.FindAction("Movement/ThrottleDown");
        ThrottleDown.performed += ctx => SubtractSpeed();
        PitchRight = InputSystem.actions.FindAction("Movement/PitchRight");
        PitchLeft = InputSystem.actions.FindAction("Movement/PitchLeft");
        //PitchForwardBackward = InputSystem.actions.FindAction("Movement/PitchFB");
        RollLeftRight = InputSystem.actions.FindAction("Movement/RollLR");
        RollLeftRight.performed += ctx => Roll();
        RollLeftRight.canceled += ctx => StopRolling();
        Cursor.lockState = CursorLockMode.Locked;
        //RollValue = RollLeftRight.ReadValue<float>();
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
        if (rolling)
        {

            //DO SMOOTH step with everything below
            if (rotato.x < -0.1)
            {
                if (rotato.x > -360)
                {
                    //Yrotat -= 1;
                    Yrotat = Mathf.SmoothStep(Yrotat, Yrotat - 2, 1f);
                }
                //Debug.Log("Move up");
            }
            else if (rotato.x > 0.1)
            {
                if (rotato.x < 360)
                {
                    //Yrotat += 1;
                    Yrotat = Mathf.SmoothStep(Yrotat, Yrotat + 2, 1f);
                }
                //Debug.Log("Move down");
            }
            if (rotato.y < -0.1)
            {
                if (rotato.y > -360)
                {
                    //Xrotat += 1;
                    Xrotat = Mathf.SmoothStep(Xrotat, Xrotat + 2, 1f);
                }
                //Debug.Log("Move right");
            }
            else if (rotato.y > 0.1)
            {
                if (rotato.y < 360)
                {
                    //Xrotat -= 1;
                    Xrotat = Mathf.SmoothStep(Xrotat, Xrotat - 2, 1f);
                }
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
        //Debug.Log(RollValue);
        //transform.Rotate(Zrotat/30, Yrotat/30, 0);
        transform.rotation = Quaternion.Euler(Xrotat, Yrotat, Zrotat);
        //transform.Rotate(Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, RollValue);
        playerRB.AddRelativeForce(Vector3.forward * ((speed * 50) * Time.deltaTime));
     
        //transform.position += transform.forward * (speed / 60);
    }
    void AddSpeed()
    {
        //Keyboard is incremental
        //Controller is smoothed
        if (speed < 5)
        {
            speed += 1;
        }
    }
    void SubtractSpeed()
    {
        //Keyboard is incremental
        //Controller is smoothed
        if (speed > 0)
        {
            speed -= 1;
        }
        if (speed == 0)
        {
            playerRB.linearVelocity = new Vector3(0, 0, 0);
        }
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
    
}
