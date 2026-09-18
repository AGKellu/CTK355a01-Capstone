using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RadiusAttackScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int cooldown;
    private bool onCoolDown;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject Shooter = transform.parent.gameObject;
            if (Shooter.name.Contains("Buzzer"))
            {
                if (!onCoolDown)
                {

                    Shooter.GetComponent<BuzzerScript>().Shoot();
                    StartCoroutine(Wait(cooldown));
                    Debug.Log("PPLLLEASSE");
                    onCoolDown = true;
                }
            }
            else if (Shooter.name == "Zarling")
            {

            }
        }
    }
    private IEnumerator Wait(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        onCoolDown = false;
    }
}
