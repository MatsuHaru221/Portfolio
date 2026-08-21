using UnityEngine;
using NaughtyAttributes;

public class HandPosCalc : MonoBehaviour
{
    [Foldout("手札の配置用")]
    [SerializeField, Label("手札を表示する円の大きさ")]
    private float _handCardRadius = 1;
    [Foldout("手札の配置用")]
    [SerializeField, Label("手札を表示する角度の最小値")]
    private float _handCardStartAngle = 60;
    [Foldout("手札の配置用")]
    [SerializeField, Label("手札を表示する角度の最大値")]
    private float _handCardEndAngle = 120;
    [Foldout("手札の配置用")]
    [SerializeField, Label("仮用の手札の枚数")]
    private int _handNum = 5;

    public Vector3 GetPositionByIndex(int index)
    {
        if (_handNum <= 0)
        {
            Debug.LogError("オブジェクト数が0以下です");
            return transform.position;
        }

        // indexを範囲内に収めるためclamp
        index = Mathf.Clamp(index, 0, _handNum- 1);
        float totalAngle = _handCardEndAngle - _handCardStartAngle;

        float angle;
        // 1枚だけなら中央角度
        if (_handNum == 1)
        {
            angle = (_handCardStartAngle + _handCardEndAngle) / 2f;
        }
        // 均等割りの角度
        else
        {
            float step = totalAngle / (_handNum - 1);
            angle = _handCardStartAngle + step * index;
        }

        // 座標計算
        float rad = Mathf.Deg2Rad * angle;
        float x = Mathf.Cos(rad) * _handCardRadius;
        float y = Mathf.Sin(rad) * _handCardRadius;

        // 自身の位置を中心にした座標を返す
        return transform.position + new Vector3(x, y, 0f);
    }
    
    
}
