namespace SearchEngine
{
    using System.Collections.Generic;

    public class DictReference<T> where T:notnull
    {
        Dictionary<T,int>? _conditionCounts;
        int _uncoveredCount;

        public DictReference(Dictionary<T, int> conditionCounts)
        {
            ConditionCounts = conditionCounts;
        }

        public int UncoveredCount { get => _uncoveredCount; }

        public Dictionary<T,int> ConditionCounts
        {
            get
            {
                return new Dictionary<T,int>(_conditionCounts!);
            }
            set
            {
                _conditionCounts = value;
                if(_conditionCounts != null)
                    _uncoveredCount = _conditionCounts.Count;
            }
        }

        public bool TryDecreaseCount(T target)
        {
            if(_conditionCounts!.ContainsKey(target))
            {
                if(_conditionCounts[target]-- == 0)
                    _uncoveredCount--;
                return true;
            }
            else
                return false;
        }

        public bool TryDecreaseCount(T target, out bool metPartialCondition)
        {
            metPartialCondition = false;
            int temp = _uncoveredCount;
            bool succeed = TryDecreaseCount(target);
            if(temp != _uncoveredCount)
                metPartialCondition = true;
            
            return succeed;
        }


        public bool TryIncreaseCount(T target)
        {
            if(_conditionCounts!.ContainsKey(target))
            {
                if(_conditionCounts[target]++ == 1)
                    _uncoveredCount++;
                return true;
            }
            else
                return false;
        }

        public bool TryIncreaseCount(T target, out bool brokePartialCondition)
        {
            brokePartialCondition = false;
            int temp = _uncoveredCount;
            bool succeed = TryIncreaseCount(target);
            if(temp != _uncoveredCount)
                brokePartialCondition = true;
            return succeed;
        }
    }
}
