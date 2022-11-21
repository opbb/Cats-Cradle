using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUtils {
    // Returns true if the given layer is in the layermask
    public static bool isLayerInMask(int layer, LayerMask layerMask) {
        return layerMask == (layerMask | (1 << layer));
    }
}