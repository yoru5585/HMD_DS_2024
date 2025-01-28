using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCarStart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("Scripts").GetComponent<CollisionExperienceManager>().SetIsOtherCarStart(true);
        }
    }
}
