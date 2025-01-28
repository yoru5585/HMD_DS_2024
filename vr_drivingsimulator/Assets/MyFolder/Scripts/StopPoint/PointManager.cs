using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

//ï]âøåãâ 
public class StopEva : Evalution
{
    [System.NonSerialized] public bool isSuccess = false;
    [System.NonSerialized] public float stopTime = 0;
}

public class LookEva : Evalution
{
    [System.NonSerialized] public bool isSuccess = false;
}

public interface Evalution { }

public class PointManager : MonoBehaviour
{
    //àÍéûí‚é~â”èäÇ≈ÇÃï]âøÇÇ‹Ç∆ÇﬂÇÈ
    private List<Evalution[]> evaluationList = new List<Evalution[]>();
    [SerializeField] private List<int> pointIndexList = new List<int>();
    [SerializeField] private int maxLen = 2;
    private int len = 0;

    public void AddList(StopEva eva_A, StopEva eva_B, LookEva eva_C)
    {
        Evalution[] tmp = { eva_A, eva_B, eva_C};
        evaluationList.Add(tmp);
        len++;
    }

    public void CheckPointIndex()
    {
        pointIndexList.Add(GetComponent<SaveTransform>().GetIndex());
    }

    public List<Evalution[]> GetList()
    {
        return evaluationList;
    }

    public List<int> GetIndexList() 
    {
        return pointIndexList;
    }

    public int GetLength()
    {
        return len;
    }

    public int GetMaxLen()
    {
        return maxLen;
    }
}
