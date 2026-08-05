using UnityEngine;
using UnityEngine.UI;

public class InformationPanelScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 0.15f;

    private void Update()
    {
        if(!InspectionManager.Instance.IsInspecting)
            return;

        if (!scrollRect.gameObject.activeInHierarchy)
            return;

        float wheel = Input.mouseScrollDelta.y;

        if (Mathf.Abs(wheel) < 0.01f)
            return;

        scrollRect.verticalNormalizedPosition +=
            wheel * scrollSpeed;

        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }
}