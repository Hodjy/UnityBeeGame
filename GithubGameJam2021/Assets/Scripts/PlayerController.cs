using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_PlayerRb;
    private GameJamGameActions m_InputActions;
    //[SerializeField] private GameObject m_PlayerCamera;
    [SerializeField] private Vector3 m_CameraOffset = new Vector3(0,0.65f,-8);
    [SerializeField] private float m_LookRotateSpeed = 5f;

    [Header("Movement")]

    [SerializeField] private float m_FlyPower = 5;
    [SerializeField] private float m_MovementSpeed = 5f;
    [SerializeField] private float m_MovementTurnSmoothTime = 0.1f;
    [SerializeField] private Transform m_CameraAngle;

    private float m_TurnSmoothVelocity;
    private float m_TurnSmoothVelocityX;
    private bool m_IsFlying = false; // TODO check if needed. If not, remove.

    private void Awake()
    {
        m_InputActions = new GameJamGameActions();
        m_InputActions.Player.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerRb = GetComponent<Rigidbody>();
        subscribeToInputActions();
        Cursor.visible = false;
        //m_PlayerCamera.transform.position = gameObject.transform.position + m_CameraOffset;
    }

    // Update is called once per frame, for rigidbodys.
    void FixedUpdate()
    {
        movePlayerByInput();
    }

    private void Update()
    {
        //rotateByLookInput();
        
    }

    private void rotateByLookInput()
    {
        Vector2 lookDirection = m_InputActions.Player.Look.ReadValue<Vector2>();
        gameObject.transform.Rotate(Vector3.up, lookDirection.x * m_LookRotateSpeed * Time.deltaTime);
        gameObject.transform.Rotate(Vector3.right, lookDirection.y * m_LookRotateSpeed * Time.deltaTime);
    }

    private void subscribeToInputActions()
    {
        m_InputActions.Player.Fly.performed += setIsFlying;
        m_InputActions.Player.Fly.canceled += setIsFlying;
        //m_InputActions.Player.Move.performed += cxt => movePlayer(cxt.ReadValue<Vector2>());
    }

    private void setIsFlying(InputAction.CallbackContext i_Context)
    {
        m_IsFlying = i_Context.performed;
        
    }

    private void movePlayerByInput()
    {
        Vector2 userInput = m_InputActions.Player.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(userInput.x, 0, userInput.y).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_CameraAngle.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, m_MovementTurnSmoothTime);

            float xAngle = 0;
            xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, m_CameraAngle.eulerAngles.x,
                ref m_TurnSmoothVelocityX, m_MovementTurnSmoothTime);

            transform.rotation = Quaternion.Euler(xAngle, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Debug.Log(m_CameraAngle.eulerAngles.x);
            //m_PlayerRb.MovePosition(transform.position + direction * m_MovementSpeed * Time.deltaTime);
            m_PlayerRb.velocity = moveDir * m_MovementSpeed;
        }
        else
        {
            //m_PlayerRb.velocity /= m_MovementSpeed;
        }
    }

    private void launchPlayerUpIfFlying()
    {
        if(m_IsFlying)
        {
            m_PlayerRb.AddForce(Vector3.up * m_FlyPower, ForceMode.Acceleration);
        }
    }
}
