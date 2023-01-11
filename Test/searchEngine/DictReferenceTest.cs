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
    public void TrayApply1_ReturnBoolCorrectly_TestHorizontally()
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
    

}
