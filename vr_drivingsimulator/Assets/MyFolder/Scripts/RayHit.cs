using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHit : MonoBehaviour
{
    //一時停止規制箇所で左右の確認をしているかどうかを判定する
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
