using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab; // źȯ ������ ������
    public Transform shellPanelParent; // źȯ �����ܵ��� ��� �θ� ������Ʈ
    public Sprite blankSprite;
    public Sprite buckshotSprite;
    public Button fireButton;


    private List<GameObject> shellSlots = new();

    public void CreateShellUI(List<Shell> shells)
    {
        if (shells == null)
        {
            Debug.LogError("shells ����Ʈ�� null�Դϴ�!");
            return;
        }

        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            if (shell == null)
            {
                Debug.LogWarning("shell �׸��� null�Դϴ�. �����մϴ�.");
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