namespace Test;
using SearchEngine;
using System.Collections.Generic;

[TestClass]
public class DictReferenceUnitTest
{
    [TestMethod]
    public void Initialize_With_ConditionCounts()
    {
        Dictionary<char, int> conditionCounts = new Dictionary<char, int>{{'t', 0 }};

        DictReference<char> dictReference = new DictReference<char>(conditionCounts);

        Assert.AreEqual(dictReference.GetConditionInfoCopy['t'], 0);
    }
    [TestMethod]
    public void Set_Condition_Counts_By_Initializing()
    {

        Dictionary<char, int> conditionCounts =
        new Dictionary<char, int>
        {
            {'a' , 2},
            {'b', 1},
            {'c', 3},
        };
        /// this is DictReference1 from the next test
        DictReference<char> reference = new DictReference<char>(conditionCounts);

        Assert.AreEqual(conditionCounts.Count, reference.UncoveredConditionCount);
        Assert.AreEqual(conditionCounts.Count, reference.TotalConditionCount);
    }

    public DictReference<char> GetDictReference1()
    {
        Dictionary<char, int> conditionCounts =
        new Dictionary<char, int>
        {
            {'a' , 2},
            {'b', 1},
            {'c', 3},
        };
        
        return new DictReference<char>(conditionCounts);
    }

    [TestMethod]
    public void TryApply1_ReturnBoolCorrectly_TestHorizontally()
    {
        /// {'a', 2}, {'b', 1}, {'c', 3}
        DictReference<char> dictReference = this.GetDictReference1();

        char[] keys = new char[]{'a', 'b', 'c', 'd', 'e'};
        bool[] expectedBools = new bool[]{true,true,true,false,false};

        for(int i = 0; i < 5; i++)
        {
            Assert.AreEqual(dictReference.TryApply(keys[i]), expectedBools[i]);
        }
    }

    public delegate bool DelTryApply1OrDeApply1 (char target);
    public delegate bool DelTryApply2OrDeApply2 (char target, out bool output);
    public DelTryApply1OrDeApply1[] GetDelTryApply1AndDeApply1(DictReference<char> target)
    {
        DelTryApply1OrDeApply1 delTryApply1 = target.TryApply;
        DelTryApply1OrDeApply1 delTryDeApply1 = target.TryDeApply;
        return new DelTryApply1OrDeApply1[]{delTryApply1,delTryDeApply1};
    }
    public DelTryApply2OrDeApply2[] GetDelTryApply2AndDeApply2(DictReference<char> target)
    {
        DelTryApply2OrDeApply2 delTryApply1 = target.TryApply;
        DelTryApply2OrDeApply2 delTryDeApply1 = target.TryDeApply;
        return new DelTryApply2OrDeApply2[]{delTryApply1,delTryDeApply1};
    }

    [TestMethod]
    public void TryApply1_TryDeApply2_ReturnBoolCorrectly_TestVertically()
    {
        /// {'a', 2}, {'b', 1}, {'c', 3}
        DictReference<char> dictReference = this.GetDictReference1();
        DelTryApply1OrDeApply1[] apl_deApl = this.GetDelTryApply1AndDeApply1(dictReference);

        int[] sequence = new int[]{0, 0, 0, 1, 1, 1, 0, 0, 0};
        string chars = "cd";
        bool[] expectedBools = new bool[]{true, false};

        for(int i = 0; i < sequence.Length; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                Assert.AreEqual(apl_deApl[sequence[i]](chars[j]),expectedBools[j]);
            }
        }
    }

}
