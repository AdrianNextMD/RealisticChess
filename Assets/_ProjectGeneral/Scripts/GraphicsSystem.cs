using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GraphicsSystem : MonoBehaviour
{
    //public Text videoMemory;

    [Header("Graphics Menu Manager")]
    public Text shadowsStatus;
    public bool shadowsEnabled;

    public Text graphicsLevel;
    public int currentGraphicsLvl;
    public string[] graphicsLvls;

    private void OnEnable()
    {
        //videoMemory.text = "Video Core: " + SystemInfo.graphicsDeviceName + " | Video Memory: " + SystemInfo.graphicsMemorySize + " MB";
        Refresh_ShadowsSaved();
        Refresh_GraphicsSaved();
    }

    public void Set_Shadows()
    {
        if (!shadowsEnabled) { shadowsEnabled = true; }
        else { shadowsEnabled = false; }

        ES3.Save("Shadows", shadowsEnabled);
        Set_ShadowsText(shadowsEnabled);

        Debug.Log("shadowsEnabled" + shadowsEnabled);
    }

    private void Set_ShadowsText(bool active)
    {
        if(active)
        {
            shadowsStatus.text = "On";
            // for(int i=0; i<shadowsStatus.Length;i++)
            // {
            //     shadowsStatus[i].text = "On";
            // }
        }
        else
        {
            shadowsStatus.text = "Off";
            // for (int i = 0; i < shadowsStatus.Length; i++)
            // {
            //     shadowsStatus[i].text = "Off";
            //}
        }
    }

    public void Set_GraphicsLevel(string btnType)
    {
        StartCoroutine(UiUtils.Instance.ShowLoading(true, 1f, "Loading..."));
        if(btnType == "leftBtn")
        {
            if(currentGraphicsLvl > 0)
            {
                currentGraphicsLvl--;
                //for (int i = 0; i < graphicsLevel.Length; i++)
                //{
                graphicsLevel.text = graphicsLvls[currentGraphicsLvl];
                //}
                Set_Quality(currentGraphicsLvl);
            }
        }

        if (btnType == "rightBtn")
        {
            if(currentGraphicsLvl < graphicsLvls.Length - 1)
            {
                currentGraphicsLvl++;
                //for (int i = 0; i < graphicsLevel.Length; i++)
                //{
                    graphicsLevel.text = graphicsLvls[currentGraphicsLvl];
                //}
                
                Set_Quality(currentGraphicsLvl);
            }
        }
    }

    public void Set_Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        ES3.Save("GraphicsLevel", qualityIndex);
    }

    public void Refresh_ShadowsSaved()
    {
        bool enabledShadows = ES3.Load<bool>("Shadows", true);

        shadowsEnabled = enabledShadows;
        Set_ShadowsText(enabledShadows);
    }

    public void Refresh_GraphicsSaved()
    {
        currentGraphicsLvl = ES3.Load<int>("GraphicsLevel", 2);
        QualitySettings.SetQualityLevel(currentGraphicsLvl);

        //for (int i = 0; i < graphicsLevel.Length; i++)
        //{
        graphicsLevel.text = graphicsLvls[currentGraphicsLvl];
        //}
    }
}
