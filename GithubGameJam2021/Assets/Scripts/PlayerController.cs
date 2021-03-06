using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using AudioSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_PlayerRb;
    private GameJamGameActions m_InputActions;
    [SerializeField] private GameObject m_PlayerMesh;
    private Animator m_PlayerAnimator;
    private bool m_IsMovementAvailable = true;
    //[SerializeField] private GameObject m_PlayerCamera;
    //[SerializeField] private Vector3 m_CameraOffset = new Vector3(0,0.65f,-8);
    //[SerializeField] private float m_LookRotateSpeed = 5f;

    [Header("Camera")]
    [SerializeField] private Transform m_CameraAngle;
    [SerializeField] private GameObject m_CinemachineVCam;
    private RotateByLookInput m_CameraRotator;
    private Cinemachine3rdPersonFollow m_Cinemachine3rdPersonFollow;
    private Vector3 m_BaseCameraDamping;
    private float m_BaseCameraDistance;

    [Header("Audio")]
    [SerializeField] private AudioManager m_AudioManager;
    private string m_SpeedBoostName = "SpeedBoost";

    [Header("Movement")]
    [SerializeField] private float m_MovementSpeed = 5f;
    private float m_BaseMovementSpeed;
    [SerializeField] private float m_MovementTurnSmoothTime = 0.5f;
    [SerializeField] private float m_MovementSpeedBoostPower = 5f;
    [SerializeField] private float m_MovementSpeedBoostTime = 5f;
    private Vector3 m_PlayerTurnDirection;

    [Header("Abilities")]
    [SerializeField] private float m_FlyBoostPower = 5;

    [Header("PlayerEffects")]
    [SerializeField] private TrailRenderer m_SpeedBoostEffect;

    private bool m_IsFlying = false; // TODO check if needed. If not, remove.

    public bool IsMovementAvailable { get => m_IsMovementAvailable; set => m_IsMovementAvailable = value; }

    private void Awake()
    {
        initVars();
    }

    private void initVars()
    {
        m_InputActions = new GameJamGameActions();
        m_InputActions.Player.Enable();
        m_PlayerTurnDirection = transform.forward;
        m_CameraRotator = GetComponentInChildren<RotateByLookInput>();
        m_BaseMovementSpeed = m_MovementSpeed;
        m_Cinemachine3rdPersonFollow = m_CinemachineVCam.GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        m_BaseCameraDamping = m_Cinemachine3rdPersonFollow.Damping;
        m_BaseCameraDistance = m_Cinemachine3rdPersonFollow.CameraDistance;
    }

    void Start()
    {
        m_PlayerRb = GetComponent<Rigidbody>();
        m_PlayerAnimator = m_PlayerMesh.GetComponentInChildren<Animator>();
        subscribeToInputActions();
        Cursor.visible = false;
        //m_PlayerCamera.transform.position = gameObject.transform.position + m_CameraOffset;
    }

    // Update is called once per frame, for rigidbodys.
    void FixedUpdate()
    {
        if (m_IsMovementAvailable)
        {
            movePlayerByInput();
        }
    }

    private void Update()
    {
        //rotateByLookInput();
        Vector2 input = m_InputActions.Player.Look.ReadValue<Vector2>();
        m_CameraRotator.rotateCameraFollowByLookInput(input);
        m_PlayerAnimator.SetFloat("f_Speed", m_PlayerRb.velocity.magnitude);
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
        m_InputActions.Player.Quit.performed += quitGame;
        //m_InputActions.Player.Move.performed += cxt => movePlayer(cxt.ReadValue<Vector2>());
    }

    private void quitGame(InputAction.CallbackContext i_CallbackContext)
    {
        Application.Quit();
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
            directionStrafeAngle *= userInput.y < 0 ? -1 : 1; 
            moveDirection = userInput.y * m_CameraAngle.forward.normalized * m_MovementSpeed;
        }
        else if (userInput.x != 0)
        {
            moveDirection = m_CameraAngle.forward.normalized * m_MovementSpeed;
            moveDirection.y = 0;
        }

        //Apply the X direction to go towards to into the moveDirection
        moveDirection = Quaternion.Euler(0, directionStrafeAngle, 0) * moveDirection;
        
        if (moveDirection.magnitude != 0)
        {
            m_PlayerTurnDirection = moveDirection;
        }   

        m_PlayerMesh.transform.rotation = Quaternion.Slerp(m_PlayerMesh.transform.rotation, Quaternion.LookRotation(m_PlayerTurnDirection),
           m_MovementTurnSmoothTime);
        m_PlayerRb.velocity = moveDirection;
    }

    private void launchPlayerUpIfFlying()
    {
        if(m_IsFlying)
        {
            m_PlayerRb.AddForce(Vector3.up * m_FlyBoostPower, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Naive implementation, should be implemented as a state. 
    /// if there will be more states for the player, we should consider re-implementing this.
    /// </summary>
    public void Knockbacked(Vector3 i_ForceDirection)
    {
        float timeToKnockback = 0.7f;
        float forceStrength = 15;
        enableMovement(false);
        StartCoroutine("KnockbackRotation", timeToKnockback);
        m_PlayerRb.velocity = new Vector3(0, 0, 0);
        m_PlayerRb.AddForce(i_ForceDirection.normalized * forceStrength, ForceMode.Impulse);
    }

    IEnumerator KnockbackRotation(float i_TimeToRotate)
    {
        Quaternion startRot = m_PlayerMesh.transform.rotation;
        //float endRot = startRot + 360f;
        float currentRot;
        float time = 0.0f;

        while (time < i_TimeToRotate)
        {
            time += Time.deltaTime;

            m_PlayerMesh.transform.rotation = startRot * Quaternion.AngleAxis(time / i_TimeToRotate * -360f, Vector3.right);

            yield return null;
        }

        m_PlayerRb.velocity = new Vector3(0, 0, 0);
        enableMovement(true);
    }

    private void enableMovement(bool i_IsEnabled)
    {
        m_IsMovementAvailable = i_IsEnabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Speed Boost"))
        {
            m_AudioManager.PlaySfx(m_SpeedBoostName);
            StopCoroutine("startMovementBoost");
            StartCoroutine("startMovementBoost");
        }
    }

    IEnumerator startMovementBoost()
    {
        startMoveBoost();

        //StopCoroutine("moveCameraDistance");
        //StartCoroutine("moveCameraDistance", boostCamDistance);


        yield return new WaitForSeconds(m_MovementSpeedBoostTime);

        stopMoveBoost();

        //StopCoroutine("moveCameraDistance");
        //StartCoroutine("moveCameraDistance", m_BaseCameraDistance);
    }

    private void stopMoveBoost()
    {
        m_MovementSpeed = m_BaseMovementSpeed;
        m_Cinemachine3rdPersonFollow.Damping = m_BaseCameraDamping;
        m_SpeedBoostEffect.enabled = false;
        StartCoroutine("moveCameraDistance", m_BaseCameraDistance);
    }

    private void startMoveBoost()
    {
        m_MovementSpeed = m_BaseMovementSpeed + m_MovementSpeedBoostPower;
        Vector3 damp = m_BaseCameraDamping / 2;
        float boostCamDistance = m_BaseCameraDistance + 10;
        m_Cinemachine3rdPersonFollow.Damping = damp;
        m_SpeedBoostEffect.enabled = true;
        StartCoroutine("moveCameraDistance", boostCamDistance);
    }

    IEnumerator moveCameraDistance(float i_NewDistance)
    {
        float duration = 0.2f;
        float elapsed = 0.0f;
        float currentDis = m_Cinemachine3rdPersonFollow.CameraDistance;
        while (elapsed < duration)
        {
            m_Cinemachine3rdPersonFollow.CameraDistance = Mathf.Lerp(currentDis, i_NewDistance, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }


}
