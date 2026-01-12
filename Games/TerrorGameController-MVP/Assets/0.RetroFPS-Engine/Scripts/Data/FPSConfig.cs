using UnityEngine;

namespace RetroFPS
{
    [CreateAssetMenu(fileName = "FPS Config", menuName = "Retro FPS/Configuration/FPS Config", order = 1)]
    public class FPSConfig : ScriptableObject
    {
        [Header("Movimiento")]
        [Tooltip("Velocidad de movimiento base del jugador")]
        public float moveSpeed = 5f;
        
        [Tooltip("Altura del salto")]
        public float jumpHeight = 2f;
        
        [Tooltip("Fuerza de gravedad")]
        public float gravity = -9.81f;
        
        [Tooltip("Multiplicador de velocidad al correr")]
        public float sprintSpeedMultiplier = 1.5f;
        
        [Tooltip("Tecla para correr")]
        public KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Cámara")]
        [Tooltip("Sensibilidad del mouse")]
        [Range(0.1f, 10f)]
        public float mouseSensitivity = 2f;
        
        [Tooltip("Ãngulo mÃ¡ximo de rotaciÃ³n vertical")]
        [Range(30f, 90f)]
        public float clampAngle = 80f;
        
        [Tooltip("FOV base de la cámara")]
        [Range(60f, 120f)]
        public float baseFOV = 75f;

        [Header("Weapon Sway")]
        [Tooltip("Cantidad de sway del arma")]
        [Range(0f, 0.1f)]
        public float swayAmount = 0.02f;
        
        [Tooltip("Sway máximo permitido")]
        [Range(0f, 0.2f)]
        public float maxSwayAmount = 0.06f;
        
        [Tooltip("Suavidad del sway")]
        [Range(1f, 10f)]
        public float swaySmooth = 3f;
        
        [Tooltip("Sway de rotación")]
        [Range(0f, 10f)]
        public float rotationSwayAmount = 2f;
        
        [Tooltip("Sway máximo de rotación")]
        [Range(0f, 15f)]
        public float maxRotationSway = 5f;
        
        [Tooltip("Velocidad del sway al caminar")]
        [Range(1f, 5f)]
        public float walkingSwaySpeed = 2f;
        
        [Tooltip("Sway al caminar")]
        [Range(0f, 0.05f)]
        public float walkingSwayAmount = 0.01f;
        
        [Tooltip("Sway de respiración (idle)")]
        [Range(0f, 0.02f)]
        public float breathingAmount = 0.005f;
        
        [Tooltip("Velocidad del sway de respiración")]
        [Range(0.5f, 3f)]
        public float breathingSpeed = 1f;

        [Header("Recoil")]
        [Tooltip("Recoil vertical base")]
        [Range(0f, 10f)]
        public float baseVerticalRecoil = 2f;
        
        [Tooltip("Recoil horizontal base")]
        [Range(0f, 5f)]
        public float baseHorizontalRecoil = 1f;
        
        [Tooltip("Suavidad del recoil")]
        [Range(1f, 10f)]
        public float recoilSmooth = 5f;
        
        [Tooltip("Velocidad de recuperación del recoil")]
        [Range(0.5f, 5f)]
        public float recoilRecoverySpeed = 2f;
        
        [Tooltip("Cantidad de camera kick")]
        [Range(0f, 2f)]
        public float cameraKickAmount = 0.5f;
        
        [Tooltip("Suavidad del camera kick")]
        [Range(5f, 15f)]
        public float cameraKickSmooth = 8f;

        [Header("Head Bob")]
        [Tooltip("Intensidad del head bob al caminar")]
        [Range(0f, 0.2f)]
        public float walkingBobAmount = 0.05f;
        
        [Tooltip("Intensidad del head bob al correr")]
        [Range(0f, 0.3f)]
        public float sprintingBobAmount = 0.1f;
        
        [Tooltip("Velocidad del head bob")]
        [Range(5f, 20f)]
        public float bobSpeed = 10f;
        
        [Tooltip("Desactivar head bob al agacharse")]
        public bool disableHeadBobOnCrouch = true;
        
        [Tooltip("Desactivar head bob al apuntar")]
        public bool disableHeadBobOnAim = true;

        [Header("ADS (Aim Down Sights)")]
        [Tooltip("FOV al apuntar")]
        [Range(20f, 60f)]
        public float adsFOV = 40f;
        
        [Tooltip("Posición del arma al apuntar")]
        public Vector3 adsPosition = new Vector3(0, -0.1f, 0.2f);
        
        [Tooltip("Velocidad de transición al apuntar")]
        [Range(5f, 20f)]
        public float adsTransitionSpeed = 10f;
        
        [Tooltip("Reducción de sway al apuntar (0-1)")]
        [Range(0f, 1f)]
        public float adsSwayReduction = 0.5f;
        
        [Tooltip("Tecla para apuntar")]
        public KeyCode aimKey = KeyCode.Mouse1;

        [Header("Sprint")]
        [Tooltip("Posición del arma al correr")]
        public Vector3 sprintPosition = new Vector3(0, -0.2f, 0.1f);
        
        [Tooltip("Aumento de FOV al correr")]
        [Range(0f, 20f)]
        public float sprintFOVIncrease = 10f;
        
        [Tooltip("Tilt del arma al correr")]
        [Range(0f, 30f)]
        public float sprintTilt = 15f;
        
        [Tooltip("Velocidad de transición al correr")]
        [Range(5f, 15f)]
        public float sprintTransitionSpeed = 8f;

        [Header("Input")]
        [Tooltip("Tecla para disparar")]
        public KeyCode fireKey = KeyCode.Mouse0;
        
        [Tooltip("Tecla para recargar")]
        public KeyCode reloadKey = KeyCode.R;
        
        [Tooltip("Tecla para interactuar")]
        public KeyCode interactKey = KeyCode.E;
        
        [Tooltip("Tecla para saltar")]
        public KeyCode jumpKey = KeyCode.Space;

        [Header("General")]
        [Tooltip("Habilitar animaciones de armas")]
        public bool enableWeaponAnimations = true;
        
        [Tooltip("Habilitar head bob")]
        public bool enableHeadBob = true;
        
        [Tooltip("Habilitar weapon sway")]
        public bool enableWeaponSway = true;
        
        [Tooltip("Habilitar recoil")]
        public bool enableRecoil = true;

        [Header("Debug")]
        [Tooltip("Mostrar información de debug")]
        public bool showDebugInfo = false;
        
        [Tooltip("Modo dios (inmortalidad)")]
        public bool godMode = false;
        
        [Tooltip("Munición infinita")]
        public bool infiniteAmmo = false;

        public void ApplyToPlayer(CPlayer3DController player)
        {
            if (player == null) return;

            player.moveSpeed = moveSpeed;
            player.jumpHeight = jumpHeight;
            player.gravity = gravity;
            player.mouseSensitivity = mouseSensitivity;
            player.clampAngle = clampAngle;
        }

        public void ApplyToCamera(Camera camera)
        {
            if (camera == null) return;

            camera.fieldOfView = baseFOV;
        }

        public void ApplyToWeaponSway(WeaponSway weaponSway)
        {
            if (weaponSway == null) return;

            weaponSway.swayAmount = this.swayAmount;
            weaponSway.maxSwayAmount = this.maxSwayAmount;
            weaponSway.swaySmooth = this.swaySmooth;
            weaponSway.rotationSwayAmount = this.rotationSwayAmount;
            weaponSway.maxRotationSway = this.maxRotationSway;
            weaponSway.walkingSwayAmount = this.walkingSwayAmount;
            weaponSway.walkingSwaySpeed = this.walkingSwaySpeed;
            weaponSway.breathingAmount = this.breathingAmount;
            weaponSway.breathingSpeed = this.breathingSpeed;
            weaponSway.enableSway = this.enableWeaponSway;
        }

