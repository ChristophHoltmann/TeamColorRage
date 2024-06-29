using Oculus.Interaction;
using UnityEngine;

public class InteractionTest : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material unselectedMaterial;

    private ISelector selector;

    private Renderer rend;

    void Start()
    {
        selector = GetComponent<ISelector>();

        selector.WhenSelected += Selector_WhenSelected;
        selector.WhenUnselected += Selector_WhenUnselected;
        
        rend = GetComponent<Renderer>();
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
