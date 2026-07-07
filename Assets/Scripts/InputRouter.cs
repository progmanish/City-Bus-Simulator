using UnityEngine;

public class InputRouter : MonoBehaviour
{
    public void SetSteering(float steeringAmount) => GameManager.instance.SetSteering(steeringAmount);
    public void SetThrottle(bool throttle) => GameManager.instance.SetThrottle(throttle);
    public void SetBreke(bool breke) => GameManager.instance.SetBreaks(breke);
    public void SetFuelInput(bool fuelInput) => GameManager.instance.ReadFuelInput(fuelInput);

}