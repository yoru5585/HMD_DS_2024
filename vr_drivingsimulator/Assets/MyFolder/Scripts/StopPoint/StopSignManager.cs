using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[System.Serializable]
class CheckStep
{
    public CheckHit front;
    public CheckHit back;

    public StopEva eva = new StopEva();

    public bool GetIsThrough()
    {
        if (front.GetIsHit() && back.GetIsHit())
        {
            return true;
        }

        return false;
    }
}

[System.Serializable]
class CheckLook
{
    public RayHit left;
    public RayHit right;

    public LookEva eva = new LookEva();

    public bool GetIsRayHit()
    {
        if (left.GetIsHit() && right.GetIsHit())
        {
            return true;
        }

        return false;
    }
}

public class StopSignManager : MonoBehaviour
{
    //各一時停止箇所の判定
    //判定結果をマネージャーに送る
    [SerializeField] CheckStep A;
    [SerializeField] CheckStep B;
    [SerializeField] CheckLook C;
    

    float time = 0;
    float interval = 3;

    int state = 0;

    void sendDataToManager()
    {
        //マネージャーに判定データを送る
        Debug.Log($"A判定：{ A.eva.isSuccess}　秒数：{A.eva.stopTime}");
        Debug.Log($"B判定：{ B.eva.isSuccess}　秒数：{B.eva.stopTime}");

        GameObject.Find("Scripts").GetComponent<PointManager>().AddList(A.eva, B.eva, C.eva);
    }

    // Update is called once per frame
    void Update()
    {
        float v = (GameObject.Find("MyCar").GetComponent<Rigidbody>().velocity.magnitude * 60 * 60) / 1000;
        //Debug.Log(v);

        //左右確認しているかどうか
        if (state >= 1 && state < 4)
        {
            if (C.GetIsRayHit())
            {
                C.eva.isSuccess = true;
                Debug.Log("C OK");
            }
        }

        switch (state) 
        { 
            case 0:
                //スタート A待機中
                if (A.GetIsThrough())
                {
                    //開始のカウントをチェック
                    GameObject.Find("Scripts").GetComponent<PointManager>().CheckPointIndex();
                    state++;
                }
                break;
            case 1:
                //Aチェック中 
                if (v < 1f)
                {
                    //徐行してなければ時間を計る
                    time += Time.deltaTime;
                    if (time > interval)
                    {
                        A.eva.isSuccess = true;
                        Debug.Log("A OK");
                    }

                }

                if (A.GetIsThrough() != true)
                {
                    //A抜け出す
                    A.eva.stopTime = time;

                    time = 0;
                    interval = 3;
                    state++;
                }
                break;
            case 2:
                //B待機中
                if (B.GetIsThrough())
                {
                    state++;
                }
                break;
            case 3:
                //Bチェック中
                if (v < 1f)
                {
                    //徐行してなければ時間を計る
                    time += Time.deltaTime;
                    if (time > interval)
                    {
                        B.eva.isSuccess = true;
                        Debug.Log("B OK");
                    }

                }

                if (B.GetIsThrough() != true)
                {
                    //B抜け出す
                    B.eva.stopTime = time;
                    state++;
                }
                break;
            case 4:
                //エンド 結果をマネージャーに送る
                sendDataToManager();
                state = 99;
                break;
            default:
                break;
        }
    }
}
