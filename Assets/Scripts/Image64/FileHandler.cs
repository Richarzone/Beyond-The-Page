using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using UnityEditor;
using UnityEngine;

public class FileHandler : MonoBehaviour
{
    public static FileHandler Instance;

    [SerializeField] private bool isLocal = true;
    [SerializeField] private bool fileValidation;
    [SerializeField] private string JSONFile = "EnemyCard.json";
    [SerializeField] private string filePath = "/Scripts/Image64/";
    public ImgSet arrayImg;

    private void Awake()
    {
        Instance = this;
        //StartCoroutine(Webrequest());
    }

    public string ReadFile()
    {
        string fullPath = string.Empty;
        if (!isLocal)
        {
            fullPath = Application.persistentDataPath + "/" + JSONFile;
        }
        else
        {
            fullPath = filePath + JSONFile;
        }

        fileValidation = File.Exists(Application.dataPath + fullPath);
        if (!fileValidation)
        {
            Debug.Log("File not found.");
            return string.Empty;
        }
        else
        {
            string streamContent = "";
            StreamReader fileReader = new StreamReader(Application.dataPath + fullPath, Encoding.Default);
            streamContent = fileReader.ReadToEnd();
            fileReader.Close();
            RefreshEditorProjectWindow();
            return streamContent;
        }
    }

    public void WriteFile(string objToStore)
    {
        string fullPath = string.Empty;
        if (!isLocal)
        {
            fullPath = Application.persistentDataPath + "/" + JSONFile;
        }
        else
        {
            fullPath = filePath + JSONFile;
        }

        fileValidation = File.Exists(Application.dataPath + fullPath);
        if (!fileValidation)
        {
            Debug.Log(JSONFile);
            File.WriteAllBytes(Application.dataPath + fullPath, System.Text.Encoding.UTF8.GetBytes(objToStore));
        }
        else
        {
            File.Delete(Application.dataPath + fullPath);
            string json = JsonUtility.ToJson(objToStore);
            File.WriteAllBytes(Application.dataPath + fullPath, System.Text.Encoding.UTF8.GetBytes(json));
        }
        RefreshEditorProjectWindow();
    }

    void RefreshEditorProjectWindow()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /*IEnumerator Webrequest()
    {
        WebRequestHandler webRequest = gameObject.AddComponent<WebRequestHandler>();
        StartCoroutine(webRequest.ExecuteRequest("ImageArray/Base64Images"));
        yield return new WaitForSeconds(0.5f);
        arrayImg = JsonUtility.FromJson<ImgSet>(webRequest.result);
    }*/
}