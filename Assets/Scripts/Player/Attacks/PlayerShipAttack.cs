using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerShipAttack : MonoBehaviour
{
    [Header("Input Actions")]
    private InputAction Shoot;
    private InputAction AltFire;
    [Header("Variables")]
    private int NormFOV = 60;
    private int ZoomFOV = 30;
    private bool zooming;
    private float zoomDuration = 0.5f;
    private float timeZooming;
    //
    [Header("Player Components")]
    [SerializeField] private GameObject Camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Shoot = InputSystem.actions.FindAction("Attacks/Laser");
        Shoot.performed += ctx => ShootLaser();
        AltFire = InputSystem.actions.FindAction("Attacks/Zoom");
        AltFire.performed += ctx => Zoom();
        AltFire.canceled += ctx => ZoomCancel();
    }

    // Update is called once per frame
    void Update()
    {
        if (zooming)
        {
            if (timeZooming < zoomDuration)
            {
                Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(NormFOV, ZoomFOV, timeZooming / zoomDuration);
                timeZooming += Time.deltaTime;
                
            }
            //Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(ZoomFOV, NormFOV, 30);
        }
    }
    void ShootLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            //Debug.Log("Hit something");
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                Debug.Log("Hit enemy\nDo damage");
            }
        }
    }
    void Zoom()
    {
        //Camera.GetComponent<Camera>().fieldOfView = ZoomFOV;
        zooming = true;
        timeZooming = 0;
    }
    void ZoomCancel()
    {
        zooming = false;
        Camera.GetComponent<Camera>().fieldOfView = NormFOV;
    }
}
