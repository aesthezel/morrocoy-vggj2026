using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace Code
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Referencias de Audio")]
        [SerializeField] private AudioMixer mainMixer;
        [Tooltip("Asegúrate de que el parámetro expuesto en el Mixer se llame exactamente 'MusicCutoff'")]
        [SerializeField] private string exposedParam = "MusicCutoff";
        
        [Header("Configuración de Frecuencia (Hz)")]
        [SerializeField] private float normalFreq = 22000f;    // Frecuencia máxima (Aire)
        [SerializeField] private float gameplayFreq = 10000f;  // Frecuencia solicitada (Agua)
        [SerializeField] private float transitionDuration = 1.5f; // Segundos que tarda el cambio

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += () => SetFilter(gameplayFreq);
                GameManager.Instance.OnGameOver += () => SetFilter(normalFreq);
            }
            
            // Iniciamos en modo normal (Intro)
            mainMixer.SetFloat(exposedParam, normalFreq);
        }

        private void SetFilter(float target)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionRoutine(target));
        }

        private IEnumerator TransitionRoutine(float target)
        {
            float currentFreq;
            mainMixer.GetFloat(exposedParam, out currentFreq);
            float time = 0;

            // Transición suave usando Lerp para evitar saltos de audio molestos
            while (time < transitionDuration)
            {
                time += Time.unscaledDeltaTime; // Importante: ignora la pausa del juego
                float newValue = Mathf.Lerp(currentFreq, target, time / transitionDuration);
                mainMixer.SetFloat(exposedParam, newValue);
                yield return null;
            }

            mainMixer.SetFloat(exposedParam, target);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                // Limpieza de delegados anónimos si es necesario o métodos directos
            }
        }
    }
}