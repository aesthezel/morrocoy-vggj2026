using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Code
{
    public enum GameState { Intro, Gameplay, GameOver }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; }
        
        [Header("Paneles de UI")]
        [SerializeField] private GameObject introPanel;
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject gameOverPanel;

        public event Action OnGameStart;
        public event Action OnGameOver;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            SetState(GameState.Intro);
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;

            // Desactivamos todo por defecto
            introPanel.SetActive(false);
            gameplayUI.SetActive(false);
            gameOverPanel.SetActive(false);

            switch (CurrentState)
            {
                case GameState.Intro:
                    introPanel.SetActive(true);
                    Time.timeScale = 0; // Pausamos el juego en la portada
                    break;

                case GameState.Gameplay:
                    gameplayUI.SetActive(true);
                    Time.timeScale = 1; // Reanudamos el tiempo
                    OnGameStart?.Invoke();
                    break;

                case GameState.GameOver:
                    gameOverPanel.SetActive(true);
                    Time.timeScale = 0; // Pausamos al morir
                    OnGameOver?.Invoke();
                    break;
            }
        }

        // Llamar desde el botón "Empezar" en la portada
        public void StartGame()
        {
            SetState(GameState.Gameplay);
        }

        // Llamar para reiniciar la partida
        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}