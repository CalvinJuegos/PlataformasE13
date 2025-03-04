using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State CurrentState => _currentState;
    State _currentState;
    private bool inTransition;

    // Start is called before the first frame update
    void Start()
    {
        //ChangeState<>(initialState);
    }

    public void ChangeState<T>() where T:State{
        T targetState = GetComponent<T>();
        if (targetState == null)
        {
            print("Tried to change to null state");
            return;
        }
        InitiateNewState(targetState);
    }

    public void InitiateNewState(State targetState){
        if (_currentState != targetState && !inTransition){
            CallNewState(targetState);
        }
    }

    void CallNewState(State newState){
        inTransition = true;

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();

        inTransition = false;
    }

    private void Update(){
            if(CurrentState != null && !inTransition){
                CurrentState.Tick();
            }
    }
}
