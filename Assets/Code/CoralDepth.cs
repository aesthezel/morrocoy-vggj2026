using UnityEngine;

namespace Code
{
    public class CoralDepth : MonoBehaviour
    {
        [Header("Configuración de Escala")]
        [SerializeField] private float minScale = 0.3f;
        [SerializeField] private float maxScale = 1.5f;
        [SerializeField] private float growthSensitivity = 0.01f; // Sensibilidad al movimiento del mouse

        [Header("Configuración de Desvanecimiento")]
        [SerializeField] private float alphaFadeStart = 1.1f;
        
        private SpriteRenderer spriteRenderer;
        private Vector3 initialPosition;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            transform.localScale = Vector3.one * minScale;
            initialPosition = transform.position;
        }

        private void Start()
        {
            // Nos suscribimos al evento de slide. 
            // Cada vez que el jugador mueva el mouse/dedo, el coral avanzará.
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSlide += AdvanceDepth;
            }
        }

        private void AdvanceDepth(float deltaX)
        {
            // Usamos el valor absoluto del movimiento (Abs) 
            // Porque no importa si nadas a la izquierda o derecha, siempre avanzas hacia el frente.
            float movementMagnitude = Mathf.Abs(deltaX);
            
            // Calculamos cuánto escalar basado en el movimiento del input
            float scaleStep = movementMagnitude * growthSensitivity;
            
            // 1. Aplicamos escala
            transform.localScale += Vector3.one * scaleStep;

            // 2. Perspectiva: Desplazamiento hacia afuera desde el centro (0,0,0)
            Vector3 directionFromCenter = (transform.position - Vector3.zero).normalized;
            transform.position += directionFromCenter * (scaleStep * 2.5f);

            // 3. Control de Alpha y Destrucción
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            float currentScale = transform.localScale.x;

            // Manejo de transparencia
            if (currentScale >= alphaFadeStart)
            {
                float alpha = Mathf.InverseLerp(maxScale, alphaFadeStart, currentScale);
                Color newColor = spriteRenderer.color;
                newColor.a = alpha;
                spriteRenderer.color = newColor;
            }

            // Destrucción
            if (currentScale >= maxScale)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            // Limpieza de eventos para evitar errores de memoria
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSlide -= AdvanceDepth;
            }
        }
    }
}