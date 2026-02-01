using System.Collections;
using UnityEngine;

namespace Code
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        [SerializeField] private float sensitivity = 0.05f;
        [SerializeField] private float xLimit = 7f;
        
        [Header("Configuración de Retorno")]
        [SerializeField] private float returnSpeed = 5f;
        
        private float currentX;
        
        // BOOLEAN FLAGS
        private bool isMovingManually;
        private bool isExited = false;
        private bool isTransitioning = false;

        private void Start()
        {
            InputManager.Instance.OnSlide += MovePlayer;
            InputManager.Instance.OnRelease += StartReturning;
            InputManager.Instance.OnDoubleSwipe += HandleVerticalAction;
        }

        private void HandleVerticalAction(Vector2 direction)
        {
            if (isTransitioning) return;
            
            if (direction.y > 0 && !isExited)
            {
                StartCoroutine(VerticalTransition(true));
            }

            else if (direction.y < 0 && isExited)
            {
                StartCoroutine(VerticalTransition(false));
            }
        }
        
        private IEnumerator VerticalTransition(bool exiting)
        {
            isTransitioning = true;
            float duration = 1.0f;
            float elapsed = 0;

            Vector3 startPos = transform.position;

            float targetY = exiting ? 10f : 0f; 
            Vector3 endPos = new Vector3(startPos.x, targetY, startPos.z);

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;
            isExited = exiting;
            isTransitioning = false;
            
            Debug.Log(isExited ? "En la superficie" : "Buceando de nuevo");
        }

        private void MovePlayer(float deltaX)
        {
            isMovingManually = true;
            
            float moveAmount = deltaX * sensitivity; 
            currentX += moveAmount;
            currentX = Mathf.Clamp(currentX, -xLimit, xLimit);
        }

        private void StartReturning()
        {
            isMovingManually = false;
        }

        private void Update()
        {
            if (!isMovingManually && currentX != 0)
            {
                currentX = Mathf.Lerp(currentX, 0, Time.deltaTime * returnSpeed);
                if (Mathf.Abs(currentX) < 0.01f) currentX = 0;
            }
            
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
        }

        private void OnDestroy()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnSlide -= MovePlayer;
                InputManager.Instance.OnRelease -= StartReturning;
            }
        }
    }
}