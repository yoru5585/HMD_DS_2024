using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] Camera cam;             // カメラ
    [SerializeField] float distance = 20f;    // 検出可能な距離
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
        // Rayはカメラの位置からとばす
        var rayStartPosition = cam.transform.position;
        // Rayはカメラが向いてる方向にとばす
        var rayDirection = cam.transform.forward.normalized;

        // Hitしたオブジェクト格納用
        //RaycastHit raycastHit;

        //// Rayを飛ばす（out raycastHit でHitしたオブジェクトを取得する）
        //var isHit = Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance);

        //// Debug.DrawRay (Vector3 start(rayを開始する位置), Vector3 dir(rayの方向と長さ), Color color(ラインの色));
        //Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

        //// なにかを検出したら
        //if (isHit)
        //{
        //    // LogにHitしたオブジェクト名を出力
        //    Debug.Log("HitObject : " + raycastHit.collider.gameObject.name);

        //    RayHit rayhit = raycastHit.collider.gameObject.GetComponent<RayHit>();
        //    if (rayhit != null)
        //    {
        //        rayhit.OnHit();

        //    }
        //}

        // RaycastAllでヒットした全てのオブジェクトを取得
        RaycastHit[] hits = Physics.RaycastAll(rayStartPosition, rayDirection);

        // ヒットしたオブジェクトがあるかどうかを確認
        if (hits.Length > 0)
        {

            // ヒットした全てのオブジェクトに対して処理を行う
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