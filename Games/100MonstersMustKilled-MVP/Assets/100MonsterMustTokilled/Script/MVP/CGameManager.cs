using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class CGameManager : MonoBehaviour
{
    public CPlayer player;
    public CEnemy enemy;

    public enum Turn { Player, Enemy }
    public Turn currentTurn;

    public enum Action { Attack, Defend, Skip }
    private Action[] TurnActions = new Action[2];

    [SerializeField] private Button attackButton;
    [SerializeField] private Button defendButton;
    [SerializeField] private Button SkipButton;

    [SerializeField] private Button replayButton;
   
    [SerializeField] private TextMeshProUGUI TurnUi;
    [SerializeField] private TextMeshProUGUI LifePlayerUI;
    [SerializeField] private TextMeshProUGUI LifeEnemyUI;
    [SerializeField] private TextMeshProUGUI ActionsPlayerUI;
    [SerializeField] private TextMeshProUGUI ActionsEnemyUI;

    [SerializeField] private Slider enemyHealthSlider;

    [SerializeField] private CanvasGroup GameOverPanel;

     [SerializeField] private TextMeshProUGUI WinText;
    
    private bool gameOverDisplayed = false; // New flag
    
    private int enemyTurnCounter = 0;
    private bool combatResolved = false;

    private WaitForSeconds turnDelay = new WaitForSeconds(.6f); // 1-second delay

        private bool playerActionChosen = false; // New flag

    void Start()
    {
        currentTurn = Turn.Player;
        
       // endOptionButton.onClick.AddListener(EndPlayerTurn); // Add listener
        UpdateTurnUI();
        UpdateLifeUI();
         replayButton.onClick.AddListener(RestartGame); 
        UpdateEnemyHealthSliderCoroutine(enemy.life);
       
        
    }

    private IEnumerator CombatResolutionCoroutine()
    {
        yield return turnDelay; // Wait for the specified delay

        CheckGameOver();

        // Llama a AdvanceTurn *después* de la resolución del combate y el delay
        AdvanceTurn();  // La corrutina ahora avanza el turno.
        combatResolved = true; // Establece la bandera después de avanzar el turno.
    }

     private void UpdateEnemyHealthSliderCoroutine(float newHealth)
    {
         // Animation duration
        float lifeChangeMagnitude = newHealth / 100; // Calculate magnitude of change
        enemyHealthSlider.value = lifeChangeMagnitude; // Ensure 
    }

    private void AdvanceTurn()
    {
        if (currentTurn == Turn.Player)
        {
            // ... (existing code for player turn)

            currentTurn = Turn.Enemy;
            EnemyDecide();
            ResolveCombat(); 
        }
        else
        {
            currentTurn = Turn.Player; // Cambia el turno inmediatamente
            UpdateTurnUI(); // Actualiza la UI después del cambio de turno
        }
    }

     private IEnumerator PlayerTurnCoroutine()
    {
        yield return turnDelay;
        currentTurn = Turn.Player;
        UpdateTurnUI(); // Update UI immediately on turn change
    }

    void Update()
    {
        if (combatResolved)
        {
            UpdateTurnUI();
            UpdateActionUI();
            UpdateLifeUI();
            UpdateEnemyHealthSliderCoroutine(enemy.life);
            combatResolved = false;
        }
    }

    private void ResolveCombat()
    {
        if (!player.isDead && !enemy.isDead) // Check both are alive BEFORE resolving combat
        {
            if (TurnActions[0] == Action.Attack && TurnActions[1] == Action.Attack)
            {
                player.DiscountLife(30);
                enemy.DiscountLife(30);
    
            }
            else if (TurnActions[0] == Action.Attack && TurnActions[1] == Action.Defend)
            {
                enemy.DiscountLife(5);
               
            }
            else if (TurnActions[0] == Action.Defend && TurnActions[1] == Action.Attack)
            {
                player.DiscountLife(20); 
               
            }
        }
       
        StartCoroutine(CombatResolutionCoroutine());
        CheckGameOver();
        combatResolved = true; // Set flag after combat resolution
    }


  private void RestartGame()
    {
        // Reset player and enemy stats
        player.life = 100f; // Or whatever the starting life is
        player.isDead = false;
        enemy.life = 100f; // Or whatever the starting life is
        enemy.isDead = false;

        // Reset UI elements
        UpdateLifeUI();
        UpdateEnemyHealthSliderCoroutine(enemy.life); // Update slider
         gameOverDisplayed = false; // Reset the flag
        GameOverPanel.alpha = 0f; // Hide game over panel
        GameOverPanel.interactable = false;
        GameOverPanel.blocksRaycasts = false;

        // Reset game logic
        currentTurn = Turn.Player;
        enemyTurnCounter = 0;
        combatResolved = false;
        playerActionChosen = false;
        TurnActions[0] = Action.Skip;  // Reset actions, you may want a default other than 'Skip'
        TurnActions[1] = Action.Skip;

        // Enable buttons
        attackButton.interactable = true;
        defendButton.interactable = true;
        SkipButton.interactable = true;


        UpdateTurnUI();
        UpdateActionUI(); // Reset action UI after setting default values


        Debug.Log("Game Restarted!");
    }

    private void CheckGameOver()
    {
        if (player.life <= 0)
        {
            player.isDead = true;
        }
        if (enemy.life <= 0)
        {
            enemy.isDead = true;
        }

        if ((player.isDead || enemy.isDead) && !gameOverDisplayed) // Check flag
        {
            gameOverDisplayed = true;  // Set the flag to prevent repeated execution

            Debug.Log("Game Over!");
           if (player.isDead && enemy.isDead)
            {
                WinText.text = "Draw!";
                Debug.Log("Draw!");
            }
            else if (player.isDead)
            {
                WinText.text = "You Lose!";
                Debug.Log("You Lose!");
            }
            else
            {
                WinText.text = "You Win!";
                Debug.Log("You Win!");
            }

            GameOverPanel.alpha = 1f; // Show game over panel
            GameOverPanel.interactable = true;
            GameOverPanel.blocksRaycasts = true;

            attackButton.interactable = false;
            defendButton.interactable = false;
            SkipButton.interactable = false; // Disable all buttons
        }

         
    }


    private void EnemyDecide()
    {
        switch (enemyTurnCounter)
        {
            case 0:
            case 1:
                TurnActions[1] = Action.Attack;
                break;
            case 2:
                TurnActions[1] = Action.Defend;
                break;
            default:
                enemyTurnCounter = 0;
                break;
        }
        enemyTurnCounter++;
         Debug.Log($"Enemy {TurnActions[1]}");
    }



    private void UpdateTurnUI()
    {
        TurnUi.text = currentTurn == Turn.Player ? "Player Turn" : "Enemy Turn";
    }

    private void UpdateActionUI()
    {
        ActionsPlayerUI.text = "Player " + TurnActions[0].ToString();
        ActionsEnemyUI.text = "Enemy " + TurnActions[1].ToString();
    }

    private void UpdateLifeUI()
    {
        LifePlayerUI.text = player.life.ToString();
        LifeEnemyUI.text = enemy.life.ToString();
    }

  
    public void AttackPlayer()
    {
        TurnActions[0] = Action.Attack;
        playerActionChosen = true; 
        UpdateActionUI();

        if (currentTurn == Turn.Player) { // Only advance turn if it's the player's turn
             AdvanceTurn();  // Automatically ends turn after attack
         }
    }


     public void SkipPlayer()
    {
        TurnActions[0] = Action.Skip;
        playerActionChosen = true;
        UpdateActionUI();
         if (currentTurn == Turn.Player) {
             AdvanceTurn();  // Automatically ends turn after skip
         }
    }
    public void DefendPlayer()
    {
        TurnActions[0] = Action.Defend;
        playerActionChosen = true;
        UpdateActionUI();

        if (currentTurn == Turn.Player) {
             AdvanceTurn(); // Automatically ends turn after defend
         }
    }

  public void EndPlayerTurn() // Now called by the button
    {
        if (currentTurn == Turn.Player && playerActionChosen)
        {
           AdvanceTurn();
        }
        else if (!playerActionChosen)
        {
            Debug.LogWarning("Player must choose an action.");
        }
    }
}
