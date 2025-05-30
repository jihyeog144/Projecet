using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab; // 탄환 아이콘 프리팹
    public Transform shellPanelParent; // 탄환 아이콘들을 담는 부모 오브젝트
    public Sprite blankSprite;
    public Sprite buckshotSprite;
    public Text shellCountText;
    public Button fireButton;


    private List<GameObject> shellSlots = new();

    public void CreateShellUI(List<Shell> shells)
    {
        // 기존 UI 제거
        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            GameObject slot = Instantiate(shellSlotPrefab, shellPanelParent);
            Image img = slot.GetComponent<Image>();
            img.sprite = shell.Type == ShellType.Blank ? blankSprite : buckshotSprite;
            img.color = new Color(1f, 1f, 1f, 0.2f); // 처음엔 흐리게
            shellSlots.Add(slot);
        }
    }

    public void HighlightFiredShell(int index)
    {
        if (index < shellSlots.Count)
        {
            var img = shellSlots[index].GetComponent<Image>();
            img.color = Color.white;
        }
    }

    public void UpdateShellUI(int remaining)
    {
        shellCountText.text = $"남은 탄환: {remaining}";
    }

    public void EnableFireButton(bool enable)
    {
        fireButton.interactable = enable;
    }
}