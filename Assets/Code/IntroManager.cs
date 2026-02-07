using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Code
{
    public class IntroManager : MonoBehaviour
    {
        [Header("Referencias de UI")]
        [SerializeField] private Image displayImage;

        [Header("Contenido de la Historia")]
        [SerializeField] private Sprite[] slidesEs;
        [SerializeField] private Sprite[] slidesEn;

        private int currentSlideIndex = -1;
        private bool languageSelected = false;
        private bool storyStarted = false; // Nueva bandera para controlar el inicio real
        private Sprite[] activeSlides;

        private void Awake()
        {
            if (displayImage != null)
            {
                // Mantenemos el objeto activo para ver la portada, pero configuramos aspect ratio
                displayImage.gameObject.SetActive(true);
                displayImage.preserveAspect = true;
            }
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            var pointer = Pointer.current;

            // 1. Selección de Idioma (No cambia la imagen todavía)
            if (!languageSelected && keyboard != null)
            {
                if (keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame) 
                {
                    languageSelected = true;
                    activeSlides = slidesEs;
                    Debug.Log("Idioma: Español seleccionado. Haz clic para empezar.");
                }
                else if (keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame) 
                {
                    languageSelected = true;
                    activeSlides = slidesEn;
                    Debug.Log("Language: English selected. Click to start.");
                }
                return; 
            }

            // 2. Control de Diapositivas
            if (languageSelected && pointer != null && pointer.press.wasPressedThisFrame)
            {
                // Si es el primer clic después de elegir idioma, iniciamos la historia
                if (!storyStarted)
                {
                    storyStarted = true;
                    ShowNextSlide();
                }
                else
                {
                    ShowNextSlide();
                }
            }

            // Animación visual (Solo si ya empezamos a ver los slides)
            if (storyStarted && displayImage.gameObject.activeSelf)
            {
                displayImage.rectTransform.localScale = Vector3.Lerp(
                    displayImage.rectTransform.localScale, 
                    Vector3.one, 
                    Time.unscaledDeltaTime * 8f
                );
            }
        }

        private void ShowNextSlide()
        {
            currentSlideIndex++;

            if (activeSlides != null && currentSlideIndex < activeSlides.Length)
            {
                displayImage.sprite = activeSlides[currentSlideIndex];
                displayImage.rectTransform.localScale = Vector3.one * 0.9f;
            }
            else
            {
                FinishIntro();
            }
        }

        private void FinishIntro()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
            
            gameObject.SetActive(false); 
        }
    }
}