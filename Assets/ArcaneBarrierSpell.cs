using UnityEngine;

public class ArcaneBarrierSpell : SpellBehaviour
{
    [SerializeField] private float barrierHealth = 50f;
    [SerializeField] private float duration = 3f;

    private Barrier barrier;

    void Start()
    {
        transform.SetParent(caster.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public override void Initialize(GameObject caster, float castDirection)
    {
        base.Initialize(caster, castDirection);

        transform.SetParent(caster.transform);
        transform.localPosition = Vector3.zero;

        barrier = caster.GetComponent<Barrier>();

        if (barrier == null)
        {
            barrier = caster.AddComponent<Barrier>();
        }

        barrier.Initialize(barrierHealth);

        Destroy(gameObject, duration);
    }

    private void Update()
    {
        if (barrier == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (barrier != null)
        {
            Destroy(barrier);
        }
    }
}