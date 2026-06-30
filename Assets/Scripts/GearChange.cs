using UnityEngine;
using UnityEngine.UI;

public class GearChange : MonoBehaviour
{
	public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        SetGear();
    }

    // Update is called once per frame
    void Update()
	{

	}

    public void SetGear()
    {   if (slider != null) GameManager.instance.SetGear(slider.value);
    }
}