using UnityEngine;

namespace RetroFPS
{
    public class WeaponIdle : MonoBehaviour
    {
        [Header("Idle Settings")]
        public float idleAmount = 0.002f;
        public float idleSpeed = 1f;
        public Vector3 idleDirection = new Vector3(1, 1, 0);

        private Vector3 initialPosition;
        private float idleTimer = 0f;
        private bool isActive = true;

        void Start()
        {
            initialPosition = transform.localPosition;
        }

        void LateUpdate()
        {
            if (isActive)
            {
                UpdateIdle();
            }
        }

        private void UpdateIdle()
        {
            idleTimer += Time.deltaTime * idleSpeed;

            Vector3 idleOffset = new Vector3(
                Mathf.Sin(idleTimer) * idleAmount * idleDirection.x,
                Mathf.Cos(idleTimer * 1.3f) * idleAmount * idleDirection.y,
                Mathf.Sin(idleTimer * 0.7f) * idleAmount * idleDirection.z
            );

            transform.localPosition = initialPosition + idleOffset;
        }

        public void SetActive(bool active)
        {
            isActive = active;
            if (!active)
            {
                transform.localPosition = initialPosition;
            }
        }
    }
}
