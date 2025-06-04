using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;


public class Gun : MonoBehaviour
{
    public List<Shell> shells = new();
    private int currentIndex = 0;
    public PlayerController CurrentTarget { get; private set; } //  πÊ±› Ω ¥ÎªÛ!

    public bool IsAmmoEmpty => currentIndex >= shells.Count;

    public Transform gunTransform; // √— ¿¸√º ø¿∫Í¡ß∆Æ
    public ParticleSystem muzzleFlash;
    public Color blankColor = Color.white;
    public Color liveColor = Color.red;
    public List<Shell> GetAllShells()
    {
        return shells;
    }

    public void LoadShells(int blankCount, int buckshotCount)
    {
        shells = new List<Shell>();

        for (int i = 0; i < blankCount; i++) shells.Add(new Shell(ShellType.Blank));
        for (int i = 0; i < buckshotCount; i++) shells.Add(new Shell(ShellType.Live));

        shells = shells.OrderBy(x => Random.value).ToList(); // º≈«√
        currentIndex = 0;
    }

    public Shell Fire(PlayerController target = null)
    {
        if (currentIndex >= shells.Count)
            return null;

        Shell shell = shells[currentIndex];
        currentIndex++;

        CurrentTarget = target;
        return shell;
    }

    public void AimAt(Transform target)
    {
        Vector3 dir = target.position - gunTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public int RemainingShellCount() => shells.Count - currentIndex;

}