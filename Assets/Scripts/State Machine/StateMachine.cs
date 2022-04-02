using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State machine code is adapted from this video: https://www.youtube.com/watch?v=V75hgcsCGOM&t=647s
public class StateMachine
{
    public IState _currentState { get; private set; }
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();
    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            SetState(transition.To);
        }
        _currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
        {
            return;
        }

        _currentState?.OnExit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
        {
            _currentTransitions = EmptyTransitions;
        }

        _currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate, bool transitionToSelf)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate, transitionToSelf));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate, bool transitionToSelf)
    {
        _anyTransitions.Add(new Transition(state, predicate, transitionToSelf));
    }

    private Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
        {
            if (transition.Condition())
            {
                if (!transition.CanTransitionToSelf)
                {
                    if (transition.To.GetType() != _currentState.GetType())
                        return transition;
                }
                else
                {
                    return transition;
                }
            }
        }

        foreach (var transition in _currentTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        return null;
    }


    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }
        public bool CanTransitionToSelf { get; }

        public Transition(IState to, Func<bool> condition, bool transitionToSelf)
        {
            To = to;
            Condition = condition;
            CanTransitionToSelf = transitionToSelf;
        }
    }
}