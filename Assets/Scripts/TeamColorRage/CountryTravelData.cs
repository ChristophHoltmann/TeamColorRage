using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Country Travel Data")]
public class CountryTravelData : ScriptableObject
{
    public string countryName;

    public List<TripData> trips = new List<TripData>();

    public void ShowTrips()
    {
        foreach(var trip in trips)
        {

        }
    }
}
