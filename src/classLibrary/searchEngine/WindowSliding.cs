using System;

namespace SearchEngine
{
    public class WindowSliding<T> where T:notnull
    {
        private T[] _item;
        private int _leftIndex = 0;
        private int _rightIndex = 0;
        private DictReference<T> _reference;

        public WindowSliding(T[] collection, DictReference<T> reference)
        {
            _item = collection;
            _reference = reference;
        }

        public T[] GetItemCopy
        { 
            get
            {
                T[] copy = new T[_item.Length];
                Array.Copy(_item, copy, _item.Length);

                return copy;
            }
        }

        public void ChangeReference(DictReference<T> reference)
        {
            ResetIndeces();
            _reference = reference;
        }

        public object GetConditionCounts
        {
            get
            {
                return _reference!.GetConditionInfoCopy;
            }
        }

        public void PushLongRightIndex(int countConditionsToMeet, out bool reachedEnd)
        {
            reachedEnd = false;
            if(countConditionsToMeet == -1)
                countConditionsToMeet = _reference!.UncoveredConditionCount;

            try
            {
                if(countConditionsToMeet > _reference!.UncoveredConditionCount)
                    throw new Exception("countConditionsToMeet exceeds uncoveredCount");
                if(_reference!.UncoveredConditionCount == 0)
                    throw new Exception("there is no more condition to meet");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            int temp = _reference.UncoveredConditionCount;

            for(int i = _rightIndex; i < _item.Length; i++)
            {
                bool metPartialCondition;
                PushRightIndex(out metPartialCondition);
                if(metPartialCondition)
                    if(countConditionsToMeet == temp - _reference.UncoveredConditionCount)
                        break;
            }

            reachedEnd = true;
        }

        public void PushLongLeftIndex(int countConditionsToBreak, out bool reachedEnd)
        {
            reachedEnd = false;

            if(countConditionsToBreak == -1)
                countConditionsToBreak = _reference.UncoveredConditionCount;

            try
            {
                int coveredConditionCount = _reference.TotalConditionCount - _reference.UncoveredConditionCount;
                if(countConditionsToBreak > coveredConditionCount)
                    throw new Exception("the number of breakable conditions is less than countConditionsToBreak");
                if(coveredConditionCount == 0)
                    throw new Exception("There is no condition to break");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            int temp = _reference.UncoveredConditionCount;

            for(int i = _leftIndex; i < _item.Length; i++)
            {
                bool metPartialCondition = false;
                PushLeftIndex(out metPartialCondition);
                if(metPartialCondition)
                    if(countConditionsToBreak == _reference.UncoveredConditionCount - temp)
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
            _reference.TryApply(_item[_rightIndex]);
        }

        public void PushRightIndex(out bool metPartialCondition)
        {
            _rightIndex++;
            _reference.TryApply(_item[_rightIndex], out metPartialCondition);
        }

        public void PushLeftIndex()
        {
            _leftIndex++;
            _reference.TryDeApply(_item[_leftIndex]);
        }

        public void PushLeftIndex(out bool brokePartialCondition)
        {
            brokePartialCondition = false;
            _leftIndex++;
            _reference.TryDeApply(_item[_leftIndex], out brokePartialCondition);
        }

        private void ResetIndeces()
        {
            _rightIndex = 0;
            _leftIndex = 0;
        }
    }
}
