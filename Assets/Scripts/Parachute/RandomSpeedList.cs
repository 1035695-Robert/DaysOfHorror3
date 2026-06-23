using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class RangeValues
{
    //[VectorLabelsAttribute("Min", "Max")]
    public Vector2 speedValues;
}

public class RandomSpeedList : MonoBehaviour
{

    public List<RangeValues> rsRange;
}
