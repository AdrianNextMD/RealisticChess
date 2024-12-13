using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMenus : MonoBehaviour
{
    [SerializeField] private List<GameObject> _menus = new List<GameObject>();

    public void OpenMenu(GameObject menu)
    {
        for (int i = 0; i < _menus.Count; i++)
        {
            if (menu)
            {
                if (_menus[i].activeSelf)
                {
                    _menus[i].SetActive(false);
                }
                if (_menus[i].name == menu.name)
                {
                    _menus[i].SetActive(true);
                }
            }
        }
    }

    public void OpenMenu(string menuName = "")
    {
        for (int i = 0; i < _menus.Count; i++)
        {
            if (_menus[i].activeSelf)
            {
                _menus[i].SetActive(false);
            }
            if (_menus[i].name == menuName)
            {
                _menus[i].SetActive(true);
            }
        }
    }
}
