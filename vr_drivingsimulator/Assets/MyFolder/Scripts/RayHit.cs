using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHit : MonoBehaviour
{
    //�ꎞ��~�K���ӏ��ō��E�̊m�F�����Ă��邩�ǂ����𔻒肷��
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
