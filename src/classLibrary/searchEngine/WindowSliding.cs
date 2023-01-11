using System;

namespace SearchEngine
{
    public class WindowSliding<T> where T:notnull
    {
        private T[] _item;
        private int _leftIndex = 0;
        private int _rightIndex = 0;
        private DictReference<T>? _reference;
        private Exception? _catchedErr;

        public WindowSliding(T[] collection)
        {
            _item = collection;
        }

        public WindowSliding(T[] collection, DictReference<T> reference)
        {
            _item = collection;
            _reference = reference;
        }

        public DictReference<T> Reference
        {
            get
            {
                if(_reference == null)
                {
                    
                }

                return _reference;
            }
            
        }

        public T[] Item { get => _item; set => _item = value;}
        public Exception? CatchedErr { get => _catchedErr;}

        public Dictionary<T, int> GetConditionCounts
        {
            get
            {
                return new Dictionary<T, int>(_reference!.ConditionCounts);
            }
        }

        public void PushLongRightIndex(int countConditionsToMeet, out bool reachedEnd)
        {
            reachedEnd = false;
            if(countConditionsToMeet == -1)
                countConditionsToMeet = _reference!.UncoveredCount;

            if(countConditionsToMeet > _reference!.UncoveredCount)
                try
                {
                    throw new Exception("countConditionsToMeet exceeds uncoveredCount");
                }
                catch(Exception ex)
                {
                    _catchedErr = ex;
                }

            int temp = _reference.UncoveredCount;

            for(int i = _rightIndex; i < _item.Length; i++)
            {
                bool metPartialCondition;
                PushRightIndex(out metPartialCondition);
                if(metPartialCondition)
                    if(countConditionsToMeet == temp - _reference.UncoveredCount)
                        break;
            }

            reachedEnd = true;
        }

        public void PushLongLeftIndex(int countConditionsToBreak, out bool reachedEnd)
        {
            reachedEnd = false;

            if(countConditionsToBreak == -1)
                countConditionsToBreak = _reference.UncoveredCount;

            if(countConditionsToBreak > _reference.ConditionCounts.Count - _reference.UncoveredCount)
                try
                {
                    throw new Exception("the number of breakable conditions is less than countConditionsToBreak");
                }
                catch(Exception ex)
                {
                    _catchedErr = ex;
                }

            int temp = _reference.UncoveredCount;

            for(int i = _leftIndex; i < _item.Length; i++)
            {
                bool metPartialCondition = false;
                PushLeftIndex(out metPartialCondition);
                if(metPartialCondition)
                    if(countConditionsToBreak == _reference.UncoveredCount - temp)
                        break;
            }

            reachedEnd = true;
        }

        public bool TryPushRightIndex(out bool metPartialCondition)
        {
            metPartialCondition = false;
            if(_rightIndex == _item.Length)
                return false;

            PushRightIndex(out metPartialCondition);
            return true;
        }

        public void PushRightIndex()
        {
            _rightIndex++;
            _reference.TryDecreaseCount(_item[_rightIndex]);
        }

        public void PushRightIndex(out bool metPartialCondition)
        {
            _rightIndex++;
            _reference.TryDecreaseCount(_item[_rightIndex], out metPartialCondition);
        }

        public void PushLeftIndex()
        {
            _leftIndex++;
            _reference.TryIncreaseCount(_item[_leftIndex]);
        }

        public void PushLeftIndex(out bool brokePartialCondition)
        {
            brokePartialCondition = false;
            _leftIndex++;
            _reference.TryIncreaseCount(_item[_leftIndex], out brokePartialCondition);
        }
    }
}
