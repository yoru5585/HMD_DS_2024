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
    //�]�����ʂ�\������
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
        string formattedDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");  // �t�@�C�����p�̓��t�t�H�[�}�b�g
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
            //Debug.Log($"{buttonList[i].name}�F{i}");
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
            textList[num].text += $"���|�C���g{num + 1}�F ���E�̈��S�m�F�F{GetStrIsSuccess(lookEva.isSuccess)}\n";
            textList[num].text += $"��i�K�ڒ�~�F{GetStrIsSuccess(stopEvaArray[0].isSuccess)}�@�b���F{stopEvaArray[0].stopTime.ToString("f2")}�b  " +
                $"��i�K�ڒ�~�F{GetStrIsSuccess(stopEvaArray[1].isSuccess)} �@�b���F {stopEvaArray[1].stopTime.ToString("f2")}�b\n";
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

            //���E�m�F�AA�n�_�A�b���AB�n�_�A�b��
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
            return "�Z";
        }
        else
        {
            return "�~";
        }
    }

    public void ConnectToPython()
    {
        // �T�[�o�[��IP�A�h���X�ƃ|�[�g��ݒ� (localhost �̏ꍇ)
        string serverIP = "192.168.50.206";// "133.17.163.33"; // �T�[�o�[��IP�A�h���X
        //string serverIP = "133.17.163.33";// "133.17.163.33"; // �T�[�o�[��IP�A�h���X
        int port = 5000; // �T�[�o�[���҂��󂯂Ă���|�[�g

        try
        {
            // TcpClient���쐬���A�T�[�o�[�ɐڑ�
            TcpClient client = new TcpClient(serverIP, port);
            UnityEngine.Debug.Log("�T�[�o�[�ɐڑ����܂���");

            // �T�[�o�[�Ƃ̃f�[�^�����p�X�g���[�����擾
            NetworkStream stream = client.GetStream();

            // �T�[�o�[�Ƀf�[�^�𑗐M
            // ������z��𑗐M
            SendStringList(stream, EvaStrList);

            // �ڑ������
            client.Close();
        }
        catch (SocketException e)
        {
            UnityEngine.Debug.LogError("SocketException: " + e);
        }
    }

    void SendStringList(NetworkStream stream, List<string> strList)
    {
        // ���M����string�^�̔z��
        string[] stringArray = strList.ToArray();

        // �e��������ʂɑ��M
        foreach (string str in stringArray)
        {
            byte[] data = Encoding.ASCII.GetBytes(str);
            stream.Write(data, 0, data.Length);
            UnityEngine.Debug.Log("���M����������: " + str);

            // �����񂲂Ƃɋ�؂�̂��߂̉��s�𑗐M
            byte[] newline = Encoding.ASCII.GetBytes("\n");
            stream.Write(newline, 0, newline.Length);
        }

        // �I���V�O�i���Ƃ��� "END" �𑗐M
        byte[] endSignal = Encoding.ASCII.GetBytes("END\n");
        stream.Write(endSignal, 0, endSignal.Length);
        UnityEngine.Debug.Log("�I���V�O�i�� 'END' �𑗐M���܂���");
    }
}
