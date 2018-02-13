﻿public class Interval {

	private int iterations;
	
	private float _stack;
	private float _interval;
	
	private OnUpdate _listener;

	private bool _canUpdate;
	
	// Use this for initialization
	public Interval(OnUpdate listener, float interval) {
		_listener = listener;
		_interval = interval;

		iterations = 0;
		
		if(_canUpdate)
			_listener(iterations);
	}

	public void Update(float deltaTime) {
	
		// Stops if can't update
		if(!_canUpdate) return;
		
		_stack += deltaTime;

		if (_stack < _interval) return;
		
		iterations++;
		_listener(iterations);
		_stack = 0;

	}

	public void Play() {
		_canUpdate = true;
	}

	public void Pause() {
		_canUpdate = false;
	}

	public delegate void OnUpdate(int iterations);
}
