using System.Linq;
using UnityEngine;

public class FreezeStackVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer playerRenderer;

    private SpriteRenderer[] shards;

    [Header("Orbit")]
    [SerializeField] private float orbitRadius = 0.6f;
    [SerializeField] private float orbitHeight = 0.25f;
    [SerializeField] private float orbitSpeed = 120f;

    [Header("Depth")]
    [SerializeField] private float frontScale = 1f;
    [SerializeField] private float backScale = 0.8f;

    private int visibleStacks;
    private float angle;

    private void Awake()
    {
        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInParent<SpriteRenderer>();
        }

        shards = GetComponentsInChildren<SpriteRenderer>(true)
            .Where(sr => sr != playerRenderer)
            .OrderBy(sr => sr.name)
            .ToArray();

        foreach (SpriteRenderer shard in shards)
        {
            shard.gameObject.SetActive(false);
        }

        Debug.Log($"Found {shards.Length} freeze shards.");
    }

    private void Update()
    {
        angle += orbitSpeed * Time.deltaTime;

        for (int i = 0; i < shards.Length; i++)
        {
            SpriteRenderer shard = shards[i];

            if (shard == null)
                continue;

            if (!shard.gameObject.activeSelf)
                continue;

            float shardAngle =
                angle + (360f / shards.Length) * i;

            float radians =
                shardAngle * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(radians) * orbitRadius,
                Mathf.Sin(radians) * orbitHeight,
                0f);

            shard.transform.localPosition = position;

            bool inFront = position.y > 0f;

            shard.sortingOrder =
                playerRenderer.sortingOrder +
                (inFront ? 1 : -1);

            float scale =
                inFront ? frontScale : backScale;

            shard.transform.localScale =
                Vector3.one * scale;
        }
    }

    public void SetStacks(int stacks)
    {
        visibleStacks = Mathf.Clamp(stacks, 0, shards.Length);

        for (int i = 0; i < shards.Length; i++)
        {
            if (shards[i] == null)
                continue;

            shards[i].gameObject.SetActive(i < visibleStacks);
        }
    }
}