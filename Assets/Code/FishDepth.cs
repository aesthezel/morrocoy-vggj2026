namespace Code
{
    using UnityEngine;

    namespace Code
    {
        public class FishDepth : MonoBehaviour
        {
            [Header("Configuración de Profundidad")] [SerializeField]
            private float minScale = 0.2f;

            [SerializeField] private float maxScale = 1.2f;

            [SerializeField]
            private float growthSensitivity = 0.008f; // Los peces suelen ser más pequeños que arrecifes

            [Header("Configuración de Desvanecimiento")] [SerializeField]
            private float alphaFadeStart = 0.9f;

            private SpriteRenderer spriteRenderer;

            private void Awake()
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                // Empezamos pequeño en el "horizonte"
                transform.localScale = Vector3.one * minScale;
            }

            private void Start()
            {
                if (InputManager.Instance != null)
                {
                    InputManager.Instance.OnSlide += AdvanceDepth;
                }
            }

            private void AdvanceDepth(float deltaX)
            {
                // Al igual que el coral, avanzamos al frente con cualquier movimiento lateral
                float movementMagnitude = Mathf.Abs(deltaX);
                float scaleStep = movementMagnitude * growthSensitivity;

                // 1. Aumentamos el tamaño
                transform.localScale += Vector3.one * scaleStep;

                // 2. Perspectiva: Los alejamos del centro conforme se acercan a la cámara
                // Esto evita que todos los peces terminen chocando en el centro de la pantalla
                Vector3 directionFromCenter = (transform.position - Vector3.zero).normalized;
                transform.position += directionFromCenter * (scaleStep * 2f);

                UpdateVisuals();
            }

            private void UpdateVisuals()
            {
                float currentScale = transform.localScale.x;

                // Desvanecimiento (Alpha) cuando ya están muy cerca de "rebasar" al jugador
                if (currentScale >= alphaFadeStart)
                {
                    float alpha = Mathf.InverseLerp(maxScale, alphaFadeStart, currentScale);
                    Color newColor = spriteRenderer.color;
                    newColor.a = alpha;
                    spriteRenderer.color = newColor;
                }

                // Destrucción: Si la escala supera el límite, el pez ya pasó de largo
                if (currentScale >= maxScale)
                {
                    Destroy(gameObject);
                }
            }

            private void OnDestroy()
            {
                if (InputManager.Instance != null)
                {
                    InputManager.Instance.OnSlide -= AdvanceDepth;
                }
            }
        }
    }
}