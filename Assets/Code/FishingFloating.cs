using UnityEngine;

namespace Code
{
    public class FishingFloating : MonoBehaviour
    {
        [Header("Configuración de Dirección")]
        [Tooltip("Si es falso, el pez mirará y se moverá hacia la izquierda al iniciar.")]
        [SerializeField] private bool startsLookingRight = true;
        
        [Header("Movimiento Errático")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float frequency = 1.5f;
        [SerializeField] private float magnitude = 0.5f;
        
        [Header("Rotación Suave")]
        [SerializeField] private float rotationLimit = 15f;
        [SerializeField] private float rotationSpeed = 5f;

        private SpriteRenderer spriteRenderer;
        private float randomOffset;
        private float currentMoveSpeed; // Usamos una variable interna para no sobreescribir el prefab

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            randomOffset = Random.Range(0f, 100f);
        }

        private void Start()
        {
            // Ajustamos la velocidad inicial según la dirección configurada
            // Si mira a la derecha, velocidad positiva. Si mira a la izquierda, negativa.
            currentMoveSpeed = startsLookingRight ? Mathf.Abs(moveSpeed) : -Mathf.Abs(moveSpeed);
            
            // Forzamos la actualización visual inmediata
            UpdateSpriteFlip();
        }

        private void Update()
        {
            HandleFloatingMovement();
            UpdateSpriteFlip();
        }

        private void HandleFloatingMovement()
        {
            // Se desplaza horizontalmente según la velocidad calculada
            transform.Translate(Vector3.right * currentMoveSpeed * Time.deltaTime);

            // Efecto de flotación errática (Eje Y)
            float upDown = Mathf.Sin((Time.time + randomOffset) * frequency) * magnitude;
            transform.position += transform.up * upDown * Time.deltaTime;

            // Rotación visual (Giro/Tilt)
            float tilt = Mathf.Sin((Time.time + randomOffset) * frequency) * rotationLimit;
            
            // Si el pez va a la izquierda, invertimos el tilt para que la nariz no apunte al lado contrario
            if (currentMoveSpeed < 0) tilt *= -1;

            Quaternion targetRotation = Quaternion.Euler(0, 0, tilt);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        private void UpdateSpriteFlip()
        {
            bool isMovingRight = currentMoveSpeed > 0;

            if (startsLookingRight)
            {
                // Si el dibujo original mira a la derecha, flipeamos si va a la izquierda
                spriteRenderer.flipX = !isMovingRight;
            }
            else
            {
                // Si el dibujo original mira a la izquierda, flipeamos si va a la derecha
                spriteRenderer.flipX = isMovingRight;
            }
        }

        public void ReverseDirection()
        {
            currentMoveSpeed *= -1;
        }
    }
}