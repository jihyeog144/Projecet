using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShellType { Blank, Live } // ����ź ��ź 
public class Shell // ���⼭ �� Ÿ�� ���� ����
{
    public ShellType Type;
    public Shell(ShellType type)
    {
        Type = type;
    }
}
