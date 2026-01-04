using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [field:SerializeField] public PlayerActionState CurrentActionState { get; private set; } = PlayerActionState.Idle;
    
    public void SetPlayerState(PlayerActionState newState)
    {
        CurrentActionState = newState;
    }
}

public enum PlayerActionState
{
    Idle = 0,
    Running = 1,
    Attacking = 3,
    Evading = 4
}
