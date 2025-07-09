using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float walkSpeed = 5f;      // 行走速度
    [SerializeField] private float runSpeed = 8f;      // 跑步速度
    [SerializeField] private float jumpHeight = 1.5f;   // 跳跃高度
    [SerializeField] private float gravity = -9.81f;    // 重力加速度

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;     // 地面检测点
    [SerializeField] private float groundDistance = 0.1f; // 地面检测距离

    [SerializeField] private LayerMask groundMask;      // 地面层

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // 重置下落速度当接触地面时
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 获取输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 跑步切换
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // 计算移动方向 (相对于玩家朝向)
        Vector3 move = transform.right * x + transform.forward * z;

        // 应用移动
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 应用重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}