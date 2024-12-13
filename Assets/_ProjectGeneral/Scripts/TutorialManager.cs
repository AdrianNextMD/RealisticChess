using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private GameObject blurGB;

    public void SetObjectInChildren(GameObject obj, int setSiblingIndex)
    {
        obj.transform.SetParent(blurGB.transform);
    }
}
