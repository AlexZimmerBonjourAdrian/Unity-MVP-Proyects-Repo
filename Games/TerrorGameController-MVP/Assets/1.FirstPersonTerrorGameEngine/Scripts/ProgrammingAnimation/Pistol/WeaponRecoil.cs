using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Handles weapon recoil mechanics, including vertical and horizontal recoil, smooth interpolation, and recovery.
    /// </summary>
    public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    [Tooltip("Maximum vertical recoil amount.")]
    public float maxVerticalRecoil = 2f;

    [Tooltip("Maximum horizontal recoil amount.")]
    public float maxHorizontalRecoil = 1f;

    [Tooltip("Speed at which recoil is applied.")]
    public float recoilSpeed = 10f;

    [Tooltip("Speed at which the weapon returns to its original position.")]
    public float returnSpeed = 5f;

    private Vector3 currentRecoil;
    private Vector3 targetRecoil;
    private Vector3 initialPosition;

    /// <summary>
    /// Initializes the weapon recoil system.
    /// </summary>
    void Start()
    {
        initialPosition = transform.localPosition;
        currentRecoil = Vector3.zero;
        targetRecoil = Vector3.zero;
    }

    /// <summary>
    /// Updates the recoil system every frame.
    /// </summary>
    void Update()
    {
        SmoothRecoilReturn();
    }

    /// <summary>
    /// Applies recoil to the weapon by offsetting its position vertically and horizontally.
    /// </summary>
    public void ApplyRecoil()
    {
        float verticalRecoil = Random.Range(0, maxVerticalRecoil);
        float horizontalRecoil = Random.Range(-maxHorizontalRecoil, maxHorizontalRecoil);

        targetRecoil += new Vector3(horizontalRecoil, verticalRecoil, 0);
        targetRecoil = Vector3.ClampMagnitude(targetRecoil, Mathf.Max(maxVerticalRecoil, maxHorizontalRecoil));
    }

    /// <summary>
    /// Smoothly returns the weapon to its initial position after recoil.
    /// </summary>
    private void SmoothRecoilReturn()
    {
        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + currentRecoil, Time.deltaTime * returnSpeed);

        // Gradually reduce the target recoil to zero for recovery
        targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, Time.deltaTime * returnSpeed);
    }
    }
}