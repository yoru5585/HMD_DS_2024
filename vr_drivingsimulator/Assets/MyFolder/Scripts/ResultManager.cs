using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using System.Net.Sockets;

public class ResultManager : MonoBehaviour
{
    //評価結果を表示する
    [SerializeField] List<Button> buttonList = new List<Button>();
    public List<string> EvaStrList = new List<string>();
    List<Text> textList = new List<Text>();
    PointManager pointManager;
    DrivingReplayManager drivingReplayManager;
    SaveTransform saveTransform;

    StreamWriter sw2;
    private void Start()
    {
        pointManager = GetComponent<PointManager>();
        drivingReplayManager = GetComponent<DrivingReplayManager>();
        saveTransform = GetComponent<SaveTransform>();

        foreach (var button in buttonList)
        {
            textList.Add(button.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>());
        }
        string formattedDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");  // ファイル名用の日付フォーマット
        //string path = Path.Combine(Application.persistentDataPath, $"{formattedDate}-EvalutionData.csv");
        //sw2 = new StreamWriter((path), false, Encoding.GetEncoding("Shift_JIS"));

        string FilePath = @"/" + formattedDate + "-EvalutionData.csv.csv";
        sw2 = new StreamWriter(Application.persistentDataPath + FilePath, false, Encoding.GetEncoding("utf-8"));

    }

    public void ShowResult()
    {
        
        //Debug.Log(pointManager.GetIndexList().Count);
        for (int i = 0; i < pointManager.GetIndexList().Count; i++)
        {
            //Debug.Log($"{buttonList[i].name}：{i}");
            int tmp = pointManager.GetIndexList()[i];
            buttonList[i].onClick.AddListener(() => 
                {
                    drivingReplayManager.SetIndex(tmp);
                }
            );
        }

        int num = 0;
        foreach (Evalution[] evaArray in pointManager.GetList())
        {
            StopEva[] stopEvaArray = { (StopEva)evaArray[0], (StopEva)evaArray[1] }; 
            LookEva lookEva = (LookEva)evaArray[2];

            textList[num].text = "";
            textList[num].text += $"◆ポイント{num + 1}： 左右の安全確認：{GetStrIsSuccess(lookEva.isSuccess)}\n";
            textList[num].text += $"一段階目停止：{GetStrIsSuccess(stopEvaArray[0].isSuccess)}　秒数：{stopEvaArray[0].stopTime.ToString("f2")}秒  " +
                $"二段階目停止：{GetStrIsSuccess(stopEvaArray[1].isSuccess)} 　秒数： {stopEvaArray[1].stopTime.ToString("f2")}秒\n";
            num++;
        }

        Invoke("ReplaySetup", 1.0f);
        saveTransform.SetEndFlag(true);
        
    }

    public void SaveEvalutionDataToCSV()
    {
        foreach (Evalution[] evaArray in pointManager.GetList())
        {
            StopEva[] stopEvaArray = { (StopEva)evaArray[0], (StopEva)evaArray[1] };
            LookEva lookEva = (LookEva)evaArray[2];

            //左右確認、A地点、秒数、B地点、秒数
            string[] s1 = { lookEva.isSuccess.ToString(), stopEvaArray[0].isSuccess.ToString(), stopEvaArray[0].stopTime.ToString("f2"), 
                stopEvaArray[1].isSuccess.ToString(), stopEvaArray[1].stopTime.ToString("f2") };
            string s2 = string.Join(",", s1);
            EvaStrList.Add(s2);

            sw2.WriteLine(s2);
        }

        Debug.Log($"Eva_writeLine_end");
        sw2.Close();
    }

    void ReplaySetup()
    {
        drivingReplayManager.Setup();
    }

    string GetStrIsSuccess(bool isSuccess)
    {
        if (isSuccess)
        {
            return "〇";
        }
        else
        {
            return "×";
        }
    }

    public void ConnectToPython()
    {
        // サーバーのIPアドレスとポートを設定 (localhost の場合)
        string serverIP = "192.168.50.206";// "133.17.163.33"; // サーバーのIPアドレス
        //string serverIP = "133.17.163.33";// "133.17.163.33"; // サーバーのIPアドレス
        int port = 5000; // サーバーが待ち受けているポート

        try
        {
            // TcpClientを作成し、サーバーに接続
            TcpClient client = new TcpClient(serverIP, port);
            UnityEngine.Debug.Log("サーバーに接続しました");

            // サーバーとのデータやり取り用ストリームを取得
            NetworkStream stream = client.GetStream();

            // サーバーにデータを送信
            // 文字列配列を送信
            SendStringList(stream, EvaStrList);

            // 接続を閉じる
            client.Close();
        }
        catch (SocketException e)
        {
            UnityEngine.Debug.LogError("SocketException: " + e);
        }
    }

    void SendStringList(NetworkStream stream, List<string> strList)
    {
        // 送信するstring型の配列
        string[] stringArray = strList.ToArray();

        // 各文字列を個別に送信
        foreach (string str in stringArray)
        {
            byte[] data = Encoding.ASCII.GetBytes(str);
            stream.Write(data, 0, data.Length);
            UnityEngine.Debug.Log("送信した文字列: " + str);

            // 文字列ごとに区切りのための改行を送信
            byte[] newline = Encoding.ASCII.GetBytes("\n");
            stream.Write(newline, 0, newline.Length);
        }

        // 終了シグナルとして "END" を送信
        byte[] endSignal = Encoding.ASCII.GetBytes("END\n");
        stream.Write(endSignal, 0, endSignal.Length);
        UnityEngine.Debug.Log("終了シグナル 'END' を送信しました");
    }
}
