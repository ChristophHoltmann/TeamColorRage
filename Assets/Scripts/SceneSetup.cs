using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    private GameObject livePreview;
    private bool currentlyPlacingAnchor = false;

    //https://www.youtube.com/watch?v=bSYoRoIVvvo
    [SerializeField] private Transform rayStartPoint;
    [SerializeField] private float rayLength = 5.0f;
    [SerializeField] private MRUKAnchor.SceneLabels labelFilter;

    private void Start()
    {
        livePreview = Instantiate(mapPrefab);
        livePreview.SetActive(false);
    }

    private void OnMRSceneLoaded()
    {
        //enable some kind of continuous raycast
        currentlyPlacingAnchor = true;
    }

    private void Update()
    {
        if (!currentlyPlacingAnchor) return;

        Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hasHit = room.Raycast(ray, rayLength, LabelFilter.Included(labelFilter), out RaycastHit hit, out MRUKAnchor anchor);

        if (hasHit)
        {
            Vector3 hitPoint = hit.point;

            livePreview.SetActive(true);
            livePreview.transform.position = hitPoint;
        }
    }

    private void OnMRAnchorPlaced()
    {
        
    }
}
