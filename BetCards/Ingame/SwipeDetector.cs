using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private TouchState _touchedPos;

    private void OnTouched(InputAction.CallbackContext context)
    {
        _touchedPos = context.ReadValue<TouchState>();
    }

    private void TouchedPosDetect()
    {
        // タッチした位置（スクリーン座標）
        var touchedPos = _touchedPos.position;

        // 移動距離（スクリーン座標）
        var deltaPos = _touchedPos.delta;

        // 移動前の座標（スクリーン座標）
        var preTouchedPos = touchedPos - deltaPos;
    }
}
