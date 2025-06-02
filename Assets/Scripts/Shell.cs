using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShellType { Blank, Live } // 공포탄 실탄 
public class Shell // 여기서 쉘 타입 변수 지정
{
    public ShellType Type;
    public Shell(ShellType type)
    {
        Type = type;
    }
}
