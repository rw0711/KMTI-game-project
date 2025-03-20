using UnityEngine;

public class PlayerLoopMovement : MonoBehaviour
{
    public float leftBoundary = -10f;
    public float rightBoundary = 10f;
    public BackgroundTransition backgroundTransition; // 배경 전환 스크립트 참조

    void Update()
    {
        if (transform.position.x > rightBoundary)
        {
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
            backgroundTransition.ChangeBackground(true); // 👉 오른쪽 이동 (다음 배경)
        }
        else if (transform.position.x < leftBoundary)
        {
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
            backgroundTransition.ChangeBackground(false); // 👈 왼쪽 이동 (이전 배경)
        }
    }
}
