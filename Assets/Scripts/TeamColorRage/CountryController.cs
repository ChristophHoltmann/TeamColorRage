using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryController : MonoBehaviour
{
    Country _country;
    CountryTravelData _travelData;

    TripController _travelTripPrefab;

    private bool activeOnStart = false;

    public void Initialize(Country country, CountryTravelData travelData, TripController travelTripPrefab)
    {
        activeOnStart = gameObject.activeSelf;

        this._country = country;
        this._travelData = travelData;
        this._travelTripPrefab = travelTripPrefab;

        if (_travelData?.trips != null && _travelData.trips.Count > 0)
            foreach (var trip in _travelData.trips)
            {
                ShowTrip(trip);
            }
    }

    public void ShowTrip(TripData tripData)
    {
        var tripController = Instantiate(this._travelTripPrefab, transform);
        tripController.Initialize(tripData);
    }

    public void ShowCountry(bool show)
    {
        gameObject.SetActive(show);
    }
    public void ShowIfWasActiveOnStart()
    {
        ShowCountry(activeOnStart);
    }
}
