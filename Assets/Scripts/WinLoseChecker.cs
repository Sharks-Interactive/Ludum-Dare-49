using Chrio;
using Chrio.Entities;
using UnityEngine;

public class WinLoseChecker : SharksBehaviour
{
    public CanvasGroup GameOverLose;
    public CanvasGroup GameOverWin;

    // Update is called once per frame
    void FixedUpdate()
    {
        int _playerOwned = 0;
        int _enemyOwned = 0;
        Vector2 _drydocks = new Vector2();

        foreach (IBaseEntity ent in GlobalState.Game.Entities.WorldEntities.Values)
            if (ent.GetOwnerID() == 0 && !ent.CompareEntityType("Drydock")) _playerOwned++; else if (ent.GetOwnerID() == 1) _enemyOwned++; else if (ent.CompareEntityType("Drydock") && ent.GetOwnerID() != 2) _drydocks[ent.GetOwnerID()]++;

        if (_playerOwned == 0 && (_drydocks[0] == 0 || GlobalState.Game.Money[0] < 150)) { GameOverLose.alpha = 1.0f; Time.timeScale = 0.0f; }

        if (_enemyOwned == 0 && (_drydocks[1] == 0 || GlobalState.Game.Money[1] < 150)) { GameOverWin.alpha = 1.0f; Time.timeScale = 0.0f; }
    }
}
