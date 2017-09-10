﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants {

    public static float UNIT_LENGTH = 1;
    public static int DOTS_WIDE = 30;
    public static int DOTS_HIGH = 15;
    public static float DOT_DELTA_X = UNIT_LENGTH;
    public static float DOT_DELTA_Y = UNIT_LENGTH * Mathf.Sqrt(3) / 2;
}