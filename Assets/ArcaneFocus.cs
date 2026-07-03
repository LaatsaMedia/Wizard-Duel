using System.Collections;
using UnityEngine;

public class ArcaneFocus : SpellBehaviour
{
    private Transform spellSpawn;

    [Header("Charge")]
    [SerializeField] private float chargeTime = 1f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private float fireDuration = 1f;
    [SerializeField] private float fireInterval = 0.05f;
    [SerializeField] private float spread = 0.2f;

    public override void Initialize(GameObject caster, float direction)
    {
        base.Initialize(caster, direction);

        spellSpawn = caster
            .GetComponent<SpellCaster>()
            .SpellSpawn;

        transform.SetParent(spellSpawn);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        float timer = 0f;

        while (timer < chargeTime)
        {
            timer += Time.deltaTime;

            float t = Mathf.SmoothStep(
                0f,
                1f,
                timer / chargeTime);

            transform.localScale = Vector3.one * t;

            yield return null;
        }

        transform.localScale = Vector3.one;

        yield return StartCoroutine(FireRoutine());

        Destroy(gameObject);
    }

    private IEnumerator FireRoutine()
    {
        float timer = 0f;

        while (timer < fireDuration)
        {
            Shoot();

            yield return new WaitForSeconds(fireInterval);

            timer += fireInterval;
        }
    }

    private void Shoot()
    {
        Vector3 spawnPosition = projectileSpawn.position;

        spawnPosition.y += Random.Range(-spread, spread);

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            projectileSpawn.rotation);

        if (projectile.TryGetComponent(out SpellBehaviour spell))
        {
            spell.Initialize(caster, castDirection);
        }
    }
}