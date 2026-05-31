using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class RangeValues
{
    [VectorLabels("Min", "Max")]
    public Vector2 speedValues;
}

public class RandomSpeedList : MonoBehaviour
{
    
    public List<RangeValues> rsRange = new List<RangeValues>();
}
