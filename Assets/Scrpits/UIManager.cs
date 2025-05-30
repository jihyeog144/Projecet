using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab; // 탄환 아이콘 프리팹
    public Transform shellPanelParent; // 탄환 아이콘들을 담는 부모 오브젝트
    public Sprite blankSprite;
    public Sprite buckshotSprite;
    public Button fireButton;


    private List<GameObject> shellSlots = new();

    public void CreateShellUI(List<Shell> shells)
    {
        if (shells == null)
        {
            Debug.LogError("shells 리스트가 null입니다!");
            return;
        }

        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            if (shell == null)
            {
                Debug.LogWarning("shell 항목이 null입니다. 무시합니다.");
                continue;
            }

            GameObject slot = Instantiate(shellSlotPrefab, shellPanelParent);
            Image img = slot.GetComponent<Image>();

            img.color = shell.Type == ShellType.Blank
                ? new Color(1f, 1f, 1f, 0.2f)
                : Color.red;

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


    public void EnableFireButton(bool enable)
    {
        fireButton.interactable = enable;
    }
}