using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundTransition : MonoBehaviour
{
    public Image fadeScreen; // 검은색 페이드 효과 이미지
    public GameObject[] backgrounds; // 배경 리스트
    private int currentBackgroundIndex = 0; // 현재 배경 인덱스
    public float fadeDuration = 0.5f; // 페이드 효과 지속 시간

    void Start()
    {
        if (fadeScreen == null)
        {
            fadeScreen = GameObject.Find("fadeScreen").GetComponent<Image>();
        }

        // 처음에는 모든 배경을 비활성화하고 첫 번째 배경만 활성화
        foreach (GameObject bg in backgrounds)
        {
            bg.SetActive(false);
        }
        backgrounds[currentBackgroundIndex].SetActive(true);
    }

    public void ChangeBackground(bool moveRight)
    {
        Debug.Log("배경 전환 실행됨!");
        StartCoroutine(FadeTransition(moveRight));
    }

    IEnumerator FadeTransition(bool moveRight)
{
    if (backgrounds == null || backgrounds.Length == 0)
    {
        Debug.LogError("❌ 오류: backgrounds 배열이 비어 있습니다! Inspector에서 설정하세요.");
        yield break;
    }

    // 🔹 1. 빠른 페이드 아웃 (화면을 검게)
    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    {
        fadeScreen.color = new Color(0, 0, 0, t / fadeDuration);
        yield return null;
    }
    fadeScreen.color = new Color(0, 0, 0, 1); // 완전히 검게 변경

    // 🔹 2. 배경 즉시 변경 (바로 활성화)
    backgrounds[currentBackgroundIndex].SetActive(false);

    if (moveRight)
    {
        // 👉 오른쪽 이동 → 다음 배경
        currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Length;
    }
    else
    {
        // 👈 왼쪽 이동 → 이전 배경
        currentBackgroundIndex = (currentBackgroundIndex - 1 + backgrounds.Length) % backgrounds.Length;
    }

    backgrounds[currentBackgroundIndex].SetActive(true);

    // 🔹 3. 빠른 페이드 인 (화면을 서서히 밝게)
    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    {
        fadeScreen.color = new Color(0, 0, 0, 1 - (t / fadeDuration));
        yield return null;
    }
    fadeScreen.color = new Color(0, 0, 0, 0); // 완전히 밝아짐
}
}
