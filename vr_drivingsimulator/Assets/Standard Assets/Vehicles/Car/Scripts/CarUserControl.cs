using Meta.WitAi.Lib;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private RecvManager m_recvManager;
        [SerializeField] bool isCollisionExperience = false;
        float h;
        float a;
        float b;
        float v;
        float scaledValue_a;
        float scaledValue_b;
        float aute_a;
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_recvManager = GameObject.Find("Scripts").GetComponent<RecvManager>();
        }


        private void FixedUpdate()
        {
            //元々はここでコントローラの入力を取得
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //float v = CrossPlatformInputManager.GetAxis("Vertical");

            //UDPで受け取ったデータを代入
            h = m_recvManager.GetStsteerAxis();
            a = (m_recvManager.GetStapedalAxis());
            b = (m_recvManager.GetStbpedalAxis());

            CalcScaledValue();

            if (isCollisionExperience)
            {
                aute_a += 0.1f * Time.deltaTime;
                if (aute_a > 0.4f)
                {
                    aute_a = 0.4f;
                }
                v = aute_a - scaledValue_b;
                Debug.Log(v + "_b:" + scaledValue_b + "_a:" + aute_a);
                m_Car.Move(h, v, v, 0f);
                return;
            }

            v = scaledValue_a - scaledValue_b;
            //Debug.Log(scaledValue_b);

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }

        void CalcScaledValue()
        {
            float min = 0;
            float max = 0.2f;
            float originalValue_a = a;
            if (originalValue_a >= 0.9f)
            {
                scaledValue_a += 0.2f * Time.deltaTime;
            }
            else
            {
                scaledValue_a = (originalValue_a * (max - min)) + min;
            }
            if (scaledValue_a >= 1)
            {
                scaledValue_a = 1;
            }

            float originalValue_b = b;
            if (originalValue_b >= 0.9f)
            {
                scaledValue_b += 0 * Time.deltaTime;
            }
            else
            {
                scaledValue_b = (originalValue_b * (max - min)) + min;
            }
            if (scaledValue_b >= 1)
            {
                scaledValue_b = 1;
            }

        }
    }

}
