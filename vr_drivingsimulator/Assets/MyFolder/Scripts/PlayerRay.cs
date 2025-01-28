using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] Camera cam;             // �J����
    [SerializeField] float distance = 20f;    // ���o�\�ȋ���
    bool IsRay = false;

    // Update is called once per frame
    void Update()
    {
        if (IsRay)
        {
            RayStart();
        }
    }

    void RayStart()
    {
        // Ray�̓J�����̈ʒu����Ƃ΂�
        var rayStartPosition = cam.transform.position;
        // Ray�̓J�����������Ă�����ɂƂ΂�
        var rayDirection = cam.transform.forward.normalized;

        // Hit�����I�u�W�F�N�g�i�[�p
        //RaycastHit raycastHit;

        //// Ray���΂��iout raycastHit ��Hit�����I�u�W�F�N�g���擾����j
        //var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);

        //// Debug.DrawRay (Vector3 start(ray���J�n����ʒu), Vector3 dir(ray�̕����ƒ���), Color color(���C���̐F));
        //Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        //// �Ȃɂ������o������
        //if (isHit)
        //{
        //    // Log��Hit�����I�u�W�F�N�g�����o��
        //    Debug.Log("HitObject : " + raycastHit.collider.gameObject.name);

        //    RayHit rayhit = raycastHit.collider.gameObject.GetComponent<RayHit>();
        //    if (rayhit != null)
        //    {
        //        rayhit.OnHit();

        //    }
        //}

        // RaycastAll�Ńq�b�g�����S�ẴI�u�W�F�N�g���擾
        RaycastHit[] hits = Physics.RaycastAll(rayStartPosition, rayDirection);

        // �q�b�g�����I�u�W�F�N�g�����邩�ǂ������m�F
        if (hits.Length > 0)
        {

            // �q�b�g�����S�ẴI�u�W�F�N�g�ɑ΂��ď������s��
            foreach (RaycastHit hit in hits)
            {
                RayHit rayhit = hit.collider.gameObject.GetComponent<RayHit>();
                if (rayhit != null)
                {
                    rayhit.OnHit();

                }
            }
        }
    }

    public void SetIsRay(bool b)
    {
        IsRay = b;
    }
}