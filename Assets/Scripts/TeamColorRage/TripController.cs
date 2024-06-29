using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripController : MonoBehaviour
{
    TripData _tripData;

    public void Initialize(TripData tripData)
    {
        this._tripData = tripData;

        gameObject.name += $"_{tripData.name}";

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
