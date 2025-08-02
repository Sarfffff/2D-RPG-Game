using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//״̬��
public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState {  get; private set; }  //�ɶ�����д
    public void Initialize(PlayerState _startState)  //��ʼ����״̬
    {
        currentState = _startState;
        currentState.Enter();

    }
    public void ChangeState(PlayerState _newState) //���ڸı�״̬
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
     void Update()
    {
        
    }

}
