using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.Build.Content;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEditor.SceneManagement;

public class SceneManagerWindow : EditorWindow
{
    private SceneAsset[] scenes = new SceneAsset[0];
    bool renameMode = false;
    public Vector2 scrollPos = Vector2.zero;
    const float NameWidth = 100f;
    const float BuildIndexWidth = 80f;
    const float LoadWidth = 50f;
    const float SelectWidth = 50f;

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
        using (var _horizontalScope = new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(EditorGUIUtility.IconContent("refresh.png").image, EditorStyles.toolbarButton,GUILayout.Width(30f)))
            {
                scenes = SuperEditorUtility.LoadAssets<SceneAsset>();
                //Sort 
                Array.Sort(scenes, (a, b) =>
                {
                    return GetBuildIndex(a).CompareTo(GetBuildIndex(b));
                });


                int GetBuildIndex(SceneAsset scene)
                {
                    string path = AssetDatabase.GetAssetPath(scene);
                    return SceneUtility.GetBuildIndexByScenePath(path);
                }
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("edit.png").image, EditorStyles.toolbarButton, GUILayout.Width(30f)))
            {
                renameMode = !renameMode;
            }
        }
        using (var scrollScope = new EditorGUILayout.ScrollViewScope(scrollPos))
        {
            scrollPos = scrollScope.scrollPosition;

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
    }


    private void DrawScene(SceneAsset scene, EditorBuildSettingsScene[] buildScenes)
    {
        //scene label
        
        string path = AssetDatabase.GetAssetPath(scene);


        if(renameMode)
        {
            string name = EditorGUILayout.DelayedTextField(scene.name, GUILayout.Width(NameWidth));
            if(name!=scene.name)
            {
                AssetDatabase.RenameAsset(path, path.Replace(path,name));
            }
        }
        else
        {
            EditorGUILayout.LabelField(scene.name);
        }

        //GUILayout.FlexibleSpace();


        //build index
        int index = SceneUtility.GetBuildIndexByScenePath(path);
        if(index == -1)
        {
            if (GUILayout.Button("Add to Build", GUILayout.Width(BuildIndexWidth), GUILayout.Height(20f)))
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
            EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(20f));
            
            if(GUILayout.Button("Remove", GUILayout.Width(BuildIndexWidth), GUILayout.Height(20f)))
            {
                ArrayUtility.RemoveAt(ref buildScenes, index);
                EditorBuildSettings.scenes = buildScenes;
            }
        }

        Scene myScene = SceneManager.GetSceneByPath(path);
        
        //buttons
        if (myScene.isLoaded)
        {
            if (EditorSceneManager.loadedRootSceneCount >1 )
            {
                using (new GUIColorScope(Color.red))
                {
                    if (GUILayout.Button("close", GUILayout.Width(LoadWidth)))
                    {
                        EditorSceneManager.CloseScene(myScene, true);
                    }
                }
            }
            else
            {
                GUILayout.Space(LoadWidth + 3f);
            }
        }
        else
        {
            using (new GUIColorScope(Color.green))
            {
                if (GUILayout.Button("open", GUILayout.Width(LoadWidth)))
                {

                    OpenSceneMode mode = OpenSceneMode.Single;
                    int result = EditorUtility.DisplayDialogComplex("Open Scene", "How to open this scene ?", "Single", "Cancel", "Additive");
                    switch (result)
                    {
                        case 0:
                            mode = OpenSceneMode.Single;
                            break;
                        case 2:
                            mode = OpenSceneMode.Additive;
                            break;
                        default:
                            break;

                    }

                    EditorSceneManager.OpenScene(path, mode);
                }
            }
        }

        using (new GUIColorScope(Color.cyan))
        {
            if (GUILayout.Button("select", GUILayout.Width(SelectWidth)))
                EditorGUIUtility.PingObject(scene);
        }
        

        GUILayout.Space(20f);

    }
}
