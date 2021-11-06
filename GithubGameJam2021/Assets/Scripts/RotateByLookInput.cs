using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByLookInput : MonoBehaviour
{
    [SerializeField] private float m_CameraRotationSpeed = 5f;
    [SerializeField] private bool m_YInverted = true;

    private Vector3 m_NextPosition = new Vector3();
    private Quaternion m_NextRotation = new Quaternion();
    private float m_RotationLerp = 1f;

    public void rotateCameraFollowByLookInput(Vector2 i_UserInput)
    {
        i_UserInput.Normalize();

        if(m_YInverted)
        {
            i_UserInput.y *= -1;
        }

        transform.rotation *= Quaternion.AngleAxis(i_UserInput.x * m_CameraRotationSpeed, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(i_UserInput.y * m_CameraRotationSpeed, Vector3.right);

        Vector3 angles = transform.localEulerAngles;
        angles.z = 0;
        float xAngle = transform.localEulerAngles.x;

        if(xAngle > 180 && xAngle < 340)
        {
            angles.x = 340;
        }
        else if(xAngle < 180 && xAngle > 40)
        {
            angles.x = 40;
        }

        transform.localEulerAngles = angles;

        m_NextRotation = Quaternion.Lerp(transform.rotation, m_NextRotation, Time.deltaTime * m_RotationLerp);

        

       
    }
}
