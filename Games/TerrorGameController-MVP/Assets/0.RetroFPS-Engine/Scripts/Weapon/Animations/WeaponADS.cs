using UnityEngine;

namespace RetroFPS
{
    public class WeaponADS : MonoBehaviour
    {
        [Header("ADS Settings")]
        public float adsFOV = 40f;
        public Vector3 adsPosition = new Vector3(0, -0.1f, 0.2f);
        public float adsTransitionSpeed = 10f;
        public float swayReduction = 0.5f;

        [Header("References")]
        public Camera targetCamera;
        public WeaponSway weaponSway;

        private Vector3 initialPosition;
        private float initialFOV;
        private bool isAiming = false;
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

            if (weaponSway == null)
            {
                weaponSway = GetComponent<WeaponSway>();
            }
        }

        void Update()
        {
            HandleInput();
            UpdateADS();
        }

        private void HandleInput()
        {
            bool aimInput = Input.GetButton("Fire2");

            if (aimInput != isAiming)
            {
                SetAiming(aimInput);
            }
        }

        public void SetAimingFromExternal(bool aiming)
        {
            SetAiming(aiming);
        }

        public void SetAiming(bool aiming)
        {
            isAiming = aiming;

            if (weaponSway != null)
            {
                weaponSway.SetSwayReduction(aiming ? swayReduction : 1f);
            }
        }

        private void UpdateADS()
        {
            if (targetCamera == null) return;

            Vector3 targetPosition = isAiming ? adsPosition : initialPosition;
            float targetFOV = isAiming ? adsFOV : initialFOV;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPosition,
                Time.deltaTime * adsTransitionSpeed
            );

            currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * adsTransitionSpeed);
            targetCamera.fieldOfView = currentFOV;
        }

        public bool IsAiming()
        {
            return isAiming;
        }

        public void SetADSPosition(Vector3 position)
        {
            adsPosition = position;
        }

        public void SetADSFOV(float fov)
        {
            adsFOV = fov;
        }

        public void ApplyConfig(FPSConfig config)
        {
            if (config == null) return;

            adsFOV = config.adsFOV;
            adsPosition = config.adsPosition;
            adsTransitionSpeed = config.adsTransitionSpeed;
            swayReduction = config.adsSwayReduction;
        }
    }
}
