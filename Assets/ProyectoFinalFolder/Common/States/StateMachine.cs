using UnityEngine;

namespace ProyectoFinalFolder.Common.States
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState;

        public void Enter()
        {
            currentState?.Enter();
        }

        public void TickCurrentState(float deltaTime)
        {
            currentState?.Tick(deltaTime);
        }

        public void Exit()
        {
            currentState?.Exit();
        }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}