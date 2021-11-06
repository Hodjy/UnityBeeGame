using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_PlayerRb;
    private GameJamGameActions m_InputActions;
    private RotateByLookInput m_CameraRotator;
    //[SerializeField] private GameObject m_PlayerCamera;
    //[SerializeField] private Vector3 m_CameraOffset = new Vector3(0,0.65f,-8);
    //[SerializeField] private float m_LookRotateSpeed = 5f;

    [Header("Camera")]
    [SerializeField] private Transform m_CameraAngle;

    [Header("Movement")]
    [SerializeField] private float m_MovementSpeed = 5f;
    [SerializeField] private float m_MovementTurnSmoothTime = 0.5f;
    private Vector3 m_PlayerTurnDirection;

    [Header("Abilities")]
    [SerializeField] private float m_FlyBoostPower = 5;
    
    private bool m_IsFlying = false; // TODO check if needed. If not, remove.

    private void Awake()
    {
        m_InputActions = new GameJamGameActions();
        m_InputActions.Player.Enable();
        m_PlayerTurnDirection = transform.forward;
        m_CameraRotator = GetComponentInChildren<RotateByLookInput>();
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
        Vector2 input = m_InputActions.Player.Look.ReadValue<Vector2>();
        m_CameraRotator.rotateCameraFollowByLookInput(input);
    }

    private void rotateByLookInput()
    {
        //Vector2 lookDirection = m_InputActions.Player.Look.ReadValue<Vector2>();
        //gameObject.transform.Rotate(Vector3.up, lookDirection.x * m_LookRotateSpeed * Time.deltaTime);
        //gameObject.transform.Rotate(Vector3.right, lookDirection.y * m_LookRotateSpeed * Time.deltaTime);
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

    //TODO try and see a more efficient way to perform this. in addition, might cause rotation/movement bug later.
    private void movePlayerByInput()
    {
        Vector2 userInput = m_InputActions.Player.Move.ReadValue<Vector2>().normalized;
        Vector3 moveDirection = new Vector3(0,0,0);
        float directionStrafeAngle = userInput.x * 90;

        // If forward/back input, generate direction with y. 
        // Else try to generate strafe from empty y camera forward vector.
        if (userInput.y != 0) 
        {
            moveDirection = userInput.y * m_CameraAngle.forward.normalized * m_MovementSpeed;
        }
        else if (userInput.x != 0)
        {
            moveDirection = m_CameraAngle.forward.normalized * m_MovementSpeed;
            moveDirection.y = 0;
        }

        moveDirection = Quaternion.Euler(0, directionStrafeAngle, 0) * moveDirection;
        
        if (moveDirection.magnitude != 0)
            m_PlayerTurnDirection = moveDirection;

        m_PlayerRb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_PlayerTurnDirection),
           m_MovementTurnSmoothTime));
        m_PlayerRb.velocity = moveDirection;
    }

    private void launchPlayerUpIfFlying()
    {
        if(m_IsFlying)
        {
            m_PlayerRb.AddForce(Vector3.up * m_FlyBoostPower, ForceMode.Acceleration);
        }
    }


}
