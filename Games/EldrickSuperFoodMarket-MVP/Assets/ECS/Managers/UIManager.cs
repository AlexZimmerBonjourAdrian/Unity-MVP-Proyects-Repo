using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance { get { return instance; } }

        [Header("Combat UI")]
        public GameObject combatPanel;
        public TextMeshProUGUI playerHealthText;
        public TextMeshProUGUI enemyHealthText;
        public Button attackButton;
        public Button defendButton;

        [Header("Dialogue UI")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;
        public GameObject dialogueOptionsPanel;
        public Button[] dialogueOptionButtons;

        [Header("Time UI")]
        public GameObject timePanel;
        public TextMeshProUGUI encountersText;
        public TextMeshProUGUI daysText;

        [Header("Game Over UI")]
        public GameObject gameOverPanel;
        public TextMeshProUGUI gameOverText;

        [Header("Victory UI")]
        public GameObject victoryPanel;
        public TextMeshProUGUI victoryText;

        [Header("Menu UI")]
        public GameObject menuPanel;
        public Button startButton;
        public Button quitButton;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
        }

        private void Start()
        {
        }

        public void ShowCombatUI()
        {
        }

        public void HideCombatUI()
        {
        }

        public void ShowDialogueUI()
        {
        }

        public void HideDialogueUI()
        {
        }

        public void ShowTimeUI()
        {
        }

        public void HideTimeUI()
        {
        }

        public void ShowGameOverUI(string message)
        {
        }

        public void HideGameOverUI()
        {
        }

        public void ShowVictoryUI(string message)
        {
        }

        public void HideVictoryUI()
        {
        }

        public void ShowMenuUI()
        {
        }

        public void HideMenuUI()
        {
        }

        public void UpdatePlayerHealth(float health)
        {
        }

        public void UpdateEnemyHealth(float health)
        {
        }

        public void UpdateEncounters(int current, int max)
        {
        }

        public void UpdateDays(int days)
        {
        }
    }
}

