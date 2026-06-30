using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform enemySpawn;

    public Transform PlayerSpawn => playerSpawn;
    public Transform EnemySpawn => enemySpawn;

    [SerializeField] private Transform leftFireSpawn;
    [SerializeField] private Transform rightFireSpawn;

    public Transform LeftFireSpawn => leftFireSpawn;
    public Transform RightFireSpawn => rightFireSpawn;
}