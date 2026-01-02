using UnityEngine;
using System;

public class CGameEvent : MonoBehaviour
{

    public static CGameEvent current;

    private void Awake()
    {
        current = this;
    }

    public event Action OnEndWave;

    public event Action OnStartWave;

    public event Action OnEnemiesISDead;

    // public event Action OnPlaySounds;

    public event Action OnWinCondition;

    public event Action OnLoseCondition;


    public void WinCondition()
    {
        if (OnWinCondition != null)
        {
            OnWinCondition();
        }
    }

    public void LoseCondition()
    {
        if (OnLoseCondition != null)
        {
            OnLoseCondition();
        }
    }

    public void StartWave()
    { if (OnStartWave != null) { OnStartWave(); } }

    public void EnemiesISDead()
    {
        if(OnEnemiesISDead != null) { OnEnemiesISDead(); }
    }

    public void EndWave()
    { if (OnEndWave != null) { OnEndWave(); } }


}
