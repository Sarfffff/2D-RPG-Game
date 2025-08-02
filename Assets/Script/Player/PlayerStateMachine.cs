using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//状态机
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState {  get; private set; }  //可读不可写
    public void Initialize(PlayerState _startState)  //初始化的状态
    {
        currentState = _startState;
        currentState.Enter();

    }
    public void ChangeState(PlayerState _newState) //用于改变状态
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
     void Update()
    {
        
    }

}
