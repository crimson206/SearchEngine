namespace Test;
using SearchEngine;
using System.Collections.Generic;

[TestClass]
public class DictReferenceUnitTest
{
    [TestMethod]
    public void Initiallize_With_ConditionCounts()
    {
        Dictionary<char, int> conditionCounts = new Dictionary<char, int>{{'t', 0 }};

        DictReference<char> dictReference = new DictReference<char>(conditionCounts);

        
    }
}