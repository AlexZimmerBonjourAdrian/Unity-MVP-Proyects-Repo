// C:\Github\UnityMVPs\TerrorGameController-MVP\Assets\1.FirstPersonTerrorGameEngine\Scripts\Items\LockedDoor.cs
using UnityEngine;
using System.Collections; // Required for Coroutines (IEnumerator)
using HorrorEngine.Interfaces;
using HorrorEngine.Events;
using HorrorEngine.Manager;
using HorrorEngine.LevelManager; // Assuming you have a CLevelManager class for scene management

// Asegúrate de que tienes definida la interfaz Iinteract en algún lugar
// public interface Iinteract { void Oninteract(); }

public class LockedDoor : MonoBehaviour, Iinteract
{
    [Header("Door Settings")]
    [SerializeField] private bool IsFinishLevel = false; // Flag indicating if the door is unlocked and leads to level finish

    [Header("Sounds")]
    [Tooltip("Sound ID to play when trying to open a locked door.")]
    [SerializeField] private int lockedSoundId = 5;
    [Tooltip("Sound ID to play when the door unlocks.")]
    [SerializeField] private int unlockSoundId = 16;
    [Tooltip("Sound ID to play just before finishing the level.")]
    [SerializeField] private int finishSequenceSoundId = 23; // Sound to play before level transition

    [Header("Level Finish")]
    [Tooltip("Delay in seconds after playing the finish sound before loading the next level.")]
    [SerializeField] private float delayBeforeFinish = 1.5f; // Wait time

    private bool isFinishing = false; // Flag to prevent multiple interactions during the finish sequence

    private void Start()
    {
        // Subscribe to the event that signals the door should be unlocked
        CGameEvents.OnUnlockDoor.Subscribe(HandleUnlockDoor);
    }

    private void OnDestroy()
    {
        // Unsubscribe when the object is destroyed to prevent memory leaks
        CGameEvents.OnUnlockDoor.Unsubscribe(HandleUnlockDoor);
    }

    public void Oninteract()
    {
        // If the finish sequence is already running, do nothing
        if (isFinishing)
        {
            Debug.Log("Finish sequence already in progress.");
            return;
        }

        if (IsFinishLevel) // Check if the door is unlocked and ready to finish the level
        {
            // Start the coroutine to play sound, wait, and then finish
            StartCoroutine(FinishLevelSequence());
        }
        else // Door is still locked
        {
            // Play the locked door sound
            if (CManagerSFX.Inst != null)
            {
                CManagerSFX.Inst.PlaySound(lockedSoundId);
            }
            else
            {
                Debug.LogError("CManagerSFX instance not found!");
            }
            Debug.Log("Door is locked, find the key!");
        }
    }

    // Coroutine to handle the level finish sequence
    private IEnumerator FinishLevelSequence()
    {
        isFinishing = true; // Set the flag to prevent re-triggering
        Debug.Log("Starting level finish sequence...");

        // 1. Play the sound designated for the finish sequence
        if (CManagerSFX.Inst != null)
        {
            Debug.Log($"Playing finish sound (ID: {finishSequenceSoundId}).");
            CManagerSFX.Inst.PlaySound(finishSequenceSoundId);
        }
        else
        {
            Debug.LogError("CManagerSFX instance not found! Cannot play finish sound.");
        }

        // 2. Wait for the specified delay
        Debug.Log($"Waiting for {delayBeforeFinish} seconds before loading next level.");
        yield return new WaitForSeconds(delayBeforeFinish);

        // 3. Call the actual level finish logic
        FinishLevel();

        // Note: isFinishing doesn't need to be reset to false here,
        // because the scene is about to change anyway.
    }

    // Contains the logic to load the next level
    public void FinishLevel()
    {
        Debug.Log("FinishLevel called - Loading next scene.");
        if (CLevelManager.Inst != null)
        {
            CLevelManager.Inst.LoadNextScene();
        }
        else
        {
            Debug.LogError("CLevelManager instance not found! Cannot load next scene.");
        }
    }

    // Method called when the OnUnlockDoor event is published
    private void HandleUnlockDoor()
    {
        if (!IsFinishLevel) // Only unlock and play sound if it wasn't already unlocked
        {
            IsFinishLevel = true; // Mark the door as unlocked
            Debug.Log("Door has been unlocked!");

            // Play the unlock sound
            if (CManagerSFX.Inst != null)
            {
                CManagerSFX.Inst.PlaySound(unlockSoundId);
            }
            else
            {
                Debug.LogError("CManagerSFX instance not found! Cannot play unlock sound.");
            }
        }
        else
        {
             Debug.Log("HandleUnlockDoor called, but door was already unlocked.");
        }
    }
}


