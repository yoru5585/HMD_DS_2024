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
    //csv�̏����o��
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

        // �V����csv�t�@�C�����쐬���āA{}�̒��̗v�f��csv�ɒǋL������
        string formattedDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");   // �t�@�C�����p�̓��t�t�H�[�}�b�g
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
                
                //ConnectToPython(saveStrList); //tcp�ő��낤�Ƃ����c�[�@���@���Ǒ���悤�ɂȂ�܂���
                Debug.Log($"writeLine_end");
                //saveStrList.Clear();
                sw1.Close();
                break;
            }

            //�i�[�f�[�^�@car( position x, y, z  rotation x, y, z, w ) camera( position x, y, z  rotation x, y, z, w ) 
            SaveTransformData();

            // ���̃L���v�`���܂őҋ@
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
            saveStrList.AddRange(strList);
            SendStringsIndividually(stream, saveStrList);

            // �ڑ������
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

        // �f�[�^�𑗐M
        stream.Write(data, 0, data.Length);
        UnityEngine.Debug.Log("�f�[�^���M: " + message);
    }

    bool ReceiveData(NetworkStream stream)
    {
        // �T�[�o�[����̉������󂯎��o�b�t�@���쐬
        Byte[] data = new Byte[256];
        String responseData = String.Empty;

        // �T�[�o�[����f�[�^��ǂݍ���
        Int32 bytes = stream.Read(data, 0, data.Length);

        if (bytes == 0)
        {
            return false;
        }

        // �ǂݍ��񂾃f�[�^�𕶎���ɕϊ�
        responseData = Encoding.ASCII.GetString(data, 0, bytes);
        UnityEngine.Debug.Log("�T�[�o�[����̉���: " + responseData);
        return true;
    }

    void SendStringsIndividually(NetworkStream stream, List<string> strList)
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
