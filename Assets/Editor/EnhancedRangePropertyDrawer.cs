using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnhancedRangeAttribute))]
public class EnhancedRangePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var min = (attribute as EnhancedRangeAttribute).min;
        var max = (attribute as EnhancedRangeAttribute).max;
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
            case SerializedPropertyType.Enum:
            case SerializedPropertyType.LayerMask:
                EditorGUI.IntSlider(position,property, (int)min, (int)max);
                break;

            case SerializedPropertyType.Float:
                EditorGUI.Slider(position,property, min, max);
                break;
            case SerializedPropertyType.Vector2:

                EditorGUI.BeginProperty(position, label, property);

                float _min = property.vector2Value.x;
                float _max = property.vector2Value.y;

                //label
                position = EditorGUI.PrefixLabel(position, label);

                Rect fieldPosition = new Rect(position);
                fieldPosition.width = EditorGUIUtility.fieldWidth;

                

                //min Value
                _min = EditorGUI.DelayedFloatField(fieldPosition,_min);
                position.xMin += fieldPosition.width + 5;


                //max Value
                fieldPosition.x = position.xMax - fieldPosition.width;
                _max = EditorGUI.DelayedFloatField(fieldPosition, _max);
                position.xMax -= fieldPosition.width + 5;

                //Slider
                EditorGUI.MinMaxSlider(position, ref _min, ref _max, min, max);
                
                //applyValue
                property.vector2Value = new Vector2(Mathf.Clamp(_min,min,_max), Mathf.Clamp(_max, min, _max));

                EditorGUI.EndProperty();

                break;
            default:
                //Error
                EditorGUI.LabelField(position, label, new GUIContent("Error, The EnhancedRange attribute must be use on ints/floats only"));
                break;
        }
    }
}