        public void ApplyToWeaponRecoil(WeaponRecoil weaponRecoil)
        {
            if (weaponRecoil == null) return;

            weaponRecoil.verticalRecoil = this.baseVerticalRecoil;
            weaponRecoil.horizontalRecoil = this.baseHorizontalRecoil;
            weaponRecoil.recoilSmooth = this.recoilSmooth;
            weaponRecoil.recoverySpeed = this.recoilRecoverySpeed;
            weaponRecoil.cameraKickAmount = this.cameraKickAmount;
            weaponRecoil.cameraKickSmooth = this.cameraKickSmooth;
        }

        public void ApplyToHeadBob(HeadBob headBob)
        {
            if (headBob == null) return;

            headBob.walkingBobAmount = this.walkingBobAmount;
            headBob.sprintingBobAmount = this.sprintingBobAmount;
            headBob.bobSpeed = this.bobSpeed;
            headBob.disableOnCrouch = this.disableHeadBobOnCrouch;
            headBob.disableOnAim = this.disableHeadBobOnAim;
            headBob.enableHeadBob = this.enableHeadBob;
        }

        public void ApplyToWeaponADS(WeaponADS weaponADS)
        {
            if (weaponADS == null) return;

            weaponADS.adsFOV = this.adsFOV;
            weaponADS.adsPosition = this.adsPosition;
            weaponADS.adsTransitionSpeed = this.adsTransitionSpeed;
            weaponADS.swayReduction = this.adsSwayReduction;
        }

        public void ApplyToWeaponSprint(WeaponSprint weaponSprint)
        {
            if (weaponSprint == null) return;

            weaponSprint.sprintPosition = this.sprintPosition;
            weaponSprint.sprintFOVIncrease = this.sprintFOVIncrease;
            weaponSprint.sprintTilt = this.sprintTilt;
            weaponSprint.sprintTransitionSpeed = this.sprintTransitionSpeed;
        }

        public void ResetToDefaults()
        {
            moveSpeed = 5f;
            jumpHeight = 2f;
            gravity = -9.81f;
            sprintSpeedMultiplier = 1.5f;
            sprintKey = KeyCode.LeftShift;
            mouseSensitivity = 2f;
            clampAngle = 80f;
            baseFOV = 75f;
            swayAmount = 0.02f;
            maxSwayAmount = 0.06f;
            swaySmooth = 3f;
            rotationSwayAmount = 2f;
            maxRotationSway = 5f;
            walkingSwaySpeed = 2f;
            walkingSwayAmount = 0.01f;
            breathingAmount = 0.005f;
            breathingSpeed = 1f;
            baseVerticalRecoil = 2f;
            baseHorizontalRecoil = 1f;
            recoilSmooth = 5f;
            recoilRecoverySpeed = 2f;
            cameraKickAmount = 0.5f;
            cameraKickSmooth = 8f;
            walkingBobAmount = 0.05f;
            sprintingBobAmount = 0.1f;
            bobSpeed = 10f;
            disableHeadBobOnCrouch = true;
            disableHeadBobOnAim = true;
            adsFOV = 40f;
            adsPosition = new Vector3(0, -0.1f, 0.2f);
            adsTransitionSpeed = 10f;
            adsSwayReduction = 0.5f;
            aimKey = KeyCode.Mouse1;
            sprintPosition = new Vector3(0, -0.2f, 0.1f);
            sprintFOVIncrease = 10f;
            sprintTilt = 15f;
            sprintTransitionSpeed = 8f;
            fireKey = KeyCode.Mouse0;
            reloadKey = KeyCode.R;
            interactKey = KeyCode.E;
            jumpKey = KeyCode.Space;
            enableWeaponAnimations = true;
            enableHeadBob = true;
            enableWeaponSway = true;
            enableRecoil = true;
            showDebugInfo = false;
            godMode = false;
            infiniteAmmo = false;
        }
    }
}
