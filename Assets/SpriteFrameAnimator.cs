using UnityEngine;

public class SpriteFrameAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private bool playOnAwake = true;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (playOnAwake)
            StartCoroutine(PlayAnimation());
    }

    public System.Collections.IEnumerator PlayAnimation()
    {
        if (frames == null || frames.Length == 0)
            yield break;

        float frameTime = animationDuration / frames.Length;

        for (int i = 0; i < frames.Length; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(frameTime);
        }
    }
}