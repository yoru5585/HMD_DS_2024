using ForUDP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class CollisionExperienceManager : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] Rigidbody car_rb;
    [SerializeField] Rigidbody car_rb_other;
    [SerializeField] GameObject endCanvas;
    [SerializeField] GameObject startCanvas;

    [SerializeField] GameObject[] slides;
    [SerializeField] GameObject[] buttons;
    int index = 0;

    RecvManager recvManager;
    bool IsBeforeStart = true;
    bool IsOtherCarStart = false;
    bool IsEnd = false;
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
                GameObject.Find("MyCar").GetComponent<CarController>().Cgear = 1;
                IsBeforeStart = false;
            }
            return;
        }

        if (IsOtherCarStart)
        {
            autoMove(car_rb_other, 40);
            return;
        }

        if (IsEnd)
        {
            car_rb.constraints = RigidbodyConstraints.FreezePositionZ;
            car_rb_other.velocity = Vector3.zero;
            GameObject.Find("MyCar").GetComponent<CarAudio>().enabled = false;
            //����p��ʂ�\��
            text.SetActive(true);
            endCanvas.SetActive(true);
            startCanvas.SetActive(false);
        }
        
    }

    void autoMove(Rigidbody g , float speed)
    {
        //Debug.Log("a");
        Vector3 t = new Vector3(-speed, 0, 0);
        g.velocity = t;
        //Debug.Log($"Rigidbody Velocity: {g.velocity}");
        //g.AddForce(t);
    }

    public void SetIsOtherCarStart(bool b)
    {
        IsOtherCarStart = b;
    }

    public void SetIsEnd(bool b)
    {
        IsEnd = b;
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSlide(int n)
    {
        if (slides == null || slides.Length == 0) return;

        // �S�X���C�h���\���ɂ���
        foreach (GameObject s in slides)
        {
            if (s != null) s.SetActive(false);
        }

        // �{�^�������Z�b�g
        buttons[1].SetActive(true); // �O��
        buttons[0].SetActive(true); // ����

        // �C���f�b�N�X�X�V�Ɣ͈͓��ɐ���
        index += n;
        index = Mathf.Clamp(index, 0, slides.Length - 1);

        // �{�^���̕\������
        if (index == 0) buttons[1].SetActive(false);
        if (index == slides.Length - 1) buttons[0].SetActive(false);

        // ���݂̃X���C�h��\��
        if (slides[index] != null)
        {
            slides[index].SetActive(true);
        }
        //foreach (GameObject s in slides)
        //{
        //    s.SetActive(false);
        //}
        //buttons[0].SetActive(true);
        //buttons[1].SetActive(true);
        //index += n;
        //if (index == 0)
        //{
        //    buttons[0].SetActive(false);
        //}
        //else if (index == slides.Length - 1)
        //{
        //    buttons[1].SetActive(false);
        //}

        //slides[index].SetActive(true);
    }
}
