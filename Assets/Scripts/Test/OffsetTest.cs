using UnityEngine;
using UnityEngine.UI;

public class OffsetTest : MonoBehaviour
{
    public AnimationCurve offsetCurve;
    public AnimationCurve rotateCurve;
    public float offsetSize;
    public float offsetRotation;
    private Vector3[] _originPos;
    private Vector3[] _originRate;
    private Vector3[] _afterPos;
    private int _childCount;
    private float _clipTime;
    private float _realTime;

    void Start()
    {
        _childCount = transform.childCount;
        _originPos = new Vector3[transform.childCount];
        _afterPos = new Vector3[transform.childCount];
        _originRate = new Vector3[transform.childCount];
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        for (int i = 0; i < _childCount; i++)
        {
            _originPos[i] = (transform.GetChild(i) as RectTransform).localPosition;
            _originRate[i] = (transform.GetChild(i) as RectTransform).localEulerAngles;
        }
        Debug.Log($"{_childCount}");
        
    }

    // Update is called once per frame
    void Update()
    {
        _clipTime = 1.00f / (_childCount-1);
        for (int i = 0; i < _childCount; i++)
        {
            float height,rotateZ;
            if (i == 0)
            {
                _realTime = 0;
                height = offsetCurve.Evaluate(_realTime);
                rotateZ = rotateCurve.Evaluate(_realTime);
            }
            else
            {
                height = offsetCurve.Evaluate(_realTime += _clipTime);
                rotateZ = rotateCurve.Evaluate(_realTime);
            }

            Debug.Log($"realTime:{_realTime} clipTime:{_clipTime} 第 {i} 个实际偏移的高 " + height);
            Transform opertionObj = transform.GetChild(i);
            Vector3 offsetPos = new Vector3(0, height * 100 * offsetSize, 0);
            Vector3 offsetRotate = new Vector3(0, 0, rotateZ * offsetRotation);
            opertionObj.localPosition = _originPos[i] + offsetPos;
            opertionObj.localRotation = Quaternion.Euler(_originRate[i] + offsetRotate);
            _afterPos[i] = opertionObj.transform.localPosition;

        }
    }
}