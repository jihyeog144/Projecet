using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public AudioSource heartbeatAudio;
    public Image vignette;
    public Text shellText;

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
