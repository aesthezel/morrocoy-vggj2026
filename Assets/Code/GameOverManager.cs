using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Code
{
    public class GameOverManager : MonoBehaviour
    {
        [Header("Referencias de UI")]
        [SerializeField] private TextMeshProUGUI goodScoreText;
        
        [Header("Referencias de UI")]
        [SerializeField] private TextMeshProUGUI badScoreText;

        private bool canRestart = false;

        private void Start()
        {
            // Nos suscribimos al evento de GameOver del GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver += ShowGameOver;
            }
        }

        private void ShowGameOver()
        {
            // 1. Obtenemos la puntuación final del ScoreManager
            if (ScoreManager.Instance != null)
            {
                var scores = ScoreManager.Instance.GetFinalScore(); // Asegúrate de tener este método en ScoreManager
                goodScoreText.text = scores.Item1.ToString();
                badScoreText.text = scores.Item2.ToString();
            }

            // 2. Permitimos el reinicio
            canRestart = true;
        }

        private void Update()
        {
            if (!canRestart) return;

            // 4. Detección de tecla R con New Input System
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                GameManager.Instance.RestartGame();
            }
        }

        private void OnDestroy()
        {
            // Limpieza de eventos
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver -= ShowGameOver;
            }
        }
    }
}