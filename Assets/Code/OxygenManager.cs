using UnityEngine;
using System;

namespace Code
{
    public class OxygenManager : MonoBehaviour
    {
        public static OxygenManager Instance { get; private set; }

        [Header("Configuración de Oxígeno")]
        [SerializeField] private float maxOxygen = 100f;
        [SerializeField] private float passiveDrain = 1f; // Consumo por segundo

        [Header("Costos por Acción")]
        [SerializeField] private float slideCost = 0.5f;
        [SerializeField] private float doubleTapCost = 2f;
        [SerializeField] private float doubleSwipeCost = 5f;

        [Header("Referencia UI")]
        [SerializeField] private RectTransform oxygenBall; // La "pelotica"
        [SerializeField] private float maxY = 100f; // Posición Y cuando está lleno
        [SerializeField] private float minY = -100f; // Posición Y cuando está vacío

        private float currentOxygen;
        public bool IsOutOfOxygen => currentOxygen <= 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            currentOxygen = maxOxygen;
        }

        private void Start()
        {
            // Suscribirse a los eventos del InputManager para cobrar oxígeno
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSlide += (delta) => ConsumeOxygen(slideCost);
                InputManager.Instance.OnDoubleTap += (pos) => ConsumeOxygen(doubleTapCost);
                InputManager.Instance.OnDoubleSwipe += (dir) => ConsumeOxygen(doubleSwipeCost);
            }
        }

        private void Update()
        {
            if (currentOxygen > 0)
            {
                // Consumo pasivo por estar bajo el agua
                ConsumeOxygen(passiveDrain * Time.deltaTime);
                UpdateOxygenUI();
            }
        }

        public void ConsumeOxygen(float amount)
        {
            currentOxygen -= amount;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

            if (currentOxygen <= 0)
            {
                GameOver();
            }
        }

        private void UpdateOxygenUI()
        {
            if (oxygenBall != null)
            {
                // Mapeamos el valor de 0-100 a las posiciones Y de la UI
                float t = currentOxygen / maxOxygen;
                float newY = Mathf.Lerp(minY, maxY, t);
                
                // Actualizamos la posición local de la pelotica
                oxygenBall.localPosition = new Vector2(oxygenBall.localPosition.x, newY);
            }
        }

        private void GameOver()
        {
            GameManager.Instance.SetState(GameState.GameOver);
        }
    }
}