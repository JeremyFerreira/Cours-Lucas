using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
[CustomPropertyDrawer(typeof(HandleValueAttribute))]
public class HandleValuePropertyDrawer : PropertyDrawer
{
    bool isSubscribed = false;
    static SerializedProperty cacheProperty = null;
    static Object[] targets = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
        cacheProperty = property;

        if(!isSubscribed)
        {
            SceneView.duringSceneGui += (s) => OnSceneGUI(s, property);
            isSubscribed = true;
        }
        
    }
    void OnSceneGUI(SceneView obj, SerializedProperty property)
    {
        try
        {
            if(cacheProperty == null || cacheProperty.serializedObject == null)
            {
                return;
            }
            var _ = cacheProperty.propertyType;
        }
        catch (Exception _)
        {
            cacheProperty = null;
            return;
        }

        foreach(var targetObject in cacheProperty.serializedObject.targetObjects)
        {
            if(targetObject is Component _component)
            {
                string label;

                switch (cacheProperty.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        label = cacheProperty.intValue.ToString();
                        break;
                    case SerializedPropertyType.Boolean:
                        label = cacheProperty.boolValue.ToString();
                        break;
                    case SerializedPropertyType.Float:
                        label = cacheProperty.floatValue.ToString();
                        break;
                    case SerializedPropertyType.String:
                        label = cacheProperty.stringValue.ToString();
                        break;
                    default:
                        label = "notimplemented";
                        break;
                }
                Color current = GUI.color;
                GUI.color = (attribute as HandleValueAttribute).color;
                
                Handles.Label(_component.transform.position, label);

                GUI.color = current;
            }
            
        }
    }
}
