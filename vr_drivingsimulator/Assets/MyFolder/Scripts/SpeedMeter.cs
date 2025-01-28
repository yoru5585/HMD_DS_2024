using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] GameObject myCar;
    [SerializeField] Text speedText;

    // Update is called once per frame
    void Update()
    {
        float v = myCar.GetComponent<Rigidbody>().velocity.magnitude;
        speedText.text = "����" + (int)(v * 60 * 60 / 1000) + "km";
        //speedText.text = "����10km";
    }
}
