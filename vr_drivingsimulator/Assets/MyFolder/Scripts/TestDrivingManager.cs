using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestDrivingManager : MonoBehaviour
{
    [SerializeField] GameObject text;
    RecvManager recvManager;
    bool IsBeforeStart = true;
    // Start is called before the first frame update
    void Start()
    {
        recvManager = GetComponent<RecvManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBeforeStart)
        {
            if (recvManager.GetStbpedalAxis() > 0.9f)
            {
                text.SetActive(false);
                IsBeforeStart = false;
            }
            return;
        }
        
    }

    public void OnEndButtonClicked()
    {
        //ÉVÅ[Éìà⁄ìÆ
        SceneManager.LoadScene("TitleScene");
    }
}
