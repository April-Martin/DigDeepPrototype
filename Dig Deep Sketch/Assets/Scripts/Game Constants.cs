using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants {

    public static float UNIT_LENGTH = .5f;
    public static int DOTS_WIDE = 40;
    public static int DOTS_HIGH = 15;
    public static float DOT_DELTA_X = UNIT_LENGTH;
    public static float DOT_DELTA_Y = UNIT_LENGTH * Mathf.Sqrt(3) / 2;
    public static int BASE_MOVES_PER_ROUND = 3;
    public static int ROUNDS_PER_GAME = 50;

    public enum Resources { WATER, GOOD, BAD };
}
