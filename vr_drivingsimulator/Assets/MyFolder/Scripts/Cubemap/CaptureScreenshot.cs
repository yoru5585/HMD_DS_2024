using System;
using System.IO;
using UnityEngine;

public class CaptureScreenshot : MonoBehaviour
{
    [SerializeField] Camera Camera;
    void Start()
    {
        
    }

    private void Update()
    {
        // 360°スクリーンショットをキャプチャ
        byte[] screenshotBytes = I360Render.Capture(2048, true, Camera, true);

        // 保存先のパスを設定
        string filePath = "Assets/MyFolder/screenshot.jpg";

        // バイト配列をファイルに書き込む
        File.WriteAllBytes(filePath, screenshotBytes);

        Debug.Log("360°スクリーンショットが保存されました: " + filePath);

    }
}
