using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripController : MonoBehaviour
{
    TripData _tripData;

    Trip _tripObject;

    public void Initialize(TripData tripData)
    {
        this._tripData = tripData;

        gameObject.name += $"_{tripData.name}";

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        // Show specific prefab
        _tripObject = Instantiate(_tripData.tripInformation.tripPrefab, transform);
        //_spline = _tripObject.GetComponentInChildren<ObiRope>();
        //ClearSplineNodes();

        ShowLocations(false);
        ShowImages(false);
    }

    private void ClearSplineNodes()
    {
        //for (int i = _spline.path.points.Count; i >= 0; i--)
        //    _spline.path.RemoveControlPoint(i);
    }

    public void ShowImages(bool show)
    {
        _tripObject.imageParent.gameObject.SetActive(show);

    }
    public void ShowLocations(bool show)
    {
        _tripObject.locationsParent.gameObject.SetActive(show);
        if (show == false) return;

        var locations = _tripObject.locationsParent.GetComponentsInChildren<Transform>();
        ClearSplineNodes();
        foreach (var location in locations)
        {
            //_spline.path.AddControlPoint(location.position, location.forward, -location.forward, location.up, 1, 1, 1, 0, Color.white, location.name);
            //_spline.AddNode(new SplineNode(location.position, location.rotation.eulerAngles));
        }
    }
}
