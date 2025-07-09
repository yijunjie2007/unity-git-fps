using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("视角设置")]
    [SerializeField] private float mouseSensitivity = 100f; // 鼠标灵敏度
    [SerializeField] private Transform playerBody;         // 玩家身体(用于水平旋转)

    [Header("视角限制")]
    [SerializeField] private float minVerticalAngle = -90f; // 最小俯角
    [SerializeField] private float maxVerticalAngle = 90f;  // 最大仰角

    private float xRotation = 0f; // 当前垂直旋转角度

    private void Start()
    {
        // 锁定并隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 垂直视角旋转 (上下看)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        // 应用垂直旋转 (只旋转相机)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 水平视角旋转 (左右看 - 旋转整个玩家对象)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}