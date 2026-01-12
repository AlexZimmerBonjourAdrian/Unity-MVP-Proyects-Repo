using UnityEngine;

namespace RetroFPS
{
    public class HeadBob : MonoBehaviour
    {
        [Header("Head Bob Settings")]
        public float walkingBobAmount = 0.05f;
        public float sprintingBobAmount = 0.1f;
        public float bobSpeed = 10f;
        public AnimationCurve bobCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Settings")]
        public bool enableHeadBob = true;
        public bool disableOnCrouch = true;
        public bool disableOnAim = true;

        private float bobTimer = 0f;
        private float currentBobAmount = 0f;
        private Vector3 initialPosition;
        private bool isWalking = false;
        private bool isSprinting = false;
        private bool isCrouched = false;
        private bool isAiming = false;

        void Start()
        {
            initialPosition = transform.localPosition;
        }

        void LateUpdate()
        {
            if (!enableHeadBob) return;

            if (ShouldDisableHeadBob())
            {
                ResetHeadBob();
                return;
            }

            UpdateHeadBob();
        }

        public void SetWalking(bool walking)
        {
            isWalking = walking;
            if (!walking)
            {
                bobTimer = 0f;
            }
        }

        public void SetSprinting(bool sprinting)
        {
            isSprinting = sprinting;
            currentBobAmount = sprinting ? sprintingBobAmount : walkingBobAmount;
        }

        public void SetCrouched(bool crouched)
        {
            isCrouched = crouched;
        }

        public void SetAiming(bool aiming)
        {
            isAiming = aiming;
        }

        private bool ShouldDisableHeadBob()
        {
            return (disableOnCrouch && isCrouched) || (disableOnAim && isAiming) || !isWalking;
        }

        private void UpdateHeadBob()
        {
            if (!isWalking) return;

            bobTimer += Time.deltaTime * bobSpeed;
            float curveValue = bobCurve.Evaluate(Mathf.Sin(bobTimer));

            Vector3 bobOffset = new Vector3(
                0,
                curveValue * currentBobAmount,
                0
            );

            transform.localPosition = initialPosition + bobOffset;
        }

        private void ResetHeadBob()
        {
            bobTimer = 0f;
            transform.localPosition = initialPosition;
        }

        public void ApplyConfig(FPSConfig config)
        {
            if (config == null) return;

            walkingBobAmount = config.walkingBobAmount;
            sprintingBobAmount = config.sprintingBobAmount;
            bobSpeed = config.bobSpeed;
            disableOnCrouch = config.disableHeadBobOnCrouch;
            disableOnAim = config.disableHeadBobOnAim;
            enableHeadBob = config.enableHeadBob;
        }
    }
}
