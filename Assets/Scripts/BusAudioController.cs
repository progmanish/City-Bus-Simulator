using UnityEditor;
using UnityEngine;

public class BusAudioController : MonoBehaviour
{
	[Header("Audio Source")]
	public AudioSource engineAudio;
	public AudioSource reverseBeepAudio;

	[Header("Pitch Settings")]
	public float minPitch = 0.6f;
	public float maxPitch = 1.8f;

	[Header("Volume")]
	public float engineVolume = 1.0f;
	public float reverseVolume = 0.6f;

	private BusController _controller;
	private Rigidbody _rigidbody;

	// Start is called before the first frame update
	void Awake()
	{
		_controller = GetComponent<BusController>();
		_rigidbody = GetComponent<Rigidbody>();
	}

    void Start()
    {
		if(engineAudio != null)
		{
			engineAudio.volume = engineVolume;
			engineAudio.pitch = minPitch;
			engineAudio.Play();
		}    
    }

    // Update is called once per frame
    void Update()
	{
		if (GameManager.instance == null) return;

		if (GameManager.instance.state != GameState.Driving || _controller.currentFuel <= 0f || !_controller.isEngineOn)
		{
			PauseAudio();
			return;
		}
		ResumeAudio();
		UpdateEngineAudio();
		HandleReverseBeep();
	}

	void UpdateEngineAudio()
	{
		if (engineAudio != null && _controller != null && _rigidbody != null)
		{
			float _speed = _rigidbody.linearVelocity.magnitude * 3.6f;
			float _speedFactor = Mathf.InverseLerp(0, _controller.maxSpeed, _speed);
			engineAudio.pitch = Mathf.Lerp(minPitch, maxPitch, _speedFactor);
		}
	}

	void HandleReverseBeep()
	{
		if (reverseBeepAudio == null || _controller == null) return;

		if (_controller.InReverse)
		{
			if (!reverseBeepAudio.isPlaying)
			{
				reverseBeepAudio.volume = reverseVolume;
				reverseBeepAudio.Play();
			}
		}
		else
		{
			if (reverseBeepAudio.isPlaying)
			{
				reverseBeepAudio.Stop();
			}
		}
	}

	void PauseAudio()
	{
		if (engineAudio != null && engineAudio.isPlaying)
			engineAudio.Pause();
		if (reverseBeepAudio != null && reverseBeepAudio.isPlaying)
			reverseBeepAudio.Pause();
	}

	void ResumeAudio()
	{
        if (engineAudio != null && !engineAudio.isPlaying)
            engineAudio.Play();
        if (reverseBeepAudio != null && _controller != null && _controller.InReverse && !reverseBeepAudio.isPlaying)
            reverseBeepAudio.Play();
    }

	public void EngineStartOff(bool active)
	{
		if (_controller != null)
		{
			_controller.isEngineOn = active;
		}
	}
}