using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shellSlotPrefab; // 탄환 아이콘 프리팹
    public Transform shellPanelParent; // 탄환 아이콘들을 담는 부모 오브젝트
    public Button fireButton;
    public Sprite blankSprite;
    public Sprite buckshotSprite;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI aiHPText;
    public GameObject targetChoicePanel;


    private List<GameObject> shellSlots = new();

    private Coroutine playerBlinkRoutine;
    private Coroutine aiBlinkRoutine;

    public TextMeshProUGUI roundInfoText;


    public void CreateShellUI(List<Shell> shells)
    {

        foreach (var slot in shellSlots)
            Destroy(slot);
        shellSlots.Clear();

        foreach (var shell in shells)
        {
            GameObject slot = Instantiate(shellSlotPrefab, shellPanelParent);
            Image img = slot.GetComponent<Image>();

            img.color = shell.Type == ShellType.Blank
                ? new Color(1f, 1f, 1f, 0.2f) // 공포탄: 희미한 회색
                : Color.red;                  // 실탄: 붉은 피색

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

    public void UpdateHP(PlayerController player, int current, int max)
    {
        TextMeshProUGUI targetText  = player.isAI ? aiHPText : playerHPText;

        Debug.Log($"🩸 UpdateHP 호출됨: {player.name} → {current} / {max}");

        // 죽음 처리
        if (current <= 0)
        {
            targetText.text = "💀 DEAD";
            targetText.color = Color.gray;
            return;
        }

        // 일반 HP 표시
        targetText.text = $"HP: {current} / {max}";

        // HP 감소 → 붉게
        targetText.color = current < max ? Color.red : Color.white;

        // 깜빡이기 조건: HP 1 남음
        if (current == 1)
        {
            Coroutine blink = StartCoroutine(BlinkText(targetText));

            if (player.isAI)
            {
                if (aiBlinkRoutine != null) StopCoroutine(aiBlinkRoutine);
                aiBlinkRoutine = blink;
            }
            else
            {
                if (playerBlinkRoutine != null) StopCoroutine(playerBlinkRoutine);
                playerBlinkRoutine = blink;
            }
        }
        else
        {
            // 깜빡이기 해제
            if (player.isAI && aiBlinkRoutine != null)
            {
                StopCoroutine(aiBlinkRoutine);
                aiBlinkRoutine = null;
                targetText.color = Color.red;
            }
            else if (!player.isAI && playerBlinkRoutine != null)
            {
                StopCoroutine(playerBlinkRoutine);
                playerBlinkRoutine = null;
                targetText.color = Color.red;
            }
        }
    }

    IEnumerator BlinkText(TextMeshProUGUI text)
    {
        while (true)
        {
            text.alpha = 0.3f;
            yield return new WaitForSeconds(0.3f);
            text.alpha = 1f;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ShowTargetChoice(bool show)
    {
        if (targetChoicePanel != null)
            targetChoicePanel.SetActive(show);
    }

    public void ShowRoundInfo(int blanks, int lives, float duration = 2f)
    {
        roundInfoText.text = $"🩸 이번 게임\n공포탄 {blanks}발 / 실탄 {lives}발";
        roundInfoText.gameObject.SetActive(true);
        StartCoroutine(HideRoundInfoAfterDelay(duration));
    }

    IEnumerator HideRoundInfoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        roundInfoText.gameObject.SetActive(false);
    }
}