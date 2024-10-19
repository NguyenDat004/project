using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashing : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject slashlinePrefab; // Prefab VẾT CHÉM

    // Các tham số có thể được thiết lập trong hàm Start
    private GameObject firePoint; // Vị trí nơi viên đạn được bắn ra

    private float TimeShoot;
    private float TimeCount;
    private void Start()
    {
        // Khởi tạo giá trị cho các biến
        // Lấy vị trí bắn từ con cái đầu tiên
        firePoint = transform.GetChild(0).gameObject;
        TimeCount = 0;
        TimeShoot = 0.5f;
    }

    private void Update()
    {
        // Kiểm tra nếu nhấn phím bắn
        if (Input.GetKeyDown(KeyCode.Space) && TimeCount>=TimeShoot)
        {
            Slash();
            TimeCount=0;
        }
        TimeCount+=Time.deltaTime;

    }

    private void Slash()
    {
        if (slashlinePrefab != null)
        {
            // Tạo viên đạn từ prefab tại vị trí firePoint
            Instantiate(slashlinePrefab, firePoint.transform.position, firePoint.transform.rotation);
        }
        else
        {
            Debug.Log("Ko có prefab Slashline");
        }
    }
}
