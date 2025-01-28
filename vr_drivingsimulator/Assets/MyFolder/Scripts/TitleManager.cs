using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey("joystick button 3"))
        {
            SceneManager.LoadScene("CollisionExperienceScene");
        }
    }
    public void SimulatorButton()
    {
        //シーン移動
        SceneManager.LoadScene("SimulatorScene");
    }

    public void TestDrivingButton()
    {
        //シーン移動
        SceneManager.LoadScene("TestDriveScene");
    }
}
