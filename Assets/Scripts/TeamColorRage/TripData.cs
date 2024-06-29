using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TripInformation
{
    public Trip tripPrefab;

    public List<Image> images;
}

[CreateAssetMenu(menuName = "Data/Trip Data")]
public class TripData : ScriptableObject
{
    public string tripName;
    public TripInformation tripInformation;
}
