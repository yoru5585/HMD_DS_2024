using System;
using UnityEngine;
using System.IO;
using System.Collections;

public class ContinuousCaptureAndSetTexture : MonoBehaviour
{
    [SerializeField] Camera Camera;
    public float captureInterval = 1.0f; // キャプチャ間隔（秒）
    private Texture2D texture;

    void Start()
    {
        // 初期テクスチャの生成
        texture = new Texture2D(1024, 512);
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = texture;
        }

        // キャプチャのコルーチンを開始
        StartCoroutine(CaptureRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            Debug.Log("ssss");
            AnyCapture();
        }
    }

    IEnumerator CaptureRoutine()
    {
        while (true)
        {
            // キャプチャを非同期で実行
            I360Render.CaptureAsync(OnCaptureComplete, 1024, true, Camera, true);

            // 次のキャプチャまで待機
            yield return new WaitForSeconds(captureInterval);
        }
    }

    void OnCaptureComplete(byte[] screenshotBytes)
    {
        /*
        // 保存先のパスを設定
        string filePath = "Assets/MyFolder/screenshot1.jpg";

        // バイト配列をファイルに書き込む
        File.WriteAllBytes(filePath, screenshotBytes);
        */

        // バイト配列からテクスチャを更新
        texture.LoadImage(screenshotBytes);

        // テクスチャを水平反転
        FlipTextureHorizontally(texture);

        Debug.Log("マテリアルのテクスチャが更新されました");
    }

    void FlipTextureHorizontally(Texture2D original)
    {
        int width = original.width;
        int height = original.height;

        // 水平反転処理
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width / 2; x++)
            {
                // 左側のピクセルと右側のピクセルを入れ替え
                Color leftPixel = original.GetPixel(x, y);
                Color rightPixel = original.GetPixel(width - 1 - x, y);

                original.SetPixel(x, y, rightPixel);
                original.SetPixel(width - 1 - x, y, leftPixel);
            }
        }

        // 変更を適用
        original.Apply();
    }

    void AnyCapture()
    {
        // 360°スクリーンショットをキャプチャ
        byte[] screenshotBytes = I360Render.Capture(1024, true, Camera, true);

        // 保存先のパスを設定
        string filePath = "Assets/MyFolder/screenshot.jpg";

        // バイト配列をファイルに書き込む
        File.WriteAllBytes(filePath, screenshotBytes);

        Debug.Log("360°スクリーンショットが保存されました: " + filePath);
    }
}
