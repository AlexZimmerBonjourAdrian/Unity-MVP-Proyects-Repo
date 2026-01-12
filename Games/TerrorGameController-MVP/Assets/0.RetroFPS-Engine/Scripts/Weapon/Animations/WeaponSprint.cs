using UnityEngine;

namespace RetroFPS
{
    public class WeaponSprint : MonoBehaviour
    {
        [Header("Sprint Settings")]
        public Vector3 sprintPosition = new Vector3(0, -0.2f, 0.1f);
        public float sprintFOVIncrease = 10f;
        public float sprintTilt = 15f;
        public float sprintTransitionSpeed = 8f;

        [Header("References")]
        public Camera targetCamera;
        public WeaponSway weaponSway;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float initialFOV;
        private bool isSprinting = false;
        private float currentFOV;

        void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            if (targetCamera != null)
            {
                initialFOV = targetCamera.fieldOfView;
                currentFOV = initialFOV;
            }

            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
        }

        void LateUpdate()
        {
            UpdateSprint();
        }

        public void SetSprinting(bool sprinting)
        {
            isSprinting = sprinting;

            if (weaponSway != null)
            {
                weaponSway.SetSprinting(sprinting);
            }
        }

        private void UpdateSprint()
        {
            if (targetCamera == null) return;

            Vector3 targetPosition = isSprinting ? sprintPosition : initialPosition;
            Quaternion targetRotation = isSprinting 
                ? initialRotation * Quaternion.Euler(sprintTilt, 0, 0)
                : initialRotation;

            float targetFOV = isSprinting 
                ? initialFOV + sprintFOVIncrease 
                : initialFOV;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPosition,
                Time.deltaTime * sprintTransitionSpeed
            );

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * sprintTransitionSpeed
            );

            currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * sprintTransitionSpeed);
            targetCamera.fieldOfView = currentFOV;
        }

        public bool IsSprinting()
        {
            return isSprinting;
        }

        public void ApplyConfig(FPSConfig config)
        {
            if (config == null) return;

            sprintPosition = config.sprintPosition;
            sprintFOVIncrease = config.sprintFOVIncrease;
            sprintTilt = config.sprintTilt;
            sprintTransitionSpeed = config.sprintTransitionSpeed;
        }
    }
}
