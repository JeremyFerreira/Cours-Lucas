using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    private PauseGameState state;
    private void OnEnable()
    {
        state = new PauseGameState();

        GameStateManager.Instance.RegisterCallback(this);
        GameStateManager.Instance.ApplyState(state);
    }
    private void OnDisable()
    {
        GameStateManager.Instance.RemoveState(state);
        GameStateManager.Instance.UnRegisterCallback(this);
    }

    public void OnApplyGameStateOverride(GameStateOverride stateOverride)
    {
        Debug.Log("Update => " + stateOverride.isPaused);
    }
}
