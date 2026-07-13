using UnityEngine;
using UnityEngine.InputSystem;
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

    [Header("Wheel Meshes")]
    public Transform FL_Mesh;
    public Transform FR_Mesh;
    public Transform RL1_Mesh;
    public Transform RR1_Mesh;
    public Transform RL2_Mesh;
    public Transform RR2_Mesh;

    [Header("Others")]
    [SerializeField] private GameObject arrows;
    [SerializeField] public GameObject headLights;

    [Header("Bus Settings")]
    public float busTorque = 1500f;
    public float brakeForce = 4000f;
    [SerializeField] private Transform com;
    public bool InReverse = false;
    public Animator animator;

    [Header("Acceleration")]
    [SerializeField] private float accelerationSmoothingSpeed = 5f;
    [SerializeField] public float maxSpeed = 40f;

    [Header("Steering Tuning")]
    [SerializeField] private Transform steerMesh;
    [SerializeField] private float steerMeshMultiplier = 10f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float minSteerAngle = 10f;
    public float steerSpeed = 5;

    [Header("Fuel System")]
    public float maxFuel = 100f;
    public float currentFuel;
    public bool isEngineOn = true;

    public float idleConsumption = 0.02f;
    public float runningConsumption = 0.06f;

    private bool isFuelEmpty = false;
    private bool throtle_Press;
    private bool brake_Press;
    private float steer_Input = 0;
    private float currentTorque;
    private Rigidbody rb;
    private Quaternion initialSteerRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (com != null)
        {
            rb.centerOfMass = com.localPosition;
        }
        animator = GetComponent<Animator>();
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameplaySceneLoaded(this);
        }
        currentFuel = maxFuel;

        if (steerMesh != null)
        {
            initialSteerRotation = steerMesh.localRotation;
        }
        OnOffDirectionIndictor(false);
        if (headLights != null) headLights.SetActive(false);
    }

    void Update()
    {
        InReverse = GameManager.instance.gear == Gear.Reverse;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        isFuelEmpty = currentFuel <= 0;
        if (isEngineOn && !isFuelEmpty)
        {
            currentFuel -= throtle_Press ? runningConsumption * Time.deltaTime : idleConsumption * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isFuelEmpty || !isEngineOn)
        {
            RL1.motorTorque = 0;
            RR1.motorTorque = 0;
            FL.brakeTorque = brakeForce * 0.1f;
            FR.brakeTorque = brakeForce * 0.1f;
            RL1.brakeTorque = brakeForce * 0.1f;
            RR1.brakeTorque = brakeForce * 0.1f;
            HandleBrakes();
            HandleSteering();
            HandleWheels();
            AudioSource source = GetComponentInChildren<AudioSource>();
            if (source != null)
            {
                source.Stop();
            }
            return;
        }
        HandleThrotle();
        HandleBrakes();
        HandleSteering();
        HandleWheels();
    }

    void HandleThrotle()
    {
        if (!throtle_Press || brake_Press)
        {
            RL1.motorTorque = 0;
            RR1.motorTorque = 0;
            return;
        }

        float speed = rb.linearVelocity.magnitude * 3.6f;
        if (speed > maxSpeed)
        {
            RL1.motorTorque = 0;
            RR1.motorTorque = 0;
            return;
        }

        float direction = (GameManager.instance.gear == Gear.Driving) ? -1 : 1;
        float finalTorque = direction * busTorque;
        ApplyTorque(finalTorque);
    }

    void ApplyTorque(float torque)
    {
        currentTorque = Mathf.Lerp(currentTorque, torque, accelerationSmoothingSpeed * Time.deltaTime);
        RL1.motorTorque = currentTorque;
        RR1.motorTorque = currentTorque;
    }

    void HandleBrakes()
    {
        float appliedBrake = brake_Press ? brakeForce : 0f;
        FL.brakeTorque = appliedBrake;
        FR.brakeTorque = appliedBrake;
        RL1.brakeTorque = appliedBrake;
        RR1.brakeTorque = appliedBrake;
    }

    void HandleSteering()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f;
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, speed);
        float steerAngle = Mathf.Lerp(maxSteerAngle, minSteerAngle, speedFactor);
        float finalSteering = Mathf.Clamp(steer_Input * steerAngle, -maxSteerAngle, maxSteerAngle);

        FL.steerAngle = finalSteering;
        FR.steerAngle = finalSteering;

        if (steerMesh)
        {
            Quaternion baseRotation = (initialSteerRotation != default) ? initialSteerRotation : steerMesh.localRotation;
            steerMesh.localRotation = baseRotation * Quaternion.Euler(0f, 0f, finalSteering * steerMeshMultiplier);
        }
    }

    void HandleWheels()
    {
        UpdateWheels(FL, FL_Mesh);
        UpdateWheels(FR, FR_Mesh);
        UpdateWheels(RL1, RL1_Mesh);
        UpdateWheels(RR1, RR1_Mesh);
        UpdateWheels(RL2, RL2_Mesh);
        UpdateWheels(RR2, RR2_Mesh);
    }

    void UpdateWheels(WheelCollider _wc, Transform _mesh)
    {
        if (_wc == null || _mesh == null) return;
        _wc.GetWorldPose(out Vector3 pos, out Quaternion quat);
        _mesh.position = pos;
        _mesh.rotation = quat;
    }

    // Button methods
    public void ThrotleInput(bool _value) => throtle_Press = _value;
    public void BrakeInput(bool _value) => brake_Press = _value;
    public void SteeringInput(float _value) => steer_Input = _value;
    public void OnOffDirectionIndictor(bool _value) => arrows.SetActive(_value);

    public string GetSpeedInKmPerHour()
    {
        float speedKmh = rb.linearVelocity.magnitude * 3.6f;
        return Mathf.RoundToInt(speedKmh).ToString();
    }

}
