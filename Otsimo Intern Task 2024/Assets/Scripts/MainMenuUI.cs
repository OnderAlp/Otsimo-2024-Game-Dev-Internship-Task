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

        string[] files = System.IO.Directory.GetFiles(galleryPath, "*.png"); // .jpg uzant�l� dosyalar� al
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

        // Platforma g�re i�lem yap
#if UNITY_ANDROID && !UNITY_EDITOR
            // Android'de galeri yolu
            path = "/storage/emulated/0/DCIM/Myapp Pictures"; // �rnek bir yol, ger�ek yolu cihazda bulman�z gerekecek
#elif UNITY_IOS && !UNITY_EDITOR
            // iOS'ta galeri yolu
            path = Application.persistentDataPath + "/../../Gallery";
#else
        // Di�er platformlar i�in varsay�lan
        path = Application.persistentDataPath;
#endif

        return path;
    }
}