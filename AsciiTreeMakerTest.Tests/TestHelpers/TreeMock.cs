using AsciiTreeMaker.Domain.Repositories;

namespace AsciiTreeMakerTest.Tests.TestHelpers
{
    /// <summary>
    /// [テスト用] 木構造のMock
    /// </summary>
    internal sealed class TreeMock : ITreeRepository
    {
        /// <summary>
        /// テスト用木構造データの種別
        /// </summary>
        public enum TreeMockKind
        {
            Typical,
            MaxDepth,
            NodeCountIs511,
            NodeCountIs3000
        }

        private readonly TreeMockKind _kind;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kind"></param>
        public TreeMock(TreeMockKind kind)
        {
            _kind = kind;
        }

        public IReadOnlyList<ITreeNode> LoadTree()
        {
            return _kind switch
            {
                TreeMockKind.Typical => new List<ITreeNode> { CreateTypicalTreeForTest() },
                TreeMockKind.MaxDepth => new List<ITreeNode> { CreateMaxDepthTree() },
                TreeMockKind.NodeCountIs511 => new List<ITreeNode> { CreateLargeTree(2, 9) },
                TreeMockKind.NodeCountIs3000 => new List<ITreeNode> { CreateLargeTree(2999, 2) },
                _ => new List<ITreeNode> { CreateTypicalTreeForTest() },
            };
        }

        public void SaveTree(IReadOnlyList<ITreeNode> roots)
        {
            return;
        }

        /// <summary>
        /// テスト用の木構造データを生成する
        /// </summary>
        /// <returns></returns>
        private static ITreeNode CreateTypicalTreeForTest()
        {
            /*
                0
                ├─ 0-0
                ├─ 0-1
                │  └─ 0-1-0
                ├─ 0-2
                │  ├─ 0-2-0
                │  │  ├─ 0-2-0-0
                │  │  └─ 0-2-0-1
                │  └─ 0-2-1
                └─ 0-3
             */
            var root =
            new TreeNodeMock("0",
                new TreeNodeMock("0-0"),
                new TreeNodeMock("0-1",
                    new TreeNodeMock("0-1-0")),
                new TreeNodeMock("0-2",
                    new TreeNodeMock("0-2-0",
                        new TreeNodeMock("0-2-0-0"),
                        new TreeNodeMock("0-2-0-1")
                        ),
                    new TreeNodeMock("0-2-1")
                    ),
                new TreeNodeMock("0-3")
            );

            return root;
        }

        /// <summary>
        /// 最大階層のノードを持つ木構造データを生成する
        /// </summary>
        /// <returns></returns>
        private static ITreeNode CreateMaxDepthTree()
        {
            int maxLevel = 10;

            // 最深ノード
            TreeNodeMock current = new TreeNodeMock(maxLevel.ToString());

            for (int level = maxLevel - 1; level >= 1; level--)
            {
                current = new TreeNodeMock(level.ToString(), current);
            }

            return current;
        }

        /// <summary>
        /// 各ノードが指定された数の子ノードを持つ木構造を生成する。
        /// ルート階層を 1 とし、最大階層が maxDepth になるように生成する。
        /// </summary>
        /// <param name="childrenPerNode">各ノードが持つ子ノードの数</param>
        /// <param name="maxDepth">ルートを 1 としたときの最大階層</param>
        /// <returns>生成された木構造のルートノード</returns>
        /// <remarks>ノード数は childrenPerNode^maxDepth のオーダー</remarks>
        private static ITreeNode CreateLargeTree(
            int childrenPerNode,
            int maxDepth)
        {
            long totalNodes;

            if (childrenPerNode == 0)
            {
                totalNodes = 1; // root のみ
            }
            else if (childrenPerNode == 1)
            {
                totalNodes = maxDepth; // 直線
            }
            else
            {
                totalNodes = ((long)Math.Pow(childrenPerNode, maxDepth) - 1) / (childrenPerNode - 1);
            }

            string rootName = $"Total Nodes Count: {totalNodes}";
            return CreateNode(rootName, 1, childrenPerNode, maxDepth);
        }

        private static TreeNodeMock CreateNode(
            string name,
            int currentDepth,
            int childrenPerNode,
            int maxDepth)
        {
            // 最大階層に達したら葉ノード
            if (currentDepth >= maxDepth || childrenPerNode == 0)
            {
                return new TreeNodeMock(name);
            }

            var children = new TreeNodeMock[childrenPerNode];

            for (int i = 0; i < childrenPerNode; i++)
            {
                var childName = "12345678901234567890123456789012345678901234567890"; // 最大文字数まで付与する
                children[i] = CreateNode(childName, currentDepth + 1, childrenPerNode, maxDepth);
            }

            return new TreeNodeMock(name, children);
        }
    }
}
