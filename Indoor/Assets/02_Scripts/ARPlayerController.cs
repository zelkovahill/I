using UnityEngine;

public class ARPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        // X축 방향 입력
        float horizontal = Input.GetAxis("Horizontal");

        // 이동 벡터 (AR 기준으로 X축만 이동)
        Vector3 moveDirection = new Vector3(horizontal, 0, 0);

        // AR 카메라의 로컬 좌표를 기준으로 움직이게 설정
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}