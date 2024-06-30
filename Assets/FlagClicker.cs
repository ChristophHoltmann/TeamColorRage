using UnityEngine;

public class FlagClicker : MonoBehaviour
{
    private LodMeshLoader meshLoader;

    private void Start()
    {
        meshLoader = FindObjectOfType<LodMeshLoader>();
    }

    public void OnFlagClicked()
    {
        Debug.Log("Trying to open India in map mode");
        meshLoader.ShowCountryInMapMode("India");
    }
}
