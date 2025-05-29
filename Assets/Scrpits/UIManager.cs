using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab;     // 탄환 아이콘 프리팹
    public Transform shellPanelParent;     // 탄환 리스트 부모 오브젝트

    public Sprite blankSprite;
    public Sprite buckshotSprite;

    private List<GameObject> shellSlots = new();

    // 탄환 리스트 구성
    public void CreateShellUI(List<Shell> shells)
    {
        // 기존 슬롯 제거
        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            GameObject slot = Instantiate(shellSlotPrefab, shellPanelParent);
            Image img = slot.GetComponent<Image>();
            img.sprite = shell.Type == ShellType.Blank ? blankSprite : buckshotSprite;
            img.color = new Color(1, 1, 1, 0.3f); // 비활성 상태처럼 처리
            shellSlots.Add(slot);
        }
    }

    // 발사 후 남은 탄 표시 강조
    public void HighlightFiredShell(int index)
    {
        if (index < shellSlots.Count)
        {
            var img = shellSlots[index].GetComponent<Image>();
            img.color = Color.white; // 사용된 탄 표시
        }
    }

    public void UpdateShellUI()
    {

    }
}