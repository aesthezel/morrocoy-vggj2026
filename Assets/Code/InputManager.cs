using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public event Action<float> OnSlide; 
        public event Action OnRelease;
        public event Action<Vector2> OnDoubleSwipe;
        public event Action<Vector2> OnDoubleTap;

        private PlayerInput playerInput;
        private InputAction clickAction;
        private InputAction pointAction;

        private bool isDragging = false;
        private Vector2 startDragPosition; // Posición fija donde inició el clic
        private Vector2 lastFramePosition; // Posición que cambia frame a frame
    
        [Header("Configuración Double Swipe")]
        [SerializeField] private float doubleSwipeTimeLimit = 0.5f; 
        [SerializeField] private float minSwipeDistance = 50f;
        
        [Header("Configuración Double Tap")]
        [SerializeField] private float tapThreshold = 0.25f; // Tiempo entre clics
        [SerializeField] private float tapMovementLimit = 10f; // Píxeles máximos para que sea Tap y no Drag
        private float lastClickTime;
    
        private float lastSwipeTime;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            playerInput = GetComponent<PlayerInput>();
            clickAction = playerInput.actions["Click"];
            pointAction = playerInput.actions["Point"];
        }

        private void OnEnable()
        {
            clickAction.started += StartDrag;
            clickAction.canceled += EndDrag;
        }

        private void OnDisable()
        {
            clickAction.started -= StartDrag;
            clickAction.canceled -= EndDrag;
        }

        private void StartDrag(InputAction.CallbackContext context)
        {
            isDragging = true;
            Vector2 currentPos = pointAction.ReadValue<Vector2>();
            startDragPosition = currentPos;
            lastFramePosition = currentPos;
        }

        private void EndDrag(InputAction.CallbackContext context)
        {
            Vector2 endPos = pointAction.ReadValue<Vector2>();
            Vector2 totalDiff = endPos - startDragPosition;
            
            float timeSinceLastClick = Time.time - lastClickTime;
            
            if (totalDiff.magnitude < tapMovementLimit)
            {
                if (timeSinceLastClick < tapThreshold)
                {
                    OnDoubleTap?.Invoke(endPos);
                    lastClickTime = 0; // Reset
                }
                else
                {
                    lastClickTime = Time.time;
                }
            }
            else if (Mathf.Abs(totalDiff.y) > minSwipeDistance && Mathf.Abs(totalDiff.y) > Mathf.Abs(totalDiff.x))
            {
                if (Time.time - lastSwipeTime < doubleSwipeTimeLimit)
                {
                    OnDoubleSwipe?.Invoke(new Vector2(0, Mathf.Sign(totalDiff.y)));
                    lastSwipeTime = 0; // Reset
                }
                else
                {
                    lastSwipeTime = Time.time;
                }
            }

            isDragging = false;
            OnRelease?.Invoke();
        }

        private void Update()
        {
            if (isDragging)
            {
                Vector2 currentPointerPosition = pointAction.ReadValue<Vector2>();
                
                // Calculamos el delta basado en el frame anterior, no en el inicio del drag
                float deltaX = currentPointerPosition.x - lastFramePosition.x;

                if (Mathf.Abs(deltaX) > 0.01f) 
                {
                    OnSlide?.Invoke(deltaX);
                }

                lastFramePosition = currentPointerPosition;
            }
        }
    }
}