using UnityEngine;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
	[Header("Wheel Collider")]
	public WheelCollider FL;
	public WheelCollider FR;
	public WheelCollider RL1;
	public WheelCollider RR1;
	public WheelCollider RL2;
	public WheelCollider RR2;

	[Header("Bus Settings")]
	public float busTorque = 1500f;
	public float steerAngle = 30f;
	public float brakeForce = 4000f;

	[Header("Gear Box")]
	public Slider gearBox;

	private bool throtle_Press;
	private bool brake_Press;
	private float steer_Input = 0;

	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		HandleThrotle();
		HandleBrakes();
		HandleSteering();
	}

	void HandleThrotle()
	{
		if (!throtle_Press || brake_Press || gearBox == null)
		{
			RL1.motorTorque = 0;
			RR1.motorTorque = 0;
			return;
		}

		float direction = (gearBox.value == 0) ? 1 : -1;
		float finalTorque = direction * busTorque;

		RL1.motorTorque = finalTorque;
		RR1.motorTorque = finalTorque;
	}

	void HandleBrakes()
	{
		if (!brake_Press)
		{
			FL.brakeTorque = 0;
			FR.brakeTorque = 0;
			RL1.brakeTorque = 0;
			RR1.brakeTorque = 0;
		} 
		else
		{
			FL.brakeTorque = brakeForce;
			FR.brakeTorque = brakeForce;
			RL1.brakeTorque = brakeForce;
			RR1.brakeTorque = brakeForce;
		}
	}

	void HandleSteering()
	{
	    float finalSteering = steer_Input * steerAngle;

		FL.steerAngle = finalSteering;
		FR.steerAngle = finalSteering;
	}

    public void ThrotlePress() => throtle_Press = true;
    public void ThrotleRelease() => throtle_Press = false;
    public void BrakePress() => brake_Press = true;
    public void BrakeRelease() => brake_Press = false;
    public void leftSteer() => steer_Input = -1f;
	public void rightSteer() => steer_Input = 1f;
	public void SteerRelease() => steer_Input = 0;
}