using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState{
	void OnPush();
	void OnPop();
	void OnReturnToTop();
	void OnPressed();
}
public class StateMachine<T> where T : IState{
	Stack<T> states = new Stack<T> ();
	public void Push(T newState){
		if (states.Count > 0) {
			states.Peek ().OnPressed ();
		}
		newState.OnPush ();
		states.Push (newState);
	}
	public T Pop<T>(){
		if (states.Count <= 0) {
			Debug.LogError ("Poping an empty State");
			return default(T);
		}

		var popState = states.Pop ();
		popState.OnPop ();
		if (states.Count > 0) {
			states.Peek ().OnReturnToTop ();
		}
		return (T)(object)popState;
	}
	public T TryGetTopState<T>(){
		if (states.Count <= 0) return default(T);
		return (T)(object)states.Peek ();
	}
}
