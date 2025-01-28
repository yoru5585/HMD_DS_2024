using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHit : MonoBehaviour
{
    //ˆê’â~‹K§‰ÓŠ‚Å¶‰E‚ÌŠm”F‚ğ‚µ‚Ä‚¢‚é‚©‚Ç‚¤‚©‚ğ”»’è‚·‚é
    bool isHit = false;
    public void OnHit()
    {
        isHit = true;
        Debug.Log("C hit");
    }
    public bool GetIsHit()
    {
        //Debug.Log($"{gameObject.name}:{isHit}");
        return isHit;
    }
}
