using UnityEngine;

namespace Code
{
    public class TrashFloating : MonoBehaviour
    {
        [Header("Configuración de Caída")]
        [SerializeField] private float fallSpeed = 1.5f;
        
        [Header("Efecto de Flotación Pesada")]
        [SerializeField] private float horizontalFrequency = 0.8f;
        [SerializeField] private float horizontalMagnitude = 0.3f; 
        
        [Header("Rotación de 'Deriva'")]
        [SerializeField] private float rotationSpeed = 20f;

        private float randomOffset;
        private int rotationDirection;

        private void Awake()
        {
            randomOffset = Random.Range(0f, 100f);
            // La basura puede rotar hacia cualquier lado
            rotationDirection = Random.value > 0.5f ? 1 : -1;
        }

        private void Update()
        {
            HandleTrashMovement();
        }

        private void HandleTrashMovement()
        {
            // 1. Caída constante (Eje Y)
            // Se mueve hacia abajo simulando que se hunde lentamente
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

            // 2. Vaivén horizontal (Eje X)
            // Simula la corriente de agua moviendo el objeto pesadamente
            float sway = Mathf.Sin((Time.time + randomOffset) * horizontalFrequency) * horizontalMagnitude;
            transform.position += Vector3.right * sway * Time.deltaTime;

            // 3. Rotación constante
            // La basura no se endereza como el pez, gira sobre su propio eje lentamente
            transform.Rotate(0, 0, rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }
}