using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHit : MonoBehaviour
{
    //一時停止におけるA地点（停止線の前）、B地点（見渡せる位置）で
    //停止しているか判定する
    //車体前後にコライダーを設置する
    //このスクリプトは一つの停止箇所で４つ使用する
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
