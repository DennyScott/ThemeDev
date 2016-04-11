using UnityEngine;
using System.Collections;

public abstract class FsmTransition<TStateEnum>
{
    public TStateEnum DestinationState;

    protected FsmTransition(TStateEnum destinationStateEnum)
    {
        DestinationState = destinationStateEnum;
    }  
	

	public abstract void OnTransition();

}
