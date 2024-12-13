using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItem : MonoBehaviour
{
    [HideInInspector] public ShopResources selected;

    private void Start()
    {
        selected = null;
    }

    public void ButtonSelected(ShopResources newSelected)
    {
        if (selected)
        {
            selected.Unselected();
        }

        selected = newSelected;
        selected.SelectedState();
        selected.changeSelected = ShopResources.ChangeSelected.Selected;
    }
}
