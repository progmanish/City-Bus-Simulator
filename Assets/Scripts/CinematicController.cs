using UnityEngine;
using UnityEngine.InputSystem;

public class CinematicController : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        // Press 1 → set int parameter to 1
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            animator.SetInteger("shot", 1);
        }

        // Press 2 → set int parameter to 2
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            animator.SetInteger("shot", 2);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            animator.SetInteger("shot", 3);
        }
    }
}
