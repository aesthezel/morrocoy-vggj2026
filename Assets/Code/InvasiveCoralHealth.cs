using UnityEngine;
using System.Collections;

namespace Code
{
    public class InvasiveCoralHealth : MonoBehaviour
    {
        [Header("Configuración de Resistencia")]
        [Tooltip("Agrega los sprites en orden: desde el más sano al más destruido.")]
        [SerializeField] private Sprite[] damageStages;
        [SerializeField] private int pointsOnDestroy = 150;
        
        [Header("Feedback Visual")]
        [SerializeField] private float shakeMagnitude = 0.05f;
        [SerializeField] private Color hitColor = new Color(1, 0.5f, 0.5f); // Un rojo suave

        private int currentHits;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private Collider2D coralCollider;
        private Camera mainCamera;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            coralCollider = GetComponent<Collider2D>();
            mainCamera = Camera.main;
            originalColor = spriteRenderer.color;
            currentHits = 0;
            
            if (damageStages.Length > 0)
            {
                spriteRenderer.sprite = damageStages[0];
            }
        }

        private void Start()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDoubleTap += TryHitCoral;
        }

        private void TryHitCoral(Vector2 screenPosition)
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);

            if (coralCollider.OverlapPoint(worldPoint))
            {
                TakeHit();
            }
        }

        private void TakeHit()
        {
            currentHits++;
            
            UpdateCoralSprite();
            
            StopAllCoroutines();
            StartCoroutine(HitFeedback());
            
            if (currentHits >= damageStages.Length)
            {
                DefeatCoral();
            }
        }

        private void UpdateCoralSprite()
        {
            // Verificamos que no nos salgamos del índice del arreglo
            if (currentHits < damageStages.Length)
            {
                spriteRenderer.sprite = damageStages[currentHits];
            }
        }

        private IEnumerator HitFeedback()
        {
            spriteRenderer.color = hitColor;
            
            // Guardamos la posición local para no interferir con el movimiento de profundidad
            Vector3 startPos = transform.localPosition;
            
            float elapsed = 0f;
            while (elapsed < 0.1f)
            {
                transform.localPosition = startPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = startPos;
            spriteRenderer.color = originalColor;
        }

        private void DefeatCoral()
        {
            Debug.Log($"¡Coral invasor eliminado! +{pointsOnDestroy} puntos.");
            ScoreManager.Instance.AddScore(pointsOnDestroy, true);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDoubleTap -= TryHitCoral;
        }
    }
}