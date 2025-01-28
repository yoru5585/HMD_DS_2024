using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class GameManager : MonoBehaviour
{
    //ëSëÃÇÃó¨ÇÍÇä«óùÇ∑ÇÈ
    [SerializeField] GameObject goalObj;
    [SerializeField] GameObject myCar;
    [SerializeField] GameObject mainCamera;
    
    [SerializeField] GameObject afterGoalObj;
    [SerializeField] GameObject startObj;
    PointManager pointManager;
    ResultManager resultManager;
    RecvManager recvManager;
    SaveTransform saveTransform;
    PlayerRay playerRay;
    
    bool IsBeforeStart = true;
    bool IsGameEnd = false;
    bool IsGameWait = false;

    // Start is called before the first frame update
    void Start()
    {
        pointManager = GetComponent<PointManager>();
        resultManager = GetComponent<ResultManager>();
        recvManager = GetComponent<RecvManager>();
        saveTransform = GetComponent<SaveTransform>();
        playerRay = GetComponent<PlayerRay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameWait)
        {
            return;
        }

        if (IsBeforeStart)
        {
            if (recvManager.GetStbpedalAxis() > 0.9f)
            {
                startObj.SetActive(false);
                playerRay.SetIsRay(true);
                IsBeforeStart = false;
            }
            return;
        }

        if (IsGameEnd)
        {
            if (recvManager.GetStartButton() > 0.9f)
            {
                MovePlayerCameraPosition();
                resultManager.ShowResult();
                resultManager.SaveEvalutionDataToCSV();
                resultManager.ConnectToPython();
                playerRay.SetIsRay(false);
                IsGameEnd = false;
                IsGameWait = true;
            }

            return;
        }

        

        //É|ÉCÉìÉgÇÇ∑Ç◊Çƒí âﬂÇµÇΩÇ©
        if (pointManager.GetLength() >= pointManager.GetMaxLen())
        {
            //ÉSÅ[ÉãÇ…êGÇÍÇΩÇ©
            if (goalObj.GetComponent<Goal>().GetIsHit())
            {
                afterGoalObj.SetActive(true);
                
                IsGameEnd = true;
                GamePause();
            }
        }
    }

    void GamePause()
    {
        //é‘ÇÃìÆÇ´Ç»Ç«Çé~ÇﬂÇÈ
        myCar.GetComponent<Rigidbody>().isKinematic = true;
    }

    void MovePlayerCameraPosition()
    {
        mainCamera.transform.parent = GameObject.Find("Result").transform;
        mainCamera.transform.localPosition = new Vector3(1.03f, 10.0f, -1.17f);
        mainCamera.transform.rotation = Quaternion.identity;
        //mainCamera.transform.localPosition = new Vector3(-2.2f, 2.0f, -1.9f);
    }

    public void OnEndButtonClicked()
    {
        if (IsGameWait)
        {
            SceneManager.LoadScene("TitleScene");
        }
        
    }

    void invokeMethod()
    {
        saveTransform.ConnectToPython(resultManager.EvaStrList);
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
