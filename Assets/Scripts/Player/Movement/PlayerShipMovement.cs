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
    [SerializeField] private float Zrotat;
    [SerializeField] private float Yrotat;
    [SerializeField] private float Xrotat;

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
        PitchForwardBackward = InputSystem.actions.FindAction("Movement/PitchFB");
        RollLeftRight = InputSystem.actions.FindAction("Movement/RollLR");
        //RollValue = RollLeftRight.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") > 0.1)
        {
            if (Yrotat < 360)
            {
                Yrotat += 2;

            }
        }
        if (Input.GetAxis("Mouse X") < -0.1)
        {
            if (Yrotat > -360)
            {

                Yrotat -= 2;
            }
        }
        if (Input.GetAxis("Mouse Y") > 0.1)
        {
            if (Xrotat > -180)
            {

                Xrotat -= 2;
            }
            //Debug.Log("Mouse moved Up");
        }
        if (Input.GetAxis("Mouse Y") < -0.1)
        {
            if (Xrotat < 180)
            {

                Xrotat += 2;
            }
            //Debug.Log("Mouse moved down");
        }
        if (PitchLeft.IsPressed())
        {
            if (Zrotat < 180)
            {

                Zrotat += 2;
            }
        }
        if (PitchRight.IsPressed())
        {
            if (Zrotat > -180)
            {

                Zrotat -= 2;
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
}
