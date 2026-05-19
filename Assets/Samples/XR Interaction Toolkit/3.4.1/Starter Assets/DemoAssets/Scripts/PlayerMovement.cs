using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class XRSprint : MonoBehaviour
{
    public ContinuousMoveProvider moveProvider;

    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveProvider.moveSpeed = sprintSpeed;
        }
        else
        {
            moveProvider.moveSpeed = walkSpeed;
        }
    }
}