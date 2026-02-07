using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class OilSplatManager : MonoBehaviour
    {
        public static OilSplatManager Instance { get; private set; }

        [Header("Referencias de UI")]
        [SerializeField] private GameObject oilOverlay1;
        [SerializeField] private GameObject oilOverlay2;
        
        [Header("Configuración de Umbrales")]
        [Tooltip("A qué nivel de petróleo se activa la primera mancha")]
        [SerializeField] private float thresholdFirstSplat = 0.2f;
        [Tooltip("A qué nivel se activa la segunda mancha (ceguera total)")]
        [SerializeField] private float thresholdSecondSplat = 0.5f;

        [SerializeField] private float cleanSpeed = 0.05f; 

        private float currentOilAmount = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else { Destroy(gameObject); return; }
            
            // Empezamos con la visión limpia
            CleanMask();
        }

        private void Update()
        {
            if (currentOilAmount > 0)
            {
                // El petróleo se va "escurriendo" lentamente
                currentOilAmount -= cleanSpeed * Time.deltaTime;
                currentOilAmount = Mathf.Max(currentOilAmount, 0);
                UpdateVisualState();
            }
        }

        public void ApplyOil(float amount)
        {
            currentOilAmount += amount;
            UpdateVisualState();
            
            Debug.Log($"<color=black>Nivel de Crudo:</color> {currentOilAmount}");
        }

        private void UpdateVisualState()
        {
            // Activación binaria basada en umbrales
            if (oilOverlay1 != null)
                oilOverlay1.SetActive(currentOilAmount >= thresholdFirstSplat);

            if (oilOverlay2 != null)
                oilOverlay2.SetActive(currentOilAmount >= thresholdSecondSplat);
        }
        
        public void CleanMask()
        {
            currentOilAmount = 0;
            if (oilOverlay1 != null) oilOverlay1.SetActive(false);
            if (oilOverlay2 != null) oilOverlay2.SetActive(false);
        }
    }
}