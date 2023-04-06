using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTest : MonoBehavable, IEarlyUpdatable
{
    public override UpdateMode UpdateMode => UpdateMode.Early;

    public void Updater()
    {
        Debug.Log("Updtate");
    }
}
