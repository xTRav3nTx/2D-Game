using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringConstants : MonoBehaviour
{
    private static StringConstants instance;
    public static StringConstants Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindAnyObjectByType<StringConstants>();
            }
            return instance;
        }
    }

    public const string ATTACK = "Attack";
    public const string IDLE = "Idle";
    public const string DIE = "Die";
    public const string RUN = "Run";
    public const string JUMP = "Jump";

    public const string PLAYER = "Player";
    public const string ENEMY = "Enemy";

    public const string GROUND = "Ground";
    public const string WALL = "Wall";
    public const string WATER = "Water";

}
