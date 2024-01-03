using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

[Serializable]
public class AppInfo
{
    public List<ResultInfo> results;
}

[Serializable]
public class ResultInfo
{
    public string version;
}

public class VersionController : MonoBehaviour
{
    [SerializeField]
    private string bundleId;

    [SerializeField]
    private string appId;

    private string currentVersion;

    private TitleUIManager manager;

    void Start()
    {
        manager = FindObjectOfType<TitleUIManager>();
        manager.currentVersionText.text = "Version " + Application.version;

        string urlString = "http://itunes.apple.com/lookup?bundleId=" + bundleId;

        using (var client = new WebClient())
        {
            try
            {
                string jsonData = client.DownloadString(urlString);
                Debug.Log(jsonData);

                AppInfo appInfo = JsonUtility.FromJson<AppInfo>(jsonData);

                if (appInfo != null && appInfo.results.Count > 0)
                {
                    currentVersion = appInfo.results[0].version;
                    if (currentVersion != Application.version)
                    {
                        manager.OpenUpdate();
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }

    public void OpenStore()
    {
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appId);
    }
}