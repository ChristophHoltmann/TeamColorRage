using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seb.Meshing;
using System.Linq;
using UnityEngine.Events;

public class LodMeshLoader : MonoBehaviour
{
    public TextAsset meshFileHighRes;
    public TextAsset meshFileLowRes;

    public Material mat;
    public Material lowResMat;
    public bool useStaticBatching;
    public bool loadOnStart;
    public SimpleLodSystem lodSystem;

    //public float globeScale = 0.01f;
    public Transform globePosition;
    //public float mapScale = 0.1f;
    public Transform mapPosition;
    public float swapGlobeMapTime = 2f;

    public List<CountryController> countries = new List<CountryController>();
    public CountryData countryData;
    public List<CountryTravelData> countryTravelDatas;

    public TripController travelTripPrefab;

    public UnityEvent OnRendersCreated = null;

    void Start()
    {
        if (loadOnStart)
        {
            Load();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SwapToGlobeMode();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SwapToMapMode();
        }
    }

    public void Load()
    {
        MeshRenderer[] highResRenderers = CreateRenderers(meshFileHighRes, mat);
        MeshRenderer[] lowResRenderers = CreateRenderers(meshFileLowRes, lowResMat);

        Debug.Assert(highResRenderers.Length == lowResRenderers.Length, "Mismatch in number of high and low res meshes");

        for (int i = 0; i < highResRenderers.Length; i++)
        {
            lodSystem.AddLOD(highResRenderers[i], lowResRenderers[i]);
        }


    }

    MeshRenderer[] CreateRenderers(TextAsset loadFile, Material material)
    {
        SimpleMeshData[] meshData = MeshSerializer.BytesToMeshes(loadFile.bytes);
        MeshRenderer[] meshRenderers = new MeshRenderer[meshData.Length];
        GameObject[] allObjects = new GameObject[meshData.Length];


        for (int i = 0; i < meshRenderers.Length; i++)
        {
            var renderObject = MeshHelper.CreateRendererObject(meshData[i].name, meshData[i], material, parent: transform, gameObject.layer);

            meshRenderers[i] = renderObject.renderer;
            allObjects[i] = renderObject.gameObject;
            renderObject.gameObject.transform.localPosition = Vector3.zero;
            renderObject.gameObject.transform.localRotation = Quaternion.identity;
            renderObject.gameObject.transform.localScale = Vector3.one;

            Country country = countryData.Countries.FirstOrDefault(c => c.name.Equals(meshData[i].name));

            if (country != null)
            {
                var controller = renderObject.gameObject.AddComponent<CountryController>();
                countries.Add(controller);
                var travelData = countryTravelDatas?.Find(t => t.countryName.Equals(country.name));

                controller.Initialize(country, travelData, travelTripPrefab);
            }

            if (useStaticBatching)
            {
                meshRenderers[i].gameObject.isStatic = true;
            }
        }

        if (useStaticBatching)
        {
            StaticBatchingUtility.Combine(allObjects, gameObject);
        }

        transform.position = globePosition.position;
        transform.rotation = globePosition.rotation;
        transform.localScale = Vector3.one * globePosition.localScale.x;

        OnRendersCreated?.Invoke();
        return meshRenderers;
    }

    Coroutine lerpPositionCorountine = null;
    Coroutine lerpRotationCorountine = null;
    Coroutine lerpScaleCorountine = null;

    public void SwapToGlobeMode()
    {
        SwapMode(globePosition, globePosition.localScale.x);
    }
    public void SwapToMapMode()
    {
        SwapMode(mapPosition, mapPosition.localScale.x);
    }
    private void SwapMode(Transform position, float scale)
    {
        if (lerpPositionCorountine != null)
            StopCoroutine(lerpPositionCorountine);
        lerpPositionCorountine = StartCoroutine(LerpPosition(swapGlobeMapTime, position));
        if (lerpScaleCorountine != null)
            StopCoroutine(lerpScaleCorountine);
        lerpScaleCorountine = StartCoroutine(LerpScale(swapGlobeMapTime, Vector3.one * scale));
    }
    public void Rotate(Vector3 rotation)
    {
        if (lerpRotationCorountine != null)
            StopCoroutine(lerpRotationCorountine);
        lerpRotationCorountine = StartCoroutine(LerpRotation(1f, rotation));
    }

    private IEnumerator LerpPosition(float lerpTime, Transform endPosition)
    {
        var startPosition = transform.position;
        var startRotation = transform.rotation;
        for (float t = 0f; t < lerpTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition.position, t / lerpTime);
            transform.rotation = Quaternion.Lerp(startRotation, endPosition.rotation, t / lerpTime);
            yield return null;
        }
    }
    private IEnumerator LerpRotation(float lerpTime, Vector3 rotation)
    {
        var startRotation = transform.localRotation;
        for (float t = 0f; t < lerpTime; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(rotation), t / lerpTime);
            yield return null;
        }
    }
    private IEnumerator LerpScale(float lerpTime, Vector3 endScale)
    {
        var startScale = transform.localScale;
        for (float t = 0f; t < lerpTime; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t / lerpTime);
            yield return null;
        }
    }

    public void HideAllCountries()
    {
        foreach (var country in countries)
        {
            country.ShowCountry(false);
        }
    }

    public void ShowAllStartCountries()
    {
        foreach (var country in countries)
        {
            country.ShowIfWasActiveOnStart();
        }
    }
    public void ShowCountry(CountryController country)
    {
        country.ShowCountry(true);
    }
}
