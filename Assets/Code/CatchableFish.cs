using UnityEngine;

namespace Code
{
    public class CatchableFish : MonoBehaviour
    {
        [Header("Tipo de Pez")]
        [SerializeField] private bool isInvasive = true;
        [SerializeField] private int pointsAmount = 100;

        private Collider2D fishCollider;
        private Camera mainCamera;

        private void Awake()
        {
            fishCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDoubleTap += TryCapture;
        }

        private void TryCapture(Vector2 screenPosition)
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);
            
            if (fishCollider.OverlapPoint(worldPoint))
            {
                CaptureFish();
            }
        }

        private void CaptureFish()
        {
            if (isInvasive)
            {
                Debug.Log($"¡Pez invasor eliminado! +{pointsAmount} puntos.");
            }
            else
            {
                Debug.Log($"¡Cuidado! Era un pez local. -{pointsAmount} puntos.");
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