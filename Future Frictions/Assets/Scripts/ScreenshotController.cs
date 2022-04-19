using System;
using System.IO;
using UnityEngine;

public class ScreenshotController : MonoBehaviour
{
    public int resWidth = 1920;
    public int resHeight = 1080;

    [SerializeField]
    private Camera screenshotCamera;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", Application.persistentDataPath, width, height, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.K))
    //     {
    //         TakeScreenhotAndStore();
    //     }
    // }

    public byte[] TakeScreenshot()
    {
        screenshotCamera.gameObject.SetActive(true);

        SetBilloardTargets(screenshotCamera.transform);

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        screenshotCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();

        screenshotCamera.gameObject.SetActive(false);

        SetBilloardTargets(Camera.main.transform);

        return bytes;
    }

    public void TakeScreenhotAndStore()
    {
        var bytes = TakeScreenshot();

        string filename = ScreenShotName(resWidth, resHeight);

        var dirPath = Path.Combine(Application.persistentDataPath, "screenshots");
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }

    private void SetBilloardTargets(Transform target)
    {
        var billies = FindObjectsOfType<Billboard>();

        foreach (var b in billies)
        {
            b.SetTarget(target);
        }
    }
}