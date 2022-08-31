using System;
using System.Collections;
using System.Collections.Generic;
using Guns;
using JetBrains.Annotations;
using UnityEngine;

namespace Entity.Enemy
{
	public delegate Coroutine CoroutineWrapper(IEnumerator enumerator);

	public class BasicStateMachine
	{
		private readonly SharedData _data;
		private readonly CoroutineWrapper _coroutineWrapper;
		private readonly Dictionary<Type, StateBase> _stateMap = new();

		private StateBase _currentState;

		public BasicStateMachine(SharedData data, CoroutineWrapper wrapper)
		{
			_data = data;
			_coroutineWrapper = wrapper;
		}

		public void AddState<TState>(object props = null) where TState : StateBase, new()
		{
			var state = new TState();
			state.Configure(this, _coroutineWrapper, _data, props);

			_stateMap[typeof(TState)] = state;
		}

		public void AddState<TState, TProps>(TProps props) where TState : StateBase<TProps>, new()
		{
			var state = new TState();
			state.Configure(this, _coroutineWrapper, _data, props);

			_stateMap[typeof(TState)] = state;
		}

		public void SetState<TState>() where TState : StateBase
		{
			Type type = typeof(TState);

			if (_stateMap.TryGetValue(type, out StateBase nextState))
			{
				_currentState?.OnExit();
				_currentState = nextState;
				nextState.OnEnter();
			}
			else
			{
				Debug.LogError($"State of type \"{type}\" not registered");
			}
		}

		public void Tick()
		{
			_currentState?.Tick();
		}
	}

	public class SharedData
	{
		public float MoveSpeed;

		public Transform Transform;
		public CharacterController CharacterController;

		[CanBeNull]
		public GunBase Gun;

		public Transform PlayerTransform;
	}

	public abstract class StateBase<TProps> : StateBase
	{
		protected new TProps Props
		{
			get => (TProps) base.Props;
			set => base.Props = value;
		}
	}

	public abstract class StateBase
	{
		protected SharedData Data { get; set; }
		protected object Props { get; set; }

		private BasicStateMachine _stateMachine;
		private CoroutineWrapper _coroutineWrapper;

		public virtual void Tick()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnExit()
		{
		}

		public void SetState<TNext>() where TNext : StateBase
		{
			_stateMachine.SetState<TNext>();
		}

		public void Configure(BasicStateMachine stateMachine, CoroutineWrapper coroutineWrapper, SharedData data, object props)
		{
			Data = data;
			Props = props;
			_stateMachine = stateMachine;
			_coroutineWrapper = coroutineWrapper;
		}

		protected Coroutine StartCoroutine(IEnumerator coroutine)
		{
			return _coroutineWrapper.Invoke(coroutine);
		}
	}
}