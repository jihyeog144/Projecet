using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab; // źȯ ������ ������
    public Transform shellPanelParent; // źȯ �����ܵ��� ��� �θ� ������Ʈ
    public Sprite blankSprite;
    public Sprite buckshotSprite;
    public Text shellCountText;
    public Button fireButton;


    private List<GameObject> shellSlots = new();

    public void CreateShellUI(List<Shell> shells)
    {
        // ���� UI ����
        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            GameObject slot = Instantiate(shellSlotPrefab, shellPanelParent);
            Image img = slot.GetComponent<Image>();
            img.sprite = shell.Type == ShellType.Blank ? blankSprite : buckshotSprite;
            img.color = new Color(1f, 1f, 1f, 0.2f); // ó���� �帮��
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
        shellCountText.text = $"���� źȯ: {remaining}";
    }

    public void EnableFireButton(bool enable)
    {
        fireButton.interactable = enable;
    }
}