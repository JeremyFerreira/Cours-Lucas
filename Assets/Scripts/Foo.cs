using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SuperHeaderAttribute))]
public class SuperHeaderPropertyDrawer : DecoratorDrawer
{
    
    public override float GetHeight()
    {
        SuperHeaderAttribute sHA = (attribute as SuperHeaderAttribute);
        return GetStyle().CalcHeight(new GUIContent(sHA.header),Screen.width);
    }
    public override void OnGUI(Rect position)
    {
        var name = (attribute as SuperHeaderAttribute).header;
        var superColor = (attribute as SuperHeaderAttribute).color;
        Color color = superColor.GetSuperColor();
        GUIStyle style = GetStyle();
        Color previous = GUI.contentColor;
        GUI.contentColor = color;
        EditorGUI.LabelField(position, name, style);
        GUI.contentColor = previous;
    }

    private GUIStyle GetStyle()
    {
        var fontSize = (attribute as SuperHeaderAttribute).fontSize;
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.fontSize = fontSize;
        return style;
    }
}



[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
public class SuperHeaderAttribute : HeaderAttribute
{
    public readonly int fontSize;
    public readonly SuperColor color;
    public SuperHeaderAttribute(string header, int fontSize = 12, SuperColor color = default) : base(header)
    {
        this.fontSize = fontSize;
        this.color = color;
    }
}



public class Foo : MonoBehaviour
{
    [SerializeField, SuperHeader("SuperHeader", 50, SuperColor.Orange)] bool foop;
    [Range(0,10)] public float slider = 0.5f;
    [EnhancedRange(0f,0.1f)] public float superSliderfloat;
    [EnhancedRange(0, 10)] public int superSliderInt = 2;
    [EnhancedRange(0, 10)] public Vector2 superSliderVector2;
    [HandleValue(SuperColor.Red)] public float handleValue;
}
