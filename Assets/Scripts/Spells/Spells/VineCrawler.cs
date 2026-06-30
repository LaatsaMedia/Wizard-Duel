using UnityEngine;

public class VineCrawler : SpellBehaviour
{
    public override bool SupportsRecast => true;

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
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundOffset = 0.05f;

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

        // Reset rotation so the sprite is always upright.
        transform.rotation = Quaternion.identity;

        // Find the ground directly below the cast position.
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            Mathf.Infinity,
            groundLayer);

        if (hit)
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.point.y + groundOffset,
                transform.position.z);
        }

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

        // Enemy automatically recasts when the player gets close.
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

        //root.Initialize(caster);

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