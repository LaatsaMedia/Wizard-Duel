using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour
{
    public static MatchUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Timing")]
    [SerializeField] private float countdownDelay = 1f;
    [SerializeField] private float knockoutDuration = 2f;
    [SerializeField] private float winnerDuration = 3f;

    [Header("Score")]
    [SerializeField] private ScoreOrb scoreOrbPrefab;

    [SerializeField] private Transform playerScoreContent;
    [SerializeField] private Transform enemyScoreContent;

    [SerializeField] private Image scoreOrbImagePrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        MatchManager.OnRoundStarted += HandleRoundStarted;
        MatchManager.OnRoundEnded += HandleRoundEnded;
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        MatchManager.OnRoundStarted -= HandleRoundStarted;
        MatchManager.OnRoundEnded -= HandleRoundEnded;
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }

    private void HandleRoundStarted(int round)
    {
        StartCoroutine(RoundCountdown(round));
    }

    private IEnumerator RoundCountdown(int round)
    {
        yield return new WaitForSeconds(1f);

        roundText.text = $"Round {round}";

        countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(countdownDelay);

        countdownText.text = "2";
        yield return new WaitForSeconds(countdownDelay);

        countdownText.text = "1";
        yield return new WaitForSeconds(countdownDelay);

        countdownText.gameObject.SetActive(false);

        MatchManager.Instance.BeginGameplay();
    }

    private void HandleRoundEnded(Team deadTeam, Vector3 deathPosition)
    {
        StartCoroutine(RoundEnded(deadTeam, deathPosition));
    }

    private IEnumerator RoundEnded(Team deadTeam, Vector3 deathPosition)
    {
        resultText.gameObject.SetActive(true);

        string loser =
            deadTeam == Team.Player
            ? "Player"
            : "Enemy";

        resultText.text = $"{loser}\nHas Been\nKnocked Out";

        SpawnScoreOrb(deadTeam, deathPosition);

        yield return new WaitForSeconds(knockoutDuration);

        resultText.gameObject.SetActive(false);

        MatchManager.Instance.BeginNextRound();
    }

    private void SpawnScoreOrb(Team deadTeam, Vector3 deathPosition)
    {
        Transform winner =
            deadTeam == Team.Player
            ? MatchManager.Instance.CurrentEnemy.transform
            : MatchManager.Instance.CurrentPlayer.transform;

        ScoreOrb orb = Instantiate(scoreOrbPrefab);

        orb.FlyTo(
            deathPosition,
            winner,
            () =>
            {
                if (deadTeam == Team.Player)
                {
                    Instantiate(scoreOrbImagePrefab, enemyScoreContent);
                }
                else
                {
                    Instantiate(scoreOrbImagePrefab, playerScoreContent);
                }
            });
    }

    private void HandleMatchEnded()
    {
        StartCoroutine(ShowWinner());
    }

    private IEnumerator ShowWinner()
    {
        resultText.gameObject.SetActive(true);

        if (MatchManager.Instance.PlayerRoundWins >
            MatchManager.Instance.EnemyRoundWins)
        {
            resultText.text = "PLAYER\nIS THE\nWINNER!";
        }
        else if (MatchManager.Instance.EnemyRoundWins >
                 MatchManager.Instance.PlayerRoundWins)
        {
            resultText.text = "ENEMY\nIS THE\nWINNER!";
        }
        else
        {
            resultText.text = "DRAW!";
        }

        yield return new WaitForSeconds(winnerDuration);

        resultText.gameObject.SetActive(false);
    }
}