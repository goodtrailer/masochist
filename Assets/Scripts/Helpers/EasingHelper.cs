// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Mathematics;
using UnityEngine;

public enum Easing
{
    In,
    Out,
    InOut,

    InCub,
    OutCub,
    InOutCub,

    InQuart,
    OutQuart,
    InOutQuart,

    InQuint,
    OutQuint,
    InOutQuint,
}

public static class EasingHelper
{
    public static double EaseValue(Easing easing, int pow, double x,
        double xMin = 0.0, double xMax = 1.0,
        double yMin = 0.0, double yMax = 1.0)
    {
        x = (x - xMin) / (xMax - xMin);
        double ret = 0;
        switch (easing)
        {
            case Easing.In:
            case Easing.InCub:
            case Easing.InQuart:
            case Easing.InQuint:
                ret = math.pow(x, pow);
                break;
            case Easing.Out:
            case Easing.OutCub:
            case Easing.OutQuart:
            case Easing.OutQuint:
                ret = 1 - math.pow(1 - x, pow);
                break;
            case Easing.InOut:
            case Easing.InOutCub:
            case Easing.InOutQuart:
            case Easing.InOutQuint:
                ret = 2 * (pow - 1) * (x < 0.5 ? math.pow(x, pow) : 1 - math.pow(1 - x, pow));
                break;
        }
        return ret * (yMax - yMin) + yMin;
    }

    public static double EaseValue(Easing easing, double x,
        double xMin = 0.0, double xMax = 1.0,
        double yMin = 0.0, double yMax = 1.0)
    {
        int pow = 2;
        switch (easing)
        {
            case Easing.InCub:
            case Easing.OutCub:
            case Easing.InOutCub:
                pow = 3;
                break;
            case Easing.InQuart:
            case Easing.OutQuart:
            case Easing.InOutQuart:
                pow = 4;
                break;
            case Easing.InQuint:
            case Easing.OutQuint:
            case Easing.InOutQuint:
                pow = 5;
                break;
        }
        return EaseValue(easing, pow, x, xMin, xMax, yMin, yMax);
    }
}
