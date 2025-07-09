using UnityEngine;

public class GirlAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    // 动画参数名称
    private const string SPEED_PARAM = "Speed";
    private const string IS_RUNNING_PARAM = "IsRunning";
    private const string IS_JUMPING_PARAM = "IsJumping";
    private const string GROUNDED_PARAM = "Grounded";

    // 移动参数
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found!");
        }
    }

    void Update()
    {
        // 检查是否在地面
        isGrounded = characterController.isGrounded;
        animator.SetBool(GROUNDED_PARAM, isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 轻微向下力确保角色稳定在地面
        }

        // 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // 判断是否在跑步
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        animator.SetBool(IS_RUNNING_PARAM, isRunning);

        // 计算移动速度
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveVelocity = moveDirection * currentSpeed;

        // 设置动画速度参数 (0-1范围)
        float animationSpeed = moveVelocity.magnitude / runSpeed;
        animator.SetFloat(SPEED_PARAM, animationSpeed);

        // 移动角色
        if (moveDirection.magnitude >= 0.1f)
        {
            // 旋转角色朝向移动方向
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            characterController.Move(moveVelocity * Time.deltaTime);
        }

        // 跳跃控制
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger(IS_JUMPING_PARAM);
        }

        // 应用重力
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}