using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumberCalculator {
    /// <summary>小数部分を返す</summary>
    public static float decimalPart(this float n) {
        return n - Mathf.Floor(n);
    }
}
