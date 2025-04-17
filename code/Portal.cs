using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위한 네임스페이스

public class Portal : MonoBehaviour
{
    public string nextSceneName; // 이동할 씬 이름
    private bool isNearPortal = false; // 포탈 근처에 있는지 여부

    void Update()
    {
        // F 키를 눌렀을 때 씬 전환
        if (isNearPortal && Input.GetKeyDown(KeyCode.F)) // F 키 입력 시
        {
            LoadNewScene(nextSceneName); // 지정된 씬으로 이동
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 포탈에 닿았을 때
        {
            isNearPortal = true; // 포탈 근처에 있다고 설정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어가 포탈에서 나갔을 때
        {
            isNearPortal = false; // 포탈 근처에서 벗어남
        }
    }

    // 씬 전환 함수
    void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 이름으로 전환
    }
}
