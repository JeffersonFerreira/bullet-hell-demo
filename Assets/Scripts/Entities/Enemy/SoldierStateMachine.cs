using System;
using System.Collections;
using System.Collections.Generic;
using Guns;
using JetBrains.Annotations;
using UnityEngine;

namespace Entity.Enemy
{
	public delegate Coroutine CoroutineWrapper(IEnumerator enumerator);

	public class SoldierStateMachine
	{
		private readonly SharedData _data;
		private readonly CoroutineWrapper _coroutineWrapper;
		private readonly Dictionary<Type, StateBase> _stateMap = new();

		private StateBase _currentState;

		public SoldierStateMachine(SharedData data, CoroutineWrapper wrapper)
		{
			_data = data;
			_coroutineWrapper = wrapper;

			AddState<MovementState>();
			AddState<AttackState>();

			SetState<MovementState>();
		}

		private void AddState<TState>() where TState : StateBase, new()
		{
			var state = new TState();
			state.Configure(this, _data, _coroutineWrapper);

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
			_currentState.Tick();
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


	public class StateBase
	{
		protected SharedData Data;
		private SoldierStateMachine _stateMachine;
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

		public void Configure(SoldierStateMachine stateMachine, SharedData data, CoroutineWrapper coroutineWrapper)
		{
			Data = data;
			_stateMachine = stateMachine;
			_coroutineWrapper = coroutineWrapper;
		}

		protected Coroutine StartCoroutine(IEnumerator coroutine)
		{
			return _coroutineWrapper.Invoke(coroutine);
		}
	}
}