using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float reloadSceneTimer = 2f;

    [Header("References")]
    [SerializeField] TextMeshProUGUI enemyHealthText;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] CanvasGroup deathScreen;
    [SerializeField] CanvasGroup winningScreen;
    [SerializeField] TextMeshProUGUI restartGameTimerText;

    [Header("Listening to")]
    [SerializeField] SO_VoidEventChannel OnEnemyTakeDamage;
    [SerializeField] SO_VoidEventChannel OnPlayerTakeDamage;
    [SerializeField] SO_VoidEventChannel OnPlayerDied;
    [SerializeField] SO_VoidEventChannel OnPlayerWon;

    private void Awake()
    {
        OnEnemyTakeDamage.OnEventRaised += OnEnemyTakeDamage_OnEventRaised;
        OnPlayerTakeDamage.OnEventRaised += OnPlayerTakeDamage_OnEventRaised;
        OnPlayerDied.OnEventRaised += OnPlayerDied_OnEventRaised;
        OnPlayerWon.OnEventRaised += OnPlayerWon_OnEventRaised;
    }

    private void OnDisable()
    {
        OnEnemyTakeDamage.OnEventRaised -= OnEnemyTakeDamage_OnEventRaised;
        OnPlayerTakeDamage.OnEventRaised -= OnPlayerTakeDamage_OnEventRaised;
        OnPlayerDied.OnEventRaised -= OnPlayerDied_OnEventRaised;
        OnPlayerWon.OnEventRaised -= OnPlayerWon_OnEventRaised;
    }

    private void OnPlayerWon_OnEventRaised(object obj)
    {
        winningScreen.gameObject.SetActive(true);
        StartCoroutine(FadeScreenCoroutine(0, 1, fadeDuration, winningScreen));
    }

    private void OnPlayerDied_OnEventRaised(object obj)
    {
        deathScreen.gameObject.SetActive(true);
        StartCoroutine(FadeScreenCoroutine(0 , 1, fadeDuration, deathScreen));
    }

    private void OnPlayerTakeDamage_OnEventRaised(object obj)
    {
        int health = (int)obj;
        playerHealthText.text = $"PLAYER'S HEALTH: {health}";
    }

    private void OnEnemyTakeDamage_OnEventRaised(object obj)
    {
        int health = (int)obj;
        enemyHealthText.text = $"ENEMY'S HEALTH: {health}";
    }

    // fade the apropriate screen
    private IEnumerator FadeScreenCoroutine(float from, float to, float duration, CanvasGroup screen)
    {
        yield return new WaitForSeconds(1.5f);

        float t = 0;
        screen.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            screen.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;  
        }

        screen.alpha = to;

        StartCoroutine(ReloadSceneCountdown());
    }

    // restart the game after a counter
    private IEnumerator ReloadSceneCountdown()
    {
        restartGameTimerText.gameObject.SetActive(true);

        restartGameTimerText.text = $"Restarting Game in: {reloadSceneTimer.ToString("N1")}";

        while (reloadSceneTimer >= 0)
        {
            reloadSceneTimer -= Time.deltaTime;

            if(reloadSceneTimer <= 0)
            {
                restartGameTimerText.text = $"Restarting Game in: {0}";
            }
            else
            {
                restartGameTimerText.text = $"Restarting Game in: {reloadSceneTimer.ToString("N1")}";
            }

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
