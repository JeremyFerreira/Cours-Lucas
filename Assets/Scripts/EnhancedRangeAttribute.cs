using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedRangeAttribute : PropertyAttribute
{
    public float min;
    public float max;
    public EnhancedRangeAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
