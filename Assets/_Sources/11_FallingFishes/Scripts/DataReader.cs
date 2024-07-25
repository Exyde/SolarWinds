using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataReader : MonoBehaviour
{
    public string searchUrl = "";
    
    void Start()
    {
        StartCoroutine(GetDataFromScript());
    }

    IEnumerator GetDataFromScript()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(searchUrl))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.InProgress:
                    break;
                case UnityWebRequest.Result.Success:
                    string responseText = webRequest.downloadHandler.text;
                    ProcessScriptData(responseText);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(webRequest.error);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    void ProcessScriptData(string scriptContent)
    {

        Debug.Log("Script Content: " + scriptContent);
    }
}
