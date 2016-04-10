using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finite State Machine that contains numerous states and is controlled through the use of transitions.  Should use Enum's for State keys, Enums for Transition keys, and a Concrete State pattern for States
/// </summary>
public class Fsm<TStateEnum, TStateAction, TConcreteState> where TConcreteState : IFsmState
{
    #region Private Variables

    //A Dictionary of keys that will be enums and values that will be a concrete state pattern.
    private readonly Dictionary<TStateEnum, TConcreteState> _states = new Dictionary<TStateEnum, TConcreteState>();

    // A Dictionary of keys that will be state enums and values that will be dictionaries, lets call them DictionaryB's.
    // Each DictionaryB's will have key that will be Transition enums, and will have a value of class inheriting from the abstract
    // class FsmTransition. FsmTransition must also contain refrence to the StateEnum, as it will contiain the key of the state to
    // exit to.
    private Dictionary<TStateEnum, Dictionary<TStateAction, FsmTransition<TStateEnum>>> _transitions =
        new Dictionary<TStateEnum, Dictionary<TStateAction, FsmTransition<TStateEnum>>>();

    #endregion

    #region Auto Properties

    public TConcreteState CurrentState { get; private set; }

    public TStateEnum CurrentStateName { get; private set; }

    public int StateCount { get { return _states.Count; } }

    #endregion

    #region State Methods

    /// <summary>
    /// Adds a State to the State Machine
    /// </summary>
    /// <param name="key">The key for this state</param>
    /// <param name="state">The State for this key</param>
    public void AddState(TStateEnum key, TConcreteState state)
    {
        _states.Add(key, state);
        _transitions.Add(key, new Dictionary<TStateAction, FsmTransition<TStateEnum>>());
    }

    /// <summary>
    /// Removes the state from the state machine with the given key
    /// </summary>
    /// <param name="key">The key to search for to remove</param>
    public void RemoveState(TStateEnum key)
    {
        _states.Remove(key);
    }

    /// <summary>
    /// Sets the initial state of this state machine.  If a second state is added, it will be ignored.
    /// </summary>
    /// <param name="key">The states key that will be the initial state.</param>
    public void SetInitialState(TStateEnum key)
    {
        //If the fsm does not contain this key
        if (!_states.ContainsKey(key))
        {
            if (Debug.isDebugBuild)
                Debug.LogError("State Machine Does not contian the key: " + key);
            return;
        }

        //If this is not the first state added
        if (CurrentState != null)
        {
            if (Debug.isDebugBuild)
                Debug.LogError("A State Machine is having its initial state set more then once.");
            return;
        }

        CurrentState = _states[key];
        CurrentStateName = key;
        CurrentState.OnEntry();
    }

    [Obsolete("SetCurrentState is depricated.  Please use TriggerTransition instead")]
    /// <summary>
    /// Sets the current state to the state with the corresponding key
    /// </summary>
    /// <param name="key">The key to search for</param>
    public void SetCurrentState(TStateEnum key)
    {
        //If the fsm does not contain this key
        if (!_states.ContainsKey(key))
        {
            Debug.LogError("State Machine Does not contian the key: " + key);
            return;
        }

        //If this is not the first state added
        if (CurrentState != null)
            CurrentState.OnExit();

        CurrentState = _states[key];
        CurrentStateName = key;
        CurrentState.OnEntry();
    }

    [Obsolete("SetCurrentStateIf is depricated.  Please use TriggerTransition instead")]
    /// <summary>
    /// Sets the current state to the state with the corresponding state if chekFunc returns true
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="checkFunc">The function that will run and if true, changes the state</param>
    public void SetCurrentStateIf(TStateEnum key, Func<bool> checkFunc)
    {
        if (checkFunc())
            SetCurrentState(key);
    }

    [Obsolete("SetCurrentStateIf is depricated.  Please use TriggerTransition instead")]
    /// <summary>
    /// Sets the current state to the state with the corresponding state if checkBool returns true
    /// </summary>
    /// <param name="key">The key to search for</param>
    /// <param name="checkBool">If true, allows the state to change</param>
    public void SetCurrentStateIf(TStateEnum key, bool checkBool)
    {
        if (checkBool)
            SetCurrentState(key);
    }


    /// <summary>
    /// Sets the state of the state machine to the passed state.
    /// </summary>
    /// <param name="key">Key.</param>
    private void SetState(TStateEnum key)
    {
        // If the fsm does not contain this key...
        if (!_states.ContainsKey(key))
        {
            // ...Then log an error to the developer...
            Debug.LogError("State Machine - Does not contian the key: " + key);

            // ...And return out of the SetState method
            return;
        }

        // The current State gets set to the new state
        CurrentState = _states[key];

        // Set the State name to the passed key
        CurrentStateName = key;

        // And call that states OnEntry method
        CurrentState.OnEntry();
    }


    /// <summary>
    /// Clears the State Machine
    /// </summary>
    public void Clear()
    {
        _states.Clear();
        _transitions.Clear();
    }

    #endregion

    #region Transition Methods

    /// <summary>
    /// Adds a transition between the two states.  This transition will not have
    /// and sort of action, simply just changes the states.
    /// </summary>
    /// <param name="prevState">The previous state the state machine was in</param>
    /// <param name="destinationState">The destination state the state machine will end in</param>
    /// <param name="stateAction">The key of the transition, this will be an action of some sort.  ex. Jump</param>
    /// <param name="transition">The transition class implimented for this transition</param>
    ///
    public void AddTransition(TStateEnum prevState, TStateEnum destinationState, TStateAction stateAction,
        FsmTransition<TStateEnum> transition)
    {
        //Get the transitions for the prevState
        var cStateTrans = _transitions[prevState];

        //If the Transition Dictionary does not have the current state's transitions...
        if (cStateTrans == null)
        {
            // ...Then log an error to inform a developer...
            Debug.LogError("Current State for transition not found");

            // ...and exit out of the add transition method.
            return;
        }

        // If the Transition Dictionary does not contain the passed Transition Key...
        if (!cStateTrans.ContainsKey(stateAction))
        {
            // ...add it to the transition Dictionary, as well as the new transition.
            cStateTrans.Add(stateAction, transition);
        }
        else
        {
            // ...else Log a warning to the developer that it already exists...
            Debug.LogWarning("Transition: " + stateAction + " - Overwritten");

            // ...and override it with the new transition.
            cStateTrans[stateAction] = transition;
        }
    }

    /// <summary>
    /// Adds a transition between the two states.  This transition will not have
    /// and sort of action, simply just changes the states.
    /// </summary>
    /// <param name="prevState">The previous state the state machine was in</param>
    /// <param name="destinationState">The destination state the state machine will end in</param>
    /// <param name="stateAction">The key of the transition, this will be an action of some sort.  ex. Jump</param>
    ///
    public void AddTransition(TStateEnum prevState, TStateEnum destinationState, TStateAction stateAction)
    {
        AddTransition(prevState, destinationState, stateAction, new EmptyTransition(destinationState));
    }

    /// <summary>
    /// Adds a transition between the two states.  This transition will need to have an action inclduded.
    /// This action will be performed between the two states running.
    /// </summary>
    /// <param name="prevState">The previous state the state machine was in</param>
    /// <param name="destinationState">The destination state the state machine will end in</param>
    /// <param name="stateAction">The key of the transition, this will be an action of some sort.  ex. Jump</param>
    /// <param name="actionPerformed">This action will be performed between the state change</param>
    ///
    public void AddTransition(TStateEnum prevState, TStateEnum destinationState, TStateAction stateAction,
        System.Action actionPerformed)
    {
        AddTransition(prevState, destinationState, stateAction,
            new ActionTransition(destinationState, actionPerformed));
    }

    /// <summary>
    /// An empty transition class, this is used for a no-arg for the transition.  Simply, this will just go from one state to another.
    /// </summary>
    public class EmptyTransition : FsmTransition<TStateEnum>
    {
        public EmptyTransition(TStateEnum destinationStateEnum) : base(destinationStateEnum)
        {
        }

        public override void OnTransition()
        {
            //Do Nothing
        }
    }

    /// <summary>
    /// An Action Transition class, this is used for users who would like to pass an action instead of implimenting the abstract class
    /// FsmTransition.  That way, a user could use lambdas without having to impliment a lot of there own code for more simple transitions.
    /// </summary>
    public class ActionTransition : FsmTransition<TStateEnum>
    {
        private System.Action _onTransitionAction;

        /// <summary>
        /// Contructor, this will transfer the passed action to a private field
        /// </summary>
        /// <param name="destinationStateEnum">The destination this transition should end up at</param>
        /// <param name="actionToPerform">Action that is performed on transition</param>
        public ActionTransition(TStateEnum destinationStateEnum, System.Action actionToPerform)
            : base(destinationStateEnum)
        {
            _onTransitionAction = actionToPerform;
        }

        /// <summary>
        /// On Transition is called when a state machine is between two states
        /// </summary>
        public override void OnTransition()
        {
            _onTransitionAction.Run();
        }
    }


    /// <summary>
    /// Triggers the transition with the matching key.  Performs the onExit, then the transition, and then enters the new state.
    /// </summary>
    /// <param name="key">Key of the transition to perform.</param>
    public void TriggerTransition(TStateAction key)
    {
        //If the current state is null, meaning there is no state set yet...
        if (CurrentState == null)
        {
            // ...Then log an error to the developer...
            Debug.LogError("Current State has not yet been set");

            // ...and return out of the trigger tranition method.
            return;
        }

        // Get the transitions for this current state.
        var cStateTrans = _transitions[CurrentStateName];

        // If the transitions for this state do not exists...
        if (cStateTrans == null)
        {
            // ...Then log an error to the developer...
            Debug.LogError("Current State for transition not found");

            // ...and return out of the trigger transition method.
            return;
        }

        if (!cStateTrans.ContainsKey(key))
            return;

        // Get the transition with the given key, ex. MoveFaster
        var transition = cStateTrans[key];

        // If the transition does not exist...
        if (transition == null)
        {
            // ...Then log a warning to the developer that the transition is not attached to this state...
            Debug.LogWarning("Current State does not have transition: " + key);

            // ...And return out of the Trigger Transition method.
            return;
        }

        // Call the current states On Exit.
        CurrentState.OnExit();

        // Then call the found transition's On Transition.
        transition.OnTransition();

        //And then Switch the current state to the state attached to the end of the Transition
        SetState(transition.DestinationState);
    }

    #endregion
}