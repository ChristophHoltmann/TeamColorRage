using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryController : MonoBehaviour
{
    Country _country;
    CountryTravelData _travelData;

    TripController _travelTripPrefab;

    private bool activeOnStart = false;

    Coroutine lerpRotationCorountine = null;

    LodMeshLoader lodMeshLoader = null;

    private void Update()
    {
        if (_travelData && _travelData.countryName == "India" && Input.GetKeyDown(KeyCode.I))
        {
            ShowCountry(true);
        }
    }

    public void Initialize(Country country, CountryTravelData travelData, TripController travelTripPrefab)
    {
        activeOnStart = gameObject.activeSelf;
        lodMeshLoader = transform.parent.GetComponent<LodMeshLoader>();

        this._country = country;
        this._travelData = travelData;
        this._travelTripPrefab = travelTripPrefab;

        if (_travelData?.trips != null && _travelData.trips.Count > 0)
            foreach (var trip in _travelData.trips)
            {
                ShowTrip(trip);
            }
    }

    private IEnumerator LerpRotation(float lerpTime, Quaternion endRotation)
    {
        var startRotation = transform.rotation;
        for (float t = 0f; t < lerpTime; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / lerpTime);
            yield return null;
        }
    }

    public void ShowTrip(TripData tripData)
    {
        var tripController = Instantiate(this._travelTripPrefab, transform);
        tripController.Initialize(tripData);
    }

    public void ShowCountry(bool show)
    {
        lodMeshLoader?.Rotate(_travelData.countryUpRotation);
        gameObject.SetActive(show);
    }
    public void ShowIfWasActiveOnStart()
    {
        ShowCountry(activeOnStart);
    }
}
