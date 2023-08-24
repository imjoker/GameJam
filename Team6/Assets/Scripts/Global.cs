using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public enum ePlayerState : int
    {
        WALK = 0,
        RUN,
        JUMP,
        CLIMB,
    }

    static public ePlayerState currState = ePlayerState.WALK;
    static public int defGravityScale = 3;

    public static bool isJumping ()
    {
        return (currState == ePlayerState.JUMP);
    }

    public static bool isClimbing()
    {
        return (currState == ePlayerState.CLIMB);
    }

    public static bool isRunning ()
    {
        return (currState == ePlayerState.RUN);
    }
}
