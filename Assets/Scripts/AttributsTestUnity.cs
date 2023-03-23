using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;
using JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class SuperTitleAttribute : PropertyAttribute
{
    public readonly SuperColor color;
    public SuperTitleAttribute(SuperColor color)
    {
        this.color = color;
    }
}

[CustomPropertyDrawer(typeof(SuperTitleAttribute))]
public class SuperTitlePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var superColor = (attribute as SuperTitleAttribute).color;
        Color color = superColor.GetSuperColor();
        Color previous = GUI.contentColor;
        GUI.contentColor = color;
        EditorGUI.PropertyField(position, property, label); 
        GUI.contentColor = previous;
    }
}
public static class SuperColorExtensions
{
    public static Color GetSuperColor(this SuperColor superColor)
    {
        switch (superColor)
        {
            case SuperColor.Red:
                return Color.red;
            case SuperColor.Orange:
                return new Color(1.0f, 0.5f, 0.0f);
            case SuperColor.Yellow:
                return Color.yellow;
            case SuperColor.Green:
                return Color.green;
            case SuperColor.Blue:
                return Color.blue;
            case SuperColor.Purple:
                return new Color(0.5f, 0.0f, 0.5f);
            case SuperColor.Pink:
                return Color.magenta;
            case SuperColor.Brown:
                return new Color(0.6f, 0.4f, 0.2f);
            case SuperColor.Black:
                return Color.black;
            case SuperColor.White:
                return Color.white;
            case SuperColor.DarkRed:
                return new Color(0.5f, 0.0f, 0.0f);
            case SuperColor.DarkOrange:
                return new Color(1.0f, 0.3f, 0.0f);
            case SuperColor.DarkYellow:
                return new Color(0.5f, 0.5f, 0.0f);
            case SuperColor.DarkGreen:
                return new Color(0.0f, 0.5f, 0.0f);
            case SuperColor.DarkBlue:
                return new Color(0.0f, 0.0f, 0.5f);
            case SuperColor.DarkPurple:
                return new Color(0.3f, 0.0f, 0.3f);
            case SuperColor.DarkPink:
                return new Color(1.0f, 0.0f, 0.5f);
            case SuperColor.LightRed:
                return new Color(1.0f, 0.6f, 0.6f);
            case SuperColor.LightOrange:
                return new Color(1.0f, 0.8f, 0.6f);
            case SuperColor.LightYellow:
                return new Color(1.0f, 1.0f, 0.6f);
            case SuperColor.UnityFond:
                return new Color(0.56f, 0.56f, 0.56f);
            default:
                return Color.white;
        }
    }
}
public enum SuperColor
{
    Red, Orange, Yellow, Green, Blue, Purple, Pink, Brown, Black, White,
    DarkRed, DarkOrange, DarkYellow, DarkGreen, DarkBlue, DarkPurple, DarkPink,
    LightRed, LightOrange, LightYellow, UnityFond
}

[InitializeOnLoad]
public class AttributsTestUnity : MonoBehaviour
{
    [Header("coucou")]
    [SuperTitle(SuperColor.Blue)] [SerializeField] private bool myBool = true;
    [SerializeField] private bool myBool2 = true;
    [SuperTitle(SuperColor.Yellow)][SerializeField] private float myFloat;
    static AttributsTestUnity()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI(int instanceID, Rect SelectionRect)
    {
        var myObject = EditorUtility.InstanceIDToObject(instanceID);
        if(myObject == null)
        {
            return;
        }
        if(myObject is GameObject gameObject && gameObject.TryGetComponent<Rigidbody>(out _))
        {
            EditorGUI.DrawRect(SelectionRect, Color.green);
        }
        
    }

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        Debug.Log("Initialize");
    }
}
