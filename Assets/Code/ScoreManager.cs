using UnityEngine;
using TMPro;

namespace Code
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [Header("Configuración de UI")]
        [SerializeField] private float scoreAnimSpeed = 10f;

        [SerializeField] private TextMeshProUGUI goodScoreText;
        [SerializeField] private TextMeshProUGUI badScoreText;

        private int currentGoodScore = 0;
        private int currentBadScore = 0;
        private float displayedGoodScore = 0;
        private float displayedBadScore = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!Mathf.Approximately(displayedGoodScore, currentGoodScore))
            {
                displayedGoodScore = Mathf.MoveTowards(displayedGoodScore, currentGoodScore, Time.deltaTime * scoreAnimSpeed * 100);
                UpdateGoodScoreUI();
            }
            
            if (!Mathf.Approximately(displayedBadScore, currentBadScore))
            {
                displayedBadScore = Mathf.MoveTowards(displayedBadScore, currentBadScore, Time.deltaTime * scoreAnimSpeed * 100);
                UpdateBadScoreUI();
            }
        }

        public void AddScore(int amount, bool isGood)
        {
            if (isGood)
            {
                currentGoodScore += amount;
                if (currentGoodScore < 0) currentGoodScore = 0;
            }
            else
            {
                currentBadScore -= amount;
                if (currentBadScore > 0) currentBadScore = 0;
            }
        }

        private void UpdateGoodScoreUI()
        {
            if (goodScoreText != null)
            {
                goodScoreText.text = Mathf.FloorToInt(displayedGoodScore).ToString();
            }
        }

        private void UpdateBadScoreUI()
        {
            if (badScoreText != null)
            {
                badScoreText.text = Mathf.FloorToInt(displayedBadScore).ToString();
            }
        }
        
        public void ResetScore()
        {
            currentGoodScore = 0;
            currentBadScore = 0;
            displayedGoodScore = 0;
            displayedBadScore = 0;
            UpdateGoodScoreUI();
            UpdateBadScoreUI();
        }
        
        public (int,int) GetFinalScore()
        {
            return (currentGoodScore, currentBadScore);
        }
    }
}