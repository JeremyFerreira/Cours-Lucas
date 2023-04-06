using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehavable : MonoBehaviour, IBaseUpdate
{
    public abstract UpdateMode UpdateMode { get ; }
    private void OnEnable()
    {
        UpdateManager.RegisterCallback(this, UpdateMode);
    }

    private void OnDisable()
    {
        UpdateManager.UnRegisterCallback(this, UpdateMode);
    }
}
