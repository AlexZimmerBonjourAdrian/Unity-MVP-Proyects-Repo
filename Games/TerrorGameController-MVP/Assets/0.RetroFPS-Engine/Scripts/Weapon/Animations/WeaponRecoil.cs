using UnityEngine;

namespace RetroFPS
{
    public class WeaponRecoil : MonoBehaviour
    {
        [Header("Recoil Settings")]
        public float verticalRecoil = 2f;
        public float horizontalRecoil = 1f;
        public float recoilSmooth = 5f;
        public float recoverySpeed = 2f;

        [Header("Camera Kick")]
        public float cameraKickAmount = 0.5f;
        public float cameraKickSmooth = 8f;

        [Header("Recoil Pattern")]
        public AnimationCurve recoilPattern = AnimationCurve.Linear(0, 0, 1, 1);
        public float patternIntensity = 1f;

        private Vector3 currentRecoil;
        private Vector3 targetRecoil;
        private float recoilTimer = 0f;
        private bool isRecoiling = false;

        private Camera mainCamera;
        private Transform weaponTransform;

        void Start()
        {
            mainCamera = Camera.main;
            weaponTransform = transform;
        }

        void LateUpdate()
        {
            if (isRecoiling)
            {
                ApplyRecoil();
            }
            else
            {
                RecoverRecoil();
            }
        }

        public void ApplyRecoil(float verticalAmount, float horizontalAmount)
        {
            float randomHorizontal = Random.Range(-horizontalAmount, horizontalAmount);
            float patternValue = recoilPattern.Evaluate(recoilTimer) * patternIntensity;

            targetRecoil = new Vector3(
                randomHorizontal * patternValue,
                verticalAmount * patternValue,
                0
            );

            isRecoiling = true;
            recoilTimer = 0f;

            // Camera kick
            if (mainCamera != null)
            {
                ApplyCameraKick(verticalAmount * 0.1f);
            }
        }

        private void ApplyRecoil()
        {
            recoilTimer += Time.deltaTime * recoverySpeed;

            currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilSmooth);

            if (weaponTransform != null)
            {
                weaponTransform.localRotation *= Quaternion.Euler(-currentRecoil.y, currentRecoil.x, 0);
            }

            if (recoilTimer >= 1f)
            {
                isRecoiling = false;
                targetRecoil = Vector3.zero;
            }
        }

        private void RecoverRecoil()
        {
            if (currentRecoil.magnitude > 0.01f)
            {
                currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, Time.deltaTime * recoverySpeed);

                if (weaponTransform != null)
                {
                    weaponTransform.localRotation *= Quaternion.Euler(-currentRecoil.y, currentRecoil.x, 0);
                }
            }
            else
            {
                currentRecoil = Vector3.zero;
            }
        }

        private void ApplyCameraKick(float kickAmount)
        {
            if (mainCamera != null)
            {
                float kick = Random.Range(-kickAmount * 0.5f, kickAmount * 0.5f);
                mainCamera.transform.localRotation *= Quaternion.Euler(-kickAmount, kick, 0);
            }
        }

        public void ResetRecoil()
        {
            currentRecoil = Vector3.zero;
            targetRecoil = Vector3.zero;
            isRecoiling = false;
            recoilTimer = 0f;
        }

        public void ApplyConfig(FPSConfig config)
        {
            if (config == null) return;

            verticalRecoil = config.baseVerticalRecoil;
            horizontalRecoil = config.baseHorizontalRecoil;
            recoilSmooth = config.recoilSmooth;
            recoverySpeed = config.recoilRecoverySpeed;
            cameraKickAmount = config.cameraKickAmount;
            cameraKickSmooth = config.cameraKickSmooth;
        }
    }
}
