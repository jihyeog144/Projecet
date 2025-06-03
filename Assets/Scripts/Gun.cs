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

    public Transform gunTransform;

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

    public List<Shell> GetAllShells()
    {
        return shells;
    }
    public int GetCurrentIndex()
    {
        return currentShellIndex;
    }

    public void AimAt(Transform target)
    {
        Vector3 direction = target.position - gunTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(0, 0, angle);
    }


}