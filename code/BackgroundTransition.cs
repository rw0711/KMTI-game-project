using UnityEngine;
using UnityEngine.UI;

public class BackgroundTransition : MonoBehaviour
{
    public GameObject[] backgrounds; // 배경 리스트
    private int currentBackgroundIndex = 0; // 현재 배경 인덱스

    void Start()
    {
        // 처음에는 모든 배경을 비활성화하고 첫 번째 배경만 활성화
        foreach (GameObject bg in backgrounds)
        {
            bg.SetActive(false);
        }

        if (backgrounds.Length > 0)
        {
            backgrounds[currentBackgroundIndex].SetActive(true);
        }
    }

    public void ChangeBackground(bool moveRight)
    {
        Debug.Log("배경 전환 실행됨!");

        if (backgrounds == null || backgrounds.Length == 0)
        {
            Debug.LogError("❌ 오류: backgrounds 배열이 비어 있습니다! Inspector에서 설정하세요.");
            return;
        }

        // 현재 배경 비활성화
        backgrounds[currentBackgroundIndex].SetActive(false);

        // 인덱스 변경
        if (moveRight)
        {
            currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Length;
        }
        else
        {
            currentBackgroundIndex = (currentBackgroundIndex - 1 + backgrounds.Length) % backgrounds.Length;
        }

        // 새 배경 활성화
        backgrounds[currentBackgroundIndex].SetActive(true);
    }
}
