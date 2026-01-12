using UnityEngine;

namespace RetroFPS
{
    public class WeaponSway : MonoBehaviour
    {
        [Header("Position Sway")]
        public float swayAmount = 0.02f;
        public float maxSwayAmount = 0.06f;
        public float swaySmooth = 3f;

        [Header("Rotation Sway")]
        public float rotationSwayAmount = 2f;
        public float maxRotationSway = 5f;

        [Header("Walking Sway")]
        public float walkingSwayAmount = 0.01f;
        public float walkingSwaySpeed = 2f;

        [Header("Breathing Sway")]
        public float breathingAmount = 0.005f;
        public float breathingSpeed = 1f;

        [Header("Settings")]
        public bool enableSway = true;
        public float swayReduction = 1f; // Multiplicador para reducir sway (usado por ADS)

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float mouseX, mouseY;
        private float walkingSwayTimer = 0f;
        private float breathingTimer = 0f;
        private bool isWalking = false;
        private bool isSprinting = false;

        private Camera mainCamera;

        void Start()
        {
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (!enableSway) return;

            CalculateSway();
        }

        public void SetWalking(bool walking)
        {
            isWalking = walking;
        }

        public void SetSprinting(bool sprinting)
        {
            isSprinting = sprinting;
        }

        public void SetSwayReduction(float reduction)
        {
            swayReduction = Mathf.Clamp01(reduction);
        }

        private void CalculateSway()
        {
            if (mainCamera == null) return;

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            float effectiveSwayAmount = swayAmount * swayReduction;
            float effectiveMaxSway = maxSwayAmount * swayReduction;

            // Position sway basado en input del mouse
            float moveX = Mathf.Clamp(mouseX * effectiveSwayAmount, -effectiveMaxSway, effectiveMaxSway);
            float moveY = Mathf.Clamp(mouseY * effectiveSwayAmount, -effectiveMaxSway, effectiveMaxSway);

            // Walking sway
            Vector3 walkingOffset = Vector3.zero;
            if (isWalking)
            {
                walkingSwayTimer += Time.deltaTime * walkingSwaySpeed;
                float walkingIntensity = isSprinting ? walkingSwayAmount * 1.5f : walkingSwayAmount;
                walkingOffset = new Vector3(
                    Mathf.Sin(walkingSwayTimer) * walkingIntensity,
                    Mathf.Abs(Mathf.Sin(walkingSwayTimer * 2f)) * walkingIntensity * 0.5f,
                    0
                );
            }
            else
            {
                walkingSwayTimer = 0f;
            }

            // Breathing sway (idle)
            Vector3 breathingOffset = Vector3.zero;
            if (!isWalking && !isSprinting)
            {
                breathingTimer += Time.deltaTime * breathingSpeed;
                breathingOffset = new Vector3(
                    0,
                    Mathf.Sin(breathingTimer) * breathingAmount,
                    0
                );
            }

            // Combinar todos los offsets
            Vector3 targetPosition = initialPosition + new Vector3(moveX, moveY, 0) + walkingOffset + breathingOffset;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * swaySmooth);

            // Rotation sway
            float effectiveRotationSway = rotationSwayAmount * swayReduction;
            float effectiveMaxRotation = maxRotationSway * swayReduction;

            float tiltX = Mathf.Clamp(mouseY * effectiveRotationSway, -effectiveMaxRotation, effectiveMaxRotation);
            float tiltY = Mathf.Clamp(mouseX * effectiveRotationSway, -effectiveMaxRotation, effectiveMaxRotation);

            // Tilt adicional al caminar
            float walkingTilt = 0f;
            if (isWalking)
            {
                walkingTilt = Mathf.Sin(walkingSwayTimer) * 2f;
            }

            Quaternion targetRotation = Quaternion.Euler(tiltX, tiltY, walkingTilt) * initialRotation;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmooth);
        }

        public void ResetSway()
        {
            transform.localPosition = initialPosition;
            transform.localRotation = initialRotation;
        }

        public void ApplyConfig(FPSConfig config)
        {
            if (config == null) return;

            swayAmount = config.swayAmount;
            maxSwayAmount = config.maxSwayAmount;
            swaySmooth = config.swaySmooth;
            rotationSwayAmount = config.rotationSwayAmount;
            maxRotationSway = config.maxRotationSway;
            walkingSwayAmount = config.walkingSwayAmount;
            walkingSwaySpeed = config.walkingSwaySpeed;
            breathingAmount = config.breathingAmount;
            breathingSpeed = config.breathingSpeed;
            enableSway = config.enableWeaponSway;
        }
    }
}
