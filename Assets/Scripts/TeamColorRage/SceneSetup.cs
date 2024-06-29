using Meta.XR.MRUtilityKit;
using UnityEngine;
using UnityEngine.Events;

public class SceneSetup : MonoBehaviour
{
    enum PlacingState
    {
        None,
        Placing,
        Rotating,
        Placed,
    }

    [SerializeField] private GameObject mapPreviewPrefab;
    private GameObject mapPreview;
    [SerializeField] private GameObject interactableWorldPrefab;
    [SerializeField] private GameObject noHitPrefab;
    private GameObject noHitPreview;

    private PlacingState currentPlacingState;

    private Vector3 placingGesturePosition;

    //https://www.youtube.com/watch?v=bSYoRoIVvvo
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private Transform rayStartPoint;
    [SerializeField] private float rayLength = 5.0f;

    [SerializeField] private UnityEvent onPlaced;

    private void Start()
    {
        mapPreview = Instantiate(mapPreviewPrefab);
        mapPreview.SetActive(false);

        noHitPreview = Instantiate(noHitPrefab);
        noHitPreview.SetActive(false);

        currentPlacingState = PlacingState.Placing;

        if (onPlaced == null)
        {
            onPlaced = new UnityEvent();
        }
    }

    private void Update()
    {
        if (currentPlacingState != PlacingState.Placed && rightHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            currentPlacingState = PlacingState.Placing;
        }

        if (currentPlacingState == PlacingState.None || currentPlacingState == PlacingState.Placed) return;

        if (currentPlacingState == PlacingState.Placing)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(rayStartPoint.position, -rayStartPoint.right, out hit, rayLength, LayerMask.GetMask("Placeable"));

            if (hasHit)
            {
                Vector3 hitPoint = hit.point;

                noHitPreview.SetActive(false);

                mapPreview.SetActive(true);
                mapPreview.transform.position = hitPoint;

                if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
                {
                    currentPlacingState = PlacingState.Rotating;
                    placingGesturePosition = rightHand.transform.position;
                    return;
                }
            }
            else
            {
                mapPreview.SetActive(false);
                noHitPreview.SetActive(true);
                noHitPreview.transform.position = rayStartPoint.position + (-rayStartPoint.right * rayLength);
            }
        }

        if (currentPlacingState == PlacingState.Rotating)
        {
            var currentHandPosition = rightHand.transform.position;
            var currentOffset = currentHandPosition - placingGesturePosition;
            currentOffset.y = 0;

            float rotationAngle = Mathf.Atan2(currentOffset.x, currentOffset.z) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0f, rotationAngle, 0f);

            mapPreview.transform.rotation = rotation;

            if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
            {
                currentPlacingState = PlacingState.Placed;
                mapPreview.SetActive(false);
                Instantiate(interactableWorldPrefab, mapPreview.transform.position, mapPreview.transform.rotation);

                onPlaced.Invoke();
                return;
            }
        }
    }
}
