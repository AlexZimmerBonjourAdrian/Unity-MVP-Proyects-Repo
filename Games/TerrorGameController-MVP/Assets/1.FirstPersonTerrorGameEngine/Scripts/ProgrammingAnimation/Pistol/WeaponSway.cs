using UnityEngine;

namespace HorrorEngine
{
    public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.10f; // Cantidad de oscilación
    public float swaySpeed = 20f; // Velocidad de oscilación
    public float breathAmount = 0.02f; // Intensidad de la respiración
    public float breathSpeed = 1.5f; // Velocidad de la respiración

    private Quaternion initialRotation;
    private ProceduralAnimation proceduralAnimaton;
    private float breathTimer;

    void Start()
    {
        proceduralAnimaton = GetComponent<ProceduralAnimation>();
        if (proceduralAnimaton == null)
        {
            Debug.LogError("ProceduralAnimation component is missing on the GameObject. Please add it.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }
        initialRotation = proceduralAnimaton.GetInitialTransform().localRotation;
    }

    void Update()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calcular la oscilación del mouse
        Quaternion xRotation = Quaternion.AngleAxis(-mouseY * swayAmount, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(mouseX * swayAmount, Vector3.up);

        // Añadir movimiento de respiración
        breathTimer += Time.deltaTime * breathSpeed;
        float breathOffset = Mathf.Sin(breathTimer) * breathAmount;
        Quaternion breathRotation = Quaternion.Euler(breathOffset, 0, 0);

        // Añadir ruido Perlin para variaciones orgánicas
        float noiseX = Mathf.PerlinNoise(Time.time * 0.5f, 0) * 0.02f;
        float noiseY = Mathf.PerlinNoise(0, Time.time * 0.5f) * 0.02f;
        Quaternion noiseRotation = Quaternion.Euler(noiseY, noiseX, 0);

        // Combinar todas las rotaciones
        Quaternion targetRotation = initialRotation * xRotation * yRotation * breathRotation * noiseRotation;

        // Aplicar la oscilación suavemente
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySpeed * Time.deltaTime);
    }
    }
}