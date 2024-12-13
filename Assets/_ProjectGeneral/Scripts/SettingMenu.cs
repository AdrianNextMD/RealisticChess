using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private GameObject[] btns;

    private void Start()
    {
        ShowPanel("LogScreen");
    }

    public void ShowPanel(string name)
    {
        SetInteractiveBtnUI(name);
        
        foreach (GameObject g in panels)
        {
            if (g.activeSelf)
                g.SetActive(false);
            
            if (g.name == name)
                g.SetActive(true);
        }
    }

    void SetInteractiveBtnUI(string name)
    {
        foreach (var button in btns)
        {
            if (button.name != name)
                button.GetComponent<Button>().interactable = true;
            
            if (button.name == name)
                button.GetComponent<Button>().interactable = false;
        }
    }
}
