using UnityEngine;

namespace RetroFPS
{
    public class WeaponReloadAnimation : MonoBehaviour
    {
        [Header("Reload Animation")]
        public Vector3 reloadPosition = new Vector3(0, -0.15f, 0.1f);
        public float reloadTilt = 10f;
        public AnimationCurve reloadCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float animationSpeed = 1f;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private bool isReloading = false;
        private float reloadTimer = 0f;
        private float reloadDuration = 2f;

        void Start()
        {
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
        }

        void LateUpdate()
        {
            if (isReloading)
            {
                UpdateReloadAnimation();
            }
        }

        public void StartReload(float duration)
        {
            isReloading = true;
            reloadTimer = 0f;
            reloadDuration = duration;
        }

        public void StopReload()
        {
            isReloading = false;
            reloadTimer = 0f;
            ResetPosition();
        }

        private void UpdateReloadAnimation()
        {
            reloadTimer += Time.deltaTime * animationSpeed;
            float progress = Mathf.Clamp01(reloadTimer / reloadDuration);
            float curveValue = reloadCurve.Evaluate(progress);

            Vector3 targetPosition = Vector3.Lerp(initialPosition, reloadPosition, curveValue);
            Quaternion targetRotation = Quaternion.Slerp(
                initialRotation,
                initialRotation * Quaternion.Euler(reloadTilt, 0, 0),
                curveValue
            );

            transform.localPosition = targetPosition;
            transform.localRotation = targetRotation;

            if (progress >= 1f)
            {
                StopReload();
            }
        }

        private void ResetPosition()
        {
            transform.localPosition = initialPosition;
            transform.localRotation = initialRotation;
        }

        public bool IsReloading()
        {
            return isReloading;
        }
    }
}
