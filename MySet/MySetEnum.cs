using System.Collections;

namespace MySet
{
    public class MySetEnum : IEnumerator
    {
        private int[] _lefts;
        private int[] _rights;
        private int _curIntervalPos;
        private int _curVal;
        public MySetEnum(int[] lefts, int[] rights)
        {
            _lefts = lefts;
            _rights = rights;
            _curIntervalPos = 0;
            _curVal = _lefts[0];
        }

        public bool MoveNext()
        {
            if (_curVal == _rights[_curIntervalPos])
            {
                _curIntervalPos++;
                if (_curIntervalPos == _rights.Length)
                    return false;
                _curVal = _lefts[_curIntervalPos];
            }
            else
                _curVal++;
            return true;
        }

        public void Reset()
        {
            _curIntervalPos = 0;
            _curVal = _lefts[0];
        }

        object IEnumerator.Current => Current;

        public int Current => _curVal;
    }
}