using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public List<Shell> shells;
    private int currentShellIndex = 0;

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
        if (currentShellIndex >= shells.Count)
            return null;

        Shell shell = shells[currentShellIndex];
        currentShellIndex++;
        return shell;
    }

    public int RemainingShells()
    {
        return shells.Count - currentShellIndex;
    }
}