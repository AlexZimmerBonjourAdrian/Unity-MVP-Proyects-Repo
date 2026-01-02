using UnityEngine;
using TMPro;
public class CGameManagerPlayButton : MonoBehaviour
{
   public enum GamePlayButton
   {
    none,
    Start,
    Play,
    FinishGame,
    GameOver
   }
    public TextMeshProUGUI textUiTest;

    public TextMeshProUGUI Counter;
    private GamePlayButton actualState = GamePlayButton.Start;

    private int counter = 0;

    private bool isHoldingButton = false;
    public void SetState(GamePlayButton state)
   {
     actualState = state;
   }

     public GamePlayButton GetState()
   {
        return actualState;
   }

    void Start()
    {
        chagedTextCounter("Start");
        SetState(GamePlayButton.Play);
    }

    public void chagedText(string changeText)
    {
        textUiTest = GameObject.Find("GameState").GetComponent<TextMeshProUGUI>();
        textUiTest.text = changeText; 
    }

   public void chagedTextCounter(string changeText)
    {
        textUiTest = GameObject.Find("Titulo").GetComponent<TextMeshProUGUI>();
        textUiTest.text = changeText; 
    }

    public void ButtonDown()
    {
        if (actualState == GamePlayButton.Play)
        {
            isHoldingButton = true;
        }
    }

     public void ButtonUp()
    {
        isHoldingButton = false;
    }
    private void GameStartState()
    {
        chagedText("State: " + "Game Start");
    }

    private void GamePlayState()
    {
          chagedText("State: " + "Game Play");
        
    }

    private void GameOverState()
    {
        chagedText("State: " + "Game GameOver");
    }

    private void FinishGameState()
    {
        chagedText("State: " + "Game FinishGame");
    }

     void Update()
   {
    if(actualState == GamePlayButton.Start)
    {
      Invoke("GameStartState",1f);
    }
    else if(actualState == GamePlayButton.Play)
    {
       Invoke("GamePlayState",1f);

       if (isHoldingButton)
        {
           counter++;
           chagedTextCounter(counter.ToString());
        }
    }
    else if(actualState == GamePlayButton.GameOver)
    {
         Invoke("GameOverState",1f);
    }
    else if(actualState == GamePlayButton.FinishGame)
    {
         Invoke("FinishGameState",1f);
    }
   
   }
}
