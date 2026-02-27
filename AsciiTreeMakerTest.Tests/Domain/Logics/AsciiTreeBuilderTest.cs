using AsciiTreeMaker.Domain.Logics;
using AsciiTreeMakerTest.Tests.TestHelpers;

namespace AsciiTreeMakerTest.Tests.Domain.Logics
{
    [TestClass]
    public class AsciiTreeBuilderTest
    {
        [TestMethod]
        public void アスキーツリーの作成()
        {
            var x = new TreeMock(TreeMock.TreeMockKind.Typical);
            var roots = x.LoadTree();
            var root = roots[0];

            string result = AsciiTreeBuilder.Build(root, 2);
            string expected =
@"0
├── 0-0
├── 0-1
│   └── 0-1-0
├── 0-2
│   ├── 0-2-0
│   │   ├── 0-2-0-0
│   │   └── 0-2-0-1
│   └── 0-2-1
└── 0-3
";

            Assert.AreEqual(expected, result);

            // 枝の長さを変える
            result = AsciiTreeBuilder.Build(root, 1);
            expected =
@"0
├─ 0-0
├─ 0-1
│  └─ 0-1-0
├─ 0-2
│  ├─ 0-2-0
│  │  ├─ 0-2-0-0
│  │  └─ 0-2-0-1
│  └─ 0-2-1
└─ 0-3
";

            Assert.AreEqual(expected, result);

            // 枝の長さを変える
            result = AsciiTreeBuilder.Build(root, 0);
            expected =
@"0
├ 0-0
├ 0-1
│ └ 0-1-0
├ 0-2
│ ├ 0-2-0
│ │ ├ 0-2-0-0
│ │ └ 0-2-0-1
│ └ 0-2-1
└ 0-3
";

            Assert.AreEqual(expected, result);
        }

        
    }
}