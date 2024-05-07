using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.IO;
using System;


public class MainMenuUI : MonoBehaviour
{
    public TextMeshProUGUI _startButtonText;

    private string galleryPath;

    void Start()
    {
        galleryPath = GetGalleryPath();

        string[] files = System.IO.Directory.GetFiles(galleryPath, "*.png");
        if (files.Length > 0)
        {
            _startButtonText.text = "Continue";
        }
        else
        {
            _startButtonText.text = "Start";
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    string GetGalleryPath()
    {
        string path = "";

#if UNITY_ANDROID && !UNITY_EDITOR

            path = "/storage/emulated/0/DCIM/Myapp Pictures"; // Örnek bir yol, gerçek yolu cihazda bulmanýz gerekecek
#elif UNITY_IOS && !UNITY_EDITOR

            path = Application.persistentDataPath + "/../../Gallery";
#else

        path = Application.persistentDataPath;
#endif

        return path;
    }
}
