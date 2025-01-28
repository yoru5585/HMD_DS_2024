using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColisionCar : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("Scripts").GetComponent<CollisionExperienceManager>().SetIsOtherCarStart(false);
            GameObject.Find("Scripts").GetComponent<CollisionExperienceManager>().SetIsEnd(true);
        }
    }
}
