using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public AudioSource heartbeatAudio;
    public Image vignette;
    public Text shellText;

    public GameObject shellPrefab;
    public Transform shells;

    public Sprite blankSprite;
    public Sprite LiveSprite;

    private List<GameObject> shellSlots = new();

    public void CreatShellUI(List<Shell> shells)
    {
        foreach (var slot in shellSlots)
            Destroy(slot);

    }

    public void ShowTensionEffect()
    {
        heartbeatAudio.Play();
        vignette.enabled = true;
        vignette.color = new Color(0.5f, 0, 0, 0.5f); // ∫”¿∫ »≠∏È
    }

    public void HideTensionEffect()
    {
        heartbeatAudio.Stop();
        vignette.enabled = false;
    }


    public void UpdateShellUI(int remaining)
    {
        shellText.text = $"≥≤¿∫ ≈∫»Ø: {remaining}";
    }   
}
