using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SuperEditorUtility
{
    public static T[] LoadAssets<T>() where T : UnityEngine.Object
    {
        return Array.ConvertAll(AssetDatabase.FindAssets($"t:{typeof(T).Name}"),
        (s) => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(s)));

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        T[] instances = new T[guids.Length];
        for (int i=0; i<guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            instances[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return instances;
    }
}
