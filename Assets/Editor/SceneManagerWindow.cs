using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.Build.Content;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SceneManagerWindow : EditorWindow
{
    private SceneAsset[] scenes = new SceneAsset[0];
    bool renameMode = false;

    // ----------------------------------------------------
    [MenuItem("Tools/Scene Manager")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneManagerWindow>(false,"Scene Manager", true);
        window.titleContent.image = EditorGUIUtility.IconContent("aaa.png").image;
        window.Show();
    }
    private void OnGUI()
    {
        using(var scope = new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(EditorGUIUtility.IconContent("refresh.png").image, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                scenes = SuperEditorUtility.LoadAssets<SceneAsset>();
                //Sort 
                Array.Sort( scenes, (a, b) => 
                {
                    return GetBuildIndex(a).CompareTo(GetBuildIndex(b)); 
                } );


                int GetBuildIndex(SceneAsset scene)
                {
                    string path = AssetDatabase.GetAssetPath(scene);
                    return SceneUtility.GetBuildIndexByScenePath(path);
                }
            }
            if(GUILayout.Button(EditorGUIUtility.IconContent("edit.png").image, GUILayout.Width(30f), GUILayout.Height(30f)))
            {
                renameMode = !renameMode;
            }
        }

        
        
        var sceneInBuild = EditorBuildSettings.scenes;

        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), Color.gray);

        for(int i=0;i<scenes.Length;i++)
        {
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                DrawScene(scenes[i], sceneInBuild);
            }
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), Color.gray);
        }
    }


    private void DrawScene(SceneAsset scene, EditorBuildSettingsScene[] buildScenes)
    {
        //scene label
        
        string path = AssetDatabase.GetAssetPath(scene);


        if(renameMode)
        {
            string name = EditorGUILayout.DelayedTextField(scene.name);
            if(name!=scene.name)
            {
                AssetDatabase.RenameAsset(path, path.Replace(path,name));
            }
        }
        else
        {
            EditorGUILayout.LabelField(scene.name);
        }

        GUILayout.FlexibleSpace();


        //build index
        int index = SceneUtility.GetBuildIndexByScenePath(path);
        if(index == -1)
        {
            if (GUILayout.Button("Add to Build", GUILayout.Width(150f), GUILayout.Height(30f)))
            {
                int _matchIndex = Array.FindIndex(buildScenes, scene => scene.path == path);
                if (_matchIndex != -1)
                {
                    buildScenes[_matchIndex].enabled = true;
                }
                else
                {
                    ArrayUtility.Add(ref buildScenes, new EditorBuildSettingsScene(path, true));
                }                
                EditorBuildSettings.scenes = buildScenes;
            }
        }
        else
        {
            EditorGUILayout.LabelField(index.ToString());
        }
        GUILayout.Space(20f);

    }
}
