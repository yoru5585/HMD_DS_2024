using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForUDP;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class RecvManager : MonoBehaviour
{
    private UDP_recv commUDP = new UDP_recv();
    [SerializeField] int port_rcv;
    [SerializeField] TextMeshProUGUI logText;

    [SerializeField] bool OfflineMode = false;
    [SerializeField] bool DebugMode = false;

    //float scaledValue;

    //ハンドルの入力値
    //アクセルの入力値
    //ブレーキの入力値

    //Rギア
    //Dギア

    //STARTボタン

    // Start is called before the first frame update
    void Start()
    {
        if (OfflineMode || DebugMode)
        {
            return;
        }

        //commUDP.init(int型の送信用ポート番号, int型の送信先ポート番号, int型の受信用ポート番号);
        commUDP.init(port_rcv);
        commUDP.start_receive();
    }

    private void Update()
    {
        if (DebugMode)
        {
            commUDP.rcv_float_arr[0] = Input.GetAxis("Horizontal");
            commUDP.rcv_float_arr[1] = Input.GetAxis("Vertical");
            commUDP.rcv_float_arr[4] = (Input.GetKey("f") ? 1 : 0);
            commUDP.rcv_float_arr[5] = Input.GetAxis("Horizontal");
            //Debug.Log("_o:"+ originalValue + "_s:" + scaledValue);
        }

        if (OfflineMode)
        {
            float hdl = Input.GetAxis("Horizontal");
            float apdl = Input.GetAxis("apedal");
            float bpdl = Input.GetAxis("bpedal");
            float rgear = (Input.GetKey("joystick button 1") ? 1 : 0);
            float dgear = (Input.GetKey("joystick button 2") ? 1 : 0);
            float start = (Input.GetKey("joystick button 0") ? 1 : 0);
            //Debug.Log(hdl +":"+ pdl + ":" +gear);
            commUDP.rcv_float_arr[0] = hdl;
            commUDP.rcv_float_arr[1] = (apdl+1)/2;
            commUDP.rcv_float_arr[2] = (bpdl+1)/2;
            commUDP.rcv_float_arr[3] = rgear;
            commUDP.rcv_float_arr[4] = dgear;
            commUDP.rcv_float_arr[5] = start;
        }

        receive();
    }

    public void receive()
    {
        //commUDP.start_receive();
        var b = commUDP.rcv_float_arr;
        string recvStr = "";

        for (int i = 0; i < b.Length; i++)
        {
            recvStr += $"{i}:{b[i]}\n";
            //Debug.Log($"{i}:{b[i]}");
            //valueSlider.value = b[i];
        }

        logText.text = recvStr;
    }

    public float[] GetRecvValue()
    {
        return commUDP.rcv_float_arr;
    }

    public float GetStsteerAxis()
    {
        return commUDP.rcv_float_arr[0];
    }

    public float GetStapedalAxis()
    {
        return commUDP.rcv_float_arr[1];
    }
    
    public float GetStbpedalAxis()
    {
        return commUDP.rcv_float_arr[2];
    }

    public float GetRgear()
    {
        return commUDP.rcv_float_arr[3];
    }

    public float GetDgear()
    {
        return commUDP.rcv_float_arr[4];
    }

    public float GetStartButton()
    {
        return commUDP.rcv_float_arr[5];
    }

    private void OnApplicationQuit()
    {
        commUDP.end();
    }
}
