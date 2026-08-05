using System.Collections;
using UnityEngine;

public class StarfallEffect : MonoBehaviour
{
    [SerializeField] private GameObject fallingStarPrefab;

    [SerializeField] private float interval = 1.25f;
    [SerializeField] private float randomDelay = 0.15f;

    [SerializeField] private float spawnHeight = 20f;
    [SerializeField] private float randomXOffset = 2.5f;

    public void Initialize(GameObject prefab)
    {
        fallingStarPrefab = prefab;
    }

    private void Start()
    {
        StartCoroutine(StarRoutine());
    }

    private IEnumerator StarRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                interval + Random.Range(-randomDelay, randomDelay));

            SpawnStar();
        }
    }

    private void SpawnStar()
    {
        if(!MatchManager.RoundActive)
            return;
            
        GameObject target = FindTarget();

        if (target == null)
            return;

        Vector3 position = target.transform.position;

        position.x += Random.Range(
            -randomXOffset,
             randomXOffset);

        position.y += spawnHeight;

        Instantiate(
            fallingStarPrefab,
            position,
            Quaternion.identity);
    }

    private GameObject FindTarget()
    {
        Health myHealth = GetComponent<Health>();

        if (myHealth == null)
            return null;

        if (myHealth.Team == Team.Player)
            return MatchManager.Instance.CurrentEnemy;

        return MatchManager.Instance.CurrentPlayer;
    }
}