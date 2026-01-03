using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace HorrorEngine
{

public static class CGameEvents 
{   
  
    //To-do: integrate 
public static readonly CGameEvent<string> OnPlayerDeath = new CGameEvent<string>(); 
    
    //Character Events
    //To-do: integrate 
   public static readonly CGameEvent OnDialogueStart = new CGameEvent();
public static readonly CGameEvent<string>  OnDialogueTrigger = new CGameEvent<string>();
    //To-do: integrate 
public static readonly CGameEvent OnDialogueEnd = new CGameEvent();
public static readonly CGameEvent OnInterruptDialogue = new CGameEvent();
public static readonly CGameEvent OnPersuasiveDialogue = new CGameEvent();

    //Eventos de UI
    //To-do: integrate 
public static readonly CGameEvent<bool> OnShowPauseMenu = new CGameEvent<bool>();
public static readonly CGameEvent<bool> OnShowInventory = new CGameEvent<bool>();

    
    
    //Eventos de sonido
public static readonly CGameEvent<int> OnPlaySound = new CGameEvent<int>();
public static readonly CGameEvent OnStopSound = new CGameEvent();

//Animatic
public static readonly CGameEvent OnAnimaticPlay= new CGameEvent();

public static readonly CGameEvent OnAnimaticStop= new CGameEvent();


public static readonly CGameEvent OnAnimaticNext = new CGameEvent();
public static readonly CGameEvent<int> OnAnimaticChange= new CGameEvent<int>();

public static readonly CGameEvent OnAnimaticPrevious = new CGameEvent();

public static readonly CGameEvent OnAnimaticQuit = new CGameEvent();
//SaveGame
public static readonly CGameEvent OnSaveGame = new CGameEvent();
   
public static readonly CGameEvent OnLoadGame = new CGameEvent();

public static readonly CGameEvent OnSceneLoaded = new CGameEvent();

public static readonly CGameEvent OnGameStart = new CGameEvent();
public static readonly CGameEvent OnGameQuit = new CGameEvent();

public static readonly CGameEvent OnCutsceneStart = new CGameEvent();


public static readonly CGameEvent OnCutsceneEnd = new CGameEvent();

public static readonly CGameEvent OnObjectInteract = new CGameEvent();

public static readonly CGameEvent OnObjectPickUp = new CGameEvent();

public static readonly CGameEvent OnObjectDrop = new CGameEvent();

public static readonly CGameEvent OnTriggerEnter = new CGameEvent();
public static readonly CGameEvent  OnTriggerExit = new CGameEvent();    

public static readonly CGameEvent OnTriggerStay = new CGameEvent();   

public static readonly CGameEvent OnEnemyHit = new CGameEvent();   

public static readonly CGameEvent OnPlayerHit = new CGameEvent();  

public static readonly CGameEvent OnEnemyDeath= new CGameEvent();   

public static readonly CGameEvent OnWeaponFire = new CGameEvent(); 

public static readonly CGameEvent OnButtonPress = new CGameEvent(); 

public static readonly CGameEvent OnMenuOpen = new CGameEvent();

public static readonly CGameEvent OnMenuClose = new CGameEvent(); 

public static readonly CGameEvent OnMusicChange = new CGameEvent(); 

public static readonly CGameEvent OnGameplayStart = new CGameEvent(); 

public static readonly CGameEvent OnTooltipShow = new CGameEvent(); 

public static readonly CGameEvent OnSoundEffectPlay = new CGameEvent(); 

//OnActivateDoor 
    public static readonly CGameEvent OnActivateDoor = new CGameEvent();
//CompleteLevel
    public static readonly CGameEvent<bool> OnCompleteLevel = new CGameEvent<bool>();


//Flags
//OnLightSwitch Event Speficic
    public static readonly CGameEvent OnLightSwitch = new CGameEvent();
    public static readonly CGameEvent OnCounLightSwitch = new CGameEvent();
     public static readonly CGameEvent OnNotificationSaveData = new CGameEvent();

    public static readonly CGameEvent OnPickupRevolver = new CGameEvent();
    public static readonly CGameEvent OnPickupShootGun = new CGameEvent();
    public static readonly CGameEvent OnPickupMag = new CGameEvent();
    public static readonly CGameEvent OnReloadShootgun = new CGameEvent();
    public static readonly CGameEvent OnReloadRevolver = new CGameEvent();
    public static readonly CGameEvent OnDeathMoffin = new CGameEvent();
    public static readonly CGameEvent OnDeathLoop = new CGameEvent();
    
    public static readonly CGameEvent OnCombineRevolver = new CGameEvent();
    public static readonly CGameEvent OnGameLevel0Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel1Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel2Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel3Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel4Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel5Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel6Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel7Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel8Complete = new CGameEvent();
    public static readonly CGameEvent OnGameLevel9Complete = new CGameEvent();  
    public static readonly CGameEvent OnGameLevel10Complete = new CGameEvent();
    public static readonly CGameEvent OnGameFinish = new CGameEvent();
    public static readonly CGameEvent OnGameDialogueRunner = new CGameEvent();





    public static readonly CGameEvent<string> OnPickupKey = new CGameEvent<string>();

    // Evento publicado cuando una puerta es desbloqueada con éxito. Pasa el ID de la puerta o llave usada.
    public static readonly CGameEvent OnUnlockDoor = new CGameEvent();

  public static readonly CGameEvent OnTakePhone = new CGameEvent();
  
//Examples to integrate public static readonly CGameEvent<bool>

//   void OnEnable()
//     {
//         CGameEvents.OnDialogueTrigger.Subscribe(OnDialogueTrigger);
//         CGameEvents.OnDialogueEnd.Subscribe(OnDialogueEnd);
//         CGameEvents.OnShowPauseMenu.Subscribe(OnShowPauseMenu);
//         CGameEvents.OnShowInventory.Subscribe(OnShowInventory);
//         CGameEvents.OnPlaySound.Subscribe(OnPlaySound);
//     }

//     void OnDisable()
//     {
//         CGameEvents.OnDialogueTrigger.Unsubscribe(OnDialogueTrigger);
//         CGameEvents.OnDialogueEnd.Unsubscribe(OnDialogueEnd);
//         CGameEvents.OnShowPauseMenu.Unsubscribe(OnShowPauseMenu);
//         CGameEvents.OnShowInventory.Unsubscribe(OnShowInventory);
//         CGameEvents.OnPlaySound.Unsubscribe(OnPlaySound);
//     }

//     private void OnDialogueTrigger(string dialogueID)
//     {
//         // Lógica para manejar el inicio del diálogo con el ID proporcionado.
//         Debug.Log("Dialogue triggered: " + dialogueID);
//     }

//     private void OnDialogueEnd(string dialogueID)
//     {
//         // Lógica para manejar el final del diálogo con el ID proporcionado.
//         Debug.Log("Dialogue ended: " + dialogueID);
//     }

//     private void OnShowPauseMenu(bool show)
//     {
//         // Lógica para mostrar u ocultar el menú de pausa.
//         Debug.Log("Show pause menu: " + show);
//     }

//     private void OnShowInventory(bool show)
//     {
//         // Lógica para mostrar u ocultar el inventario.
//         Debug.Log("Show inventory: " + show);
//     }

//     private void OnPlaySound(int soundID)
//     {
//         // Lógica para reproducir el sonido con el ID proporcionado.
//         Debug.Log("Play sound: " + soundID);
//     }

}
}
