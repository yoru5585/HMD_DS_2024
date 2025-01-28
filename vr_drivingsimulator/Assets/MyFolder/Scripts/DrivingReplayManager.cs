using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DrivingReplayManager : MonoBehaviour
{
    // CSVデータを保存するためのリスト
    List<string[]> csvData = new List<string[]>();
    [SerializeField] float moveInterval = 0.5f;
    [SerializeField] Transform carTrans;
    [SerializeField] Transform cameraTrans;
    [SerializeField] bool endFlag = true;
    [SerializeField] bool pauseFlag = true;
    [SerializeField] Slider replaySlider;
    [SerializeField] int index = 0;
    float time = 0;

    void Update()
    {
        if (endFlag)
        {
            return;
        }

        SetTransform(csvData[index]);
        ChangeSlider();
        time += Time.deltaTime;
        
        if (time > moveInterval)
        {
            time = 0;
            if (pauseFlag)
            {
                return;
            }

            index += 1;
            if (index >= csvData.Count)
            {
                index = 0;
                replaySlider.value = 0;
                endFlag = true;
            }
        }
    }

    public void Setup()
    {
        csvData.Clear();
        //LoadCSV("TransformData");
        LoadData();

        replaySlider.maxValue = csvData.Count - 1;

        endFlag = false;

        //StartCoroutine(Replay());
    }

    void LoadData()
    {
        foreach (string line in GetComponent<SaveTransform>().saveStrList)
        {
            string[] fields = line.Split(',');
            csvData.Add(fields);
        }
        
        
    }

    void LoadCSV(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".csv");
        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSVファイルが見つかりません");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            string[] fields = line.Split(',');
            csvData.Add(fields);
        }

        Debug.Log("データ数：" + csvData.Count);
    }


    //IEnumerator Replay()
    //{
    //    while (true)
    //    {
    //        if (endFlag)
    //        {
    //            break;
    //        }

    //        SetTransform(csvData[index]);

    //        if (pauseFlag)
    //        {
    //            continue;
    //        }

    //        index += 1;

    //        yield return new WaitForSeconds(moveInterval);

    //    }
    //}

    void SetTransform(string[] data)
    {
        float[] floatData = data
                  .Select(float.Parse)
                  .ToArray();

        carTrans.position = new Vector3(floatData[0], floatData[1], floatData[2]);
        carTrans.rotation = new Quaternion(floatData[3], floatData[4], floatData[5], floatData[6]);

        cameraTrans.position = new Vector3(floatData[7], floatData[8], floatData[9]);
        cameraTrans.localRotation = new Quaternion(floatData[10], floatData[11], floatData[12], floatData[13]);
    }

    void ChangeSlider()
    {
        if (pauseFlag)
        {
            index = (int)(replaySlider.value);
            return;
        }

        replaySlider.value = index;
    }

    public void StopStartButton()
    {
        endFlag = false;
        if (pauseFlag)
        {
            pauseFlag = false;
        }
        else
        {
            pauseFlag = true;
        }
    }

    public void SetIndex(int i)
    {
        replaySlider.value = i;
    }
}
