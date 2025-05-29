using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab;     // źȯ ������ ������
    public Transform shellPanelParent;     // źȯ ����Ʈ �θ� ������Ʈ

    public Sprite blankSprite;
    public Sprite buckshotSprite;

    private List<GameObject> shellSlots = new();

    // źȯ ����Ʈ ����
    public void CreateShellUI(List<Shell> shells)
    {
        // ���� ���� ����
        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            GameObject slot = Instantiate(shellSlotPrefab, shellPanelParent);
            Image img = slot.GetComponent<Image>();
            img.sprite = shell.Type == ShellType.Blank ? blankSprite : buckshotSprite;
            img.color = new Color(1, 1, 1, 0.3f); // ��Ȱ�� ����ó�� ó��
            shellSlots.Add(slot);
        }
    }

    // �߻� �� ���� ź ǥ�� ����
    public void HighlightFiredShell(int index)
    {
        if (index < shellSlots.Count)
        {
            var img = shellSlots[index].GetComponent<Image>();
            img.color = Color.white; // ���� ź ǥ��
        }
    }

    public void UpdateShellUI()
    {

    }
}