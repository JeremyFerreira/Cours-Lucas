using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleValueAttribute : PropertyAttribute
{
    public Color color;
    public HandleValueAttribute(SuperColor color = SuperColor.White)
    {
        this.color = color.GetSuperColor();
    }
}
