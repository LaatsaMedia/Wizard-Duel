using UnityEngine;
using System;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance { get; private set; }
    public static bool RoundActive { get; private set; }
    public static Action OnCharactersSpawned;

    public static Action<Team, Vector3> OnRoundEnded;
    public static Action OnMatchEnded;
    public static Action<int> OnRoundStarted;

    [Header("Match")]
    [SerializeField] private int winsToWin = 3;

    [Header("Arena")]
    [SerializeField] private Arena arenaPrefab;
    private Arena currentArena;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform enemySpawn;

    [Header("Sudden Death")]
    [SerializeField] private FireWall fireWallPrefab;
    [SerializeField] private float fireStartDelay = 10f;

    private FireWall leftFireWall;
    private FireWall rightFireWall;

    private Coroutine duelRoutine;

    private GameObject currentPlayer;
    private GameObject currentEnemy;

    public GameObject CurrentPlayer => currentPlayer;
    public GameObject CurrentEnemy => currentEnemy;

    public int CurrentRound { get; private set; } = 1;

    public int PlayerRoundWins { get; private set; }
    public int EnemyRoundWins { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartMatch()
    {
        CurrentRound = 1;
        PlayerRoundWins = 0;
        EnemyRoundWins = 0;

        Debug.Log("Match Started!");

        currentArena = Instantiate(arenaPrefab);
        //currentArena = Instantiate(arenas[Random.Range(0, arenas.Length)]);
        StartRound();
    }

    public void StartRound()
    {
        RoundActive = false;

        ClearProjectiles();
        SpawnCharacters();

        OnRoundStarted?.Invoke(CurrentRound);

        Debug.Log($"Round {CurrentRound} Started!");
    }

    private void ClearProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }

    public void BeginGameplay()
    {
        RoundActive = true;

        if (duelRoutine != null)
            StopCoroutine(duelRoutine);

        duelRoutine = StartCoroutine(DuelRoutine());
    }

    private IEnumerator DuelRoutine()
    {
        yield return new WaitForSeconds(fireStartDelay);

        SpawnFireWalls();
    }

    private void SpawnCharacters()
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);

        if (currentEnemy != null)
            Destroy(currentEnemy);

        currentPlayer = Instantiate(
            playerPrefab,
            currentArena.PlayerSpawn.position,
            currentArena.PlayerSpawn.rotation);

        if (currentPlayer.TryGetComponent(out SpellCaster playerCaster))
        {
            playerCaster.ApplyBuild(
                RunManager.Instance.PlayerBuild);
        }

        currentEnemy = Instantiate(
            enemyPrefab,
            currentArena.EnemySpawn.position,
            currentArena.EnemySpawn.rotation);

        if (currentEnemy.TryGetComponent(out SpellCaster enemyCaster))
        {
            enemyCaster.ApplyBuild(
                RunManager.Instance.EnemyBuild);
        }

        OnCharactersSpawned?.Invoke();
    }

    private void SpawnFireWalls()
    {
        leftFireWall = Instantiate(
            fireWallPrefab,
            currentArena.LeftFireSpawn.position,
            Quaternion.identity);

        rightFireWall = Instantiate(
            fireWallPrefab,
            currentArena.RightFireSpawn.position,
            Quaternion.identity);

        leftFireWall.Initialize(Vector2.right);
        rightFireWall.Initialize(Vector2.left);
    }

    public void EndRound(Team deadTeam, Vector3 deathPosition)
    {
        RoundActive = false;

        DestroyFireWalls();

        if (duelRoutine != null)
        {
            StopCoroutine(duelRoutine);
            duelRoutine = null;
        }

        if (deadTeam == Team.Player)
        {
            EnemyRoundWins++;
        }
        else
        {
            PlayerRoundWins++;
        }

        OnRoundEnded?.Invoke(deadTeam, deathPosition);
    }

    private void DestroyFireWalls()
    {
        if (leftFireWall != null)
            Destroy(leftFireWall.gameObject);

        if (rightFireWall != null)
            Destroy(rightFireWall.gameObject);
    }

    public void BeginNextRound()
    {
        if (PlayerRoundWins >= winsToWin ||
            EnemyRoundWins >= winsToWin)
        {
            OnMatchEnded?.Invoke();
            EndMatch();
            return;
        }

        CurrentRound++;
        StartRound();
    }

    private void EndMatch()
    {
        Debug.Log("Match Finished!");

        if (PlayerRoundWins > EnemyRoundWins)
        {
            Debug.Log("PLAYER WINS THE MATCH!");
        }
        else if (EnemyRoundWins > PlayerRoundWins)
        {
            Debug.Log("ENEMY WINS THE MATCH!");
        }
        else
        {
            Debug.Log("MATCH DRAW!");
        }
    }
}