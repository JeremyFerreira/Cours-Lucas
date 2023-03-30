using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeSTateBehaviour : MonoBehaviour
{
    private RuntimeGameState state;
    private void OnEnable()
    {
        state = new RuntimeGameState();
        GameStateManager.Instance.ApplyState(state);
    }
    private void OnDisable()
    {
        GameStateManager.Instance.RemoveState(state);
    }
}
