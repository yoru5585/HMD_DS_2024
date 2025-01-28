using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;
using System;
using System.Net.Sockets;
using System.Net;
using Unity.VisualScripting;
using TMPro;

public class SaveTransform : MonoBehaviour
{
    //csvの書き出し
    //https://qiita.com/tak001/items/e69029e9246d80d1279b

    [SerializeField] float saveInterval = 0.5f;
    [SerializeField] Transform carTrans = null;
    [SerializeField] Transform cameraTrans = null;
    [SerializeField] bool endFlag = false;
    [SerializeField] TextMeshProUGUI pathText;
    [SerializeField] Text test; 
    int index = -1;

    public List<string> saveStrList = new List<string>();

    StreamWriter sw1;
    

    // Start is called before the first frame update
    void Start()
    {
        if (carTrans == null || cameraTrans == null)
        {
            Debug.Log("error");
            return;
        }

        // 新しくcsvファイルを作成して、{}の中の要素分csvに追記をする
        string formattedDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");   // ファイル名用の日付フォーマット
        //string path = Path.Combine(Application.persistentDataPath, $"{formattedDate}-TransformData.csv");
        
        //Debug.Log(path);
        
        //sw1 = new StreamWriter((pathText.text), false, Encoding.GetEncoding("Shift_JIS"));

        string FilePath = @"/"+formattedDate+"-TransformData.csv.csv";
        sw1 = new StreamWriter(Application.persistentDataPath + FilePath, false, Encoding.GetEncoding("utf-8"));
        pathText.text = sw1.ToString();

        StartCoroutine(SaveRoutine());
    }

    IEnumerator SaveRoutine()
    {
        //float time = 0;
        while (true)
        {
            if (endFlag)
            {
                foreach (var data in saveStrList)
                {
                    Debug.Log(data);
                    test.text += " "+data;
                }
                if (test.text.Equals(""))
                {
                    test.text = "no data";
                }
                //time += Time.deltaTime;
                try
                {
                    foreach (var data in saveStrList)
                    {
                        
                        sw1.WriteLine(data);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError (e);
                }
                
                //ConnectToPython(saveStrList); //tcpで送ろうとした残骸　→　結局送るようになりました
                Debug.Log($"writeLine_end");
                //saveStrList.Clear();
                sw1.Close();
                break;
            }

            //格納データ　car( position x, y, z  rotation x, y, z, w ) camera( position x, y, z  rotation x, y, z, w ) 
            SaveTransformData();

            // 次のキャプチャまで待機
            yield return new WaitForSeconds(saveInterval);
        }
    }

    void SaveTransformData()
    {
        //car
        Vector3 carPos = carTrans.position;
        Quaternion carRot = carTrans.rotation;

        //camera
        Vector3 cameraPos = cameraTrans.position;
        Quaternion cameraRot = cameraTrans.localRotation;

        float[] s1 = { carPos.x, carPos.y, carPos.z, carRot.x, carRot.y, carRot.z, carRot.w , 
            cameraPos.x, cameraPos.y, cameraPos.z, cameraRot.x, cameraRot.y, cameraRot.z, cameraRot.w };
        string s2 = string.Join(",", s1);
        //Debug.Log(s2);
        saveStrList.Add(s2);
        index += 1;
    }

    public void SetEndFlag(bool b)
    {
        endFlag = b;
    }

    public int GetIndex()
    {
        return index;
    }

    //https://note.com/takataok/n/n09e46ef47475
    public void ConnectToPython(List<string> strList)
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
            saveStrList.AddRange(strList);
            SendStringsIndividually(stream, saveStrList);

            // 接続を閉じる
            client.Close();
        }
        catch (SocketException e)
        {
            UnityEngine.Debug.LogError("SocketException: " + e);
        }
    }

    void SendData(NetworkStream stream, string message)
    {
        Byte[] data = Encoding.ASCII.GetBytes(message);

        // データを送信
        stream.Write(data, 0, data.Length);
        UnityEngine.Debug.Log("データ送信: " + message);
    }

    bool ReceiveData(NetworkStream stream)
    {
        // サーバーからの応答を受け取るバッファを作成
        Byte[] data = new Byte[256];
        String responseData = String.Empty;

        // サーバーからデータを読み込む
        Int32 bytes = stream.Read(data, 0, data.Length);

        if (bytes == 0)
        {
            return false;
        }

        // 読み込んだデータを文字列に変換
        responseData = Encoding.ASCII.GetString(data, 0, bytes);
        UnityEngine.Debug.Log("サーバーからの応答: " + responseData);
        return true;
    }

    void SendStringsIndividually(NetworkStream stream, List<string> strList)
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
