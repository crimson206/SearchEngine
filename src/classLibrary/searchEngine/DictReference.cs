namespace SearchEngine
{
    using System.Collections.Generic;

    public class DictReference<T> where T:notnull
    {
        Dictionary<T,int> _conditionInfo;
        int _uncoveredConditionCount;
        int _totalConditionCount;

        public DictReference(Dictionary<T, int> conditionInfo)
        {
            _conditionInfo = conditionInfo;
            _uncoveredConditionCount = conditionInfo.Count;
        }

        public int UncoveredConditionCount { get => _uncoveredConditionCount;}
        public int TotalConditionCount { get => _totalConditionCount;}

        public Dictionary<T,int> GetConditionInfoCopy
        {
            get
            {
                return new Dictionary<T,int>(_conditionInfo!);
            }
        }

        public bool TryDeApply(T target)
        {
            if(_conditionInfo!.ContainsKey(target))
            {
                if(_conditionInfo[target]-- == 0)
                    _uncoveredConditionCount--;
                return true;
            }
            else
                return false;
        }

        public bool TryDeApply(T target, out bool metPartialCondition)
        {
            metPartialCondition = false;
            int temp = _uncoveredConditionCount;
            bool succeed = TryDeApply(target);
            if(temp != _uncoveredConditionCount)
                metPartialCondition = true;
            
            return succeed;
        }


        public bool TryApply(T target)
        {
            if(_conditionInfo!.ContainsKey(target))
            {
                if(_conditionInfo[target]++ == 1)
                    _uncoveredConditionCount++;
                return true;
            }
            else
                return false;
        }

        public bool TryApply(T target, out bool brokePartialCondition)
        {
            brokePartialCondition = false;
            int temp = _uncoveredConditionCount;
            bool succeed = TryApply(target);
            if(temp != _uncoveredConditionCount)
                brokePartialCondition = true;
            return succeed;
        }
    }
}
