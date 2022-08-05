
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GhostCamera : MonoBehaviour
{
    public float mouseSensitivity = 5f;
    public float speedSensitivity = 20f;
    private float m_deltX = 0f;
    private float m_deltY = 0f;
    private Camera mainCamera;

    
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        m_deltX = mainCamera.transform.rotation.eulerAngles.x;
        m_deltY = mainCamera.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        CamerFreeMove();
        if (Input.GetMouseButton(1))
        {
            CursorVisible(true);
            FollowRotation();
        }
        else
        {
            CursorVisible(false);
        }
    }
    private void CamerFreeMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontal *= 3; vertical *= 3;
        }
        mainCamera.transform.Translate(Vector3.forward * vertical * speedSensitivity * Time.deltaTime);
        mainCamera.transform.Translate(Vector3.right * horizontal * speedSensitivity * Time.deltaTime);
    }

    private void FollowRotation()
    {
        m_deltX += Input.GetAxis("Mouse X") * mouseSensitivity;
        m_deltY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        m_deltX = ClampAngle(m_deltX, -360, 360);
        m_deltY = ClampAngle(m_deltY, -70, 70);
        mainCamera.transform.rotation = Quaternion.Euler(m_deltY, m_deltX, 0);
    }

    private void CursorVisible(bool b)
    {
        //Cursor.lockState = b ? CursorLockMode.Locked : Cursor.lockState = CursorLockMode.None;
        Cursor.visible = b ? false : true;
    }

        float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;
        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
}