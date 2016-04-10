using System;
using UnityEngine;

public abstract class State : IState
{
	protected GameObject CurrentObject;
	protected Action<int> SetState; //Allows the State to set the state of the state machine

	/// <summary>
	///		Used to attach the Set State Action, and to get the game Object. This should be called first,
	///		directly by the state machine.
	/// </summary>
	/// <param name="g"></param>
	/// <param name="setState"></param>
	public void StateMachineProperty(GameObject g, Action<int> setState)
	{
		SetState = setState;
		CurrentObject = g;
	}

	public abstract void OnUpdate(GameObject g);

	public abstract void OnStateEnter(GameObject g);

	public abstract void OnStateExit(GameObject g);
}
