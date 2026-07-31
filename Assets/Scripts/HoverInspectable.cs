using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MonoBehaviour))]
public class HoverInspectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private IInspectionProvider provider;

    private void Awake()
    {
        provider = GetComponent<IInspectionProvider>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (provider == null)
            return;

        InspectionManager.Instance.Show(provider);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InspectionManager.Instance.Hide();
    }
}