using UnityEngine;
using Yarn.Unity;
using HorrorEngine;
using TMPro;

public class CNotesController : MonoBehaviour
{
    [SerializeField] private GameObject noteUI; // UI element to display the note
    [SerializeField] private float interactionDistance = 3.0f; // Distance to interact with the note
    [SerializeField] private string noteText; // Text of the note
    [SerializeField] private TMP_Text noteUIText; // TextMeshPro UI element for the note text
    private bool isReading = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isReading)
        {
            TryReadNote();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isReading)
        {
            CloseNote();
        }
    }

    private void TryReadNote()
    {
        if (Vector3.Distance(transform.position, CGameManager.Inst.PlayerPosition) <= interactionDistance)
        {
            isReading = true;
            CGameManager.Inst.SetGameState(GameState.ReadingNote);
            noteUI.SetActive(true);
            StartCoroutine(TypewriterEffect(noteText));
        }
    }

    private void CloseNote()
    {
        isReading = false;
        CGameManager.Inst.SetGameState(GameState.Playing);
        noteUI.SetActive(false);
    }

    private System.Collections.IEnumerator TypewriterEffect(string text)
    {
        noteUIText.text = ""; // Use TextMeshPro component
        foreach (char c in text)
        {
            noteUIText.text += c; // Update TextMeshPro text
            yield return new WaitForSecondsRealtime(0.02f); // Independent of game time
        }
    }
}
