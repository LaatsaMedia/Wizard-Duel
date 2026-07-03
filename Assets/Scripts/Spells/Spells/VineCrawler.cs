using UnityEngine;

public class VineCrawler : SpellBehaviour
{
    public override bool SupportsRecast => true;
    public override bool IsGroundSpell => true;

    [Header("Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifetime = 6f;
    private float direction;

    [Header("Activation")]
    [SerializeField] private VineRootArea rootAreaPrefab;
    [SerializeField] private float rootSpawnYOffset = 1f;

    [Header("Enemy AI")]
    [SerializeField] private float minAutoActivateDistance = 2f;
    [SerializeField] private float maxAutoActivateDistance = 3f;

    private float autoActivateDistance;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayer;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteA;
    [SerializeField] private Sprite spriteB;
    [SerializeField] private float animationInterval = 0.5f;

    private bool useFirstSprite = true;
    private bool activated;

    private void Start()
    {
        // Remember which horizontal direction to travel.
        direction = Mathf.Sign(transform.right.x);

        // Keep the sprite upright.
        transform.rotation = Quaternion.identity;

        Invoke(nameof(Activate), lifetime);

        autoActivateDistance = Random.Range(
            minAutoActivateDistance,
            maxAutoActivateDistance);

        spriteRenderer.sprite = spriteA;
        InvokeRepeating(nameof(Animate), animationInterval, animationInterval);
    }

    private void Update()
    {
        if (activated)
            return;

        // Caster died before activating the VineCrawler.
        if (caster == null)
        {
            Destroy(gameObject);
            return;
        }

        // Enemy AI automatically activates when the player gets close.
        if (caster.TryGetComponent(out Health health) &&
            health.Team == Team.Enemy)
        {
            GameObject player = MatchManager.Instance.CurrentPlayer;

            if (player != null)
            {
                float distance = Mathf.Abs(
                    player.transform.position.x - transform.position.x);

                if (distance <= autoActivateDistance)
                {
                    Activate();
                }
            }
        }

        transform.position +=
            Vector3.right * direction * speed * Time.deltaTime;
    }

    private void Animate()
    {
        if (activated)
            return;

        useFirstSprite = !useFirstSprite;

        spriteRenderer.sprite = useFirstSprite
            ? spriteA
            : spriteB;
    }

    public void Activate()
    {
        if (activated)
            return;

        activated = true;

        Vector3 spawnPosition =
            transform.position + Vector3.up * rootSpawnYOffset;

        VineRootArea root = Instantiate(
            rootAreaPrefab,
            spawnPosition,
            Quaternion.identity);

        // Uncomment if VineRootArea needs to know who cast it.
        // root.Initialize(caster);

        Destroy(gameObject);
    }

    public override bool Recast()
    {
        Activate();
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & wallLayer) != 0)
        {
            Activate();
        }
    }
}