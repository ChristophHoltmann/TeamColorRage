using com.mukarillo.prominentcolor;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImageSelector : MonoBehaviour
{
    public MeshRenderer screen;
    public List<MeshRenderer> colorSpheres = new();
    public Color sphereColor = Color.white;

    private SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowImageOnScreen();
        }
    }

    void Start()
    {
        screen = FindObjectOfType<ImageScreen>()?.GetComponent<MeshRenderer>();
        var imageColorSpheres = FindObjectsOfType<ImageColorSphere>();
        if (imageColorSpheres != null)
            foreach (var ics in imageColorSpheres)
                colorSpheres.Add(ics.GetComponent<MeshRenderer>());

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.AddComponent<BoxCollider>();
    }

    public void ShowImageOnScreen()
    {
        screen.transform.localScale = new Vector3(screen.transform.localScale.x,
            GetImageRatio() * screen.transform.localScale.x, screen.transform.localScale.z);
        screen.material.mainTexture = spriteRenderer.sprite.texture;

        if (colorSpheres != null && colorSpheres.Count > 0 && spriteRenderer?.sprite?.texture != null)
        {
            //var colors = ProminentColor.GetColors32FromImage(spriteRenderer.sprite.texture, 1, 50f, 30, 3f);
            colorSpheres.ForEach(ics => ics.material.color = sphereColor);
        }
    }

    public float GetImageRatio()
    {
        if (spriteRenderer?.sprite?.texture == null) return 1;

        return ((float)spriteRenderer.sprite.texture.width) / spriteRenderer.sprite.texture.height;
    }
}
