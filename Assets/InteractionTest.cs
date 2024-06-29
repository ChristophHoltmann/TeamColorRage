using Oculus.Interaction;
using UnityEngine;

public class InteractionTest : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material unselectedMaterial;

    [SerializeField] private IndexPinchSafeReleaseSelector selector;
//[SerializeField] private RayInteractable interactable;

    [SerializeField] private Renderer rend;

    void Start()
    {
        if (selector is null) selector = GetComponent<IndexPinchSafeReleaseSelector>();

        selector.WhenSelected += Selector_WhenSelected;
        selector.WhenUnselected += Selector_WhenUnselected;
        
        if (rend is null) rend = GetComponent<Renderer>();
    }

    private void Selector_WhenSelected()
    {
        rend.material = selectedMaterial;
    }

    private void Selector_WhenUnselected()
    {
        rend.material = unselectedMaterial;
    }
}
