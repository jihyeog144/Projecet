using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class Gun : MonoBehaviour
{
    public List<Shell> shells = new();
    private int currentShellIndex = 0;

    public CinemachineImpulseSource impulse;

    public void LoadShells(int blankCount, int buckshotCount)
    {
        shells = new List<Shell>();

        for (int i = 0; i < blankCount; i++) shells.Add(new Shell(ShellType.Blank));
        for (int i = 0; i < buckshotCount; i++) shells.Add(new Shell(ShellType.Live));

        shells = shells.OrderBy(x => Random.value).ToList(); // ¼ÅÇÃ
        currentShellIndex = 0;
    }

    public Shell Fire()
    {
        if (currentShellIndex >= shells.Count) return null;

        Shell shell = shells[currentShellIndex++];

        if (shell.Type == ShellType.Live && impulse != null)
        {
            impulse.GenerateImpulse(); // Èçµé¸² ¹ß»ý!
        }

        return shell;
    }

    public int RemainingShellCount()
    {
        return shells.Count - currentShellIndex;
    }
}