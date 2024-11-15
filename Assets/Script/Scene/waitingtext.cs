using UnityEngine;
using TMPro;
using Unity.Netcode;

public class RoleTextManager : MonoBehaviour
{
    public TMP_Text roleText; // Tham chiếu đến TMP_Text UI trong Inspector

    private void Start()
    {
        // Kiểm tra xem người chơi có phải là host không
        if (NetworkManager.Singleton.IsHost)
        {
            // Hiển thị nút nếu người chơi là host
            roleText.gameObject.SetActive(true);
        }
        else
        {
            // Ẩn nút nếu người chơi là client
            roleText.gameObject.SetActive(false);
        }
    }
}
