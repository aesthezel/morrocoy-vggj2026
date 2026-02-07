using UnityEngine;

namespace Code
{
    public class TrashInteraction : MonoBehaviour
    {
        [Header("Tipo de Basura")]
        [SerializeField] private bool isToxic = false;
        [SerializeField] private int pointsOnCapture = 50;
        [SerializeField] private int pointsPenaltyIfNotToxic = 20;

        [Header("Efecto de Petróleo (Solo si es Tóxico)")]
        [SerializeField] private float oilIntensity = 0.25f;
        [SerializeField] private float oxygenPenalty = 5f;

        private Collider2D trashCollider;
        private Camera mainCamera;
        private bool isCaptured = false;

        private void Awake()
        {
            trashCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDoubleTap += TryCapture;
        }

        private void TryCapture(Vector2 screenPosition)
        {
            if (isCaptured) return;

            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);

            if (trashCollider.OverlapPoint(worldPoint))
            {
                HandleCapture();
            }
        }

        private void HandleCapture()
        {
            isCaptured = true;

            if (isToxic)
            {
                if (OilSplatManager.Instance != null)
                    OilSplatManager.Instance.ApplyOil(oilIntensity);
                
                if (OxygenManager.Instance != null)
                    OxygenManager.Instance.ConsumeOxygen(oxygenPenalty);

                ScoreManager.Instance.AddScore(pointsOnCapture, true);
                Debug.Log("¡Basura tóxica recolectada! Máscara manchada.");
            }
            else
            {
                ScoreManager.Instance.AddScore(pointsOnCapture, true);
                Debug.Log("¡Basura común recolectada! Mar más limpio.");
            }

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDoubleTap -= TryCapture;
        }
    }
}