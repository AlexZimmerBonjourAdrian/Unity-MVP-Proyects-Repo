using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour
{
    public void Start()
    {
        CGameEvent.current.OnEndWave += EndWave;
        CGameEvent.current.OnStartWave += StartWave;
        CGameEvent.current.OnWinCondition += WinCondition;
        CGameEvent.current.OnLoseCondition += LoseCondition;
        CGameEvent.current.OnEnemiesISDead += EnemiesIsDead;

    }

    public void StartWave()
    {

    }
    public void EnemiesIsDead()
    {

    }

    public void WinCondition()
    {

    }

    public void LoseCondition()
    {

    }

    public void EndWave()

    {

    }
}
