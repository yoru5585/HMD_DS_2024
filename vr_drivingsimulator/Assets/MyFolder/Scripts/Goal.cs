     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //ÉSÅ[Éãóp
    bool isHit = false;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{gameObject.name}:{other.name}");
        if (other.gameObject.tag == "Player")
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isHit = false;
        }
    }

    public bool GetIsHit()
    {
        //Debug.Log($"{gameObject.name}:{isHit}");
        return isHit;
    }
}
