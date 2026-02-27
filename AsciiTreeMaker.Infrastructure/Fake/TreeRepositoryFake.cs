using AsciiTreeMaker.Domain.Repositories;
using System.Diagnostics;

namespace AsciiTreeMaker.Infrastructure.Fake
{
    /// <summary>
    /// 木構造データのFake
    /// </summary>
    internal sealed class TreeRepositoryFake : ITreeRepository
    {
        private List<ITreeNode> _roots = new();

        public enum FakeDataKind
        {
            Typical,
            MaxDepth,
            Large
        }

        public TreeRepositoryFake(FakeDataKind kind)
        {
            switch (kind)
            {
                case FakeDataKind.Typical:
                    _roots.Add(CreateTypicalTree());
                    break;

                case FakeDataKind.MaxDepth:
                    _roots.Add(CreateMaxDepthTree());
                    break;

                case FakeDataKind.Large:
                    // 511 ノード
                    // すべて展開するのに5秒程度かかる
                    //_roots.Add(CreateLargeTree(2, 9));

                    // 1023 ノード
                    // すべて展開するのに14秒程度かかる
                    //_roots.Add(CreateLargeTree(2, 10));

                    // 3280ノード
                    _roots.Add(CreateLargeTree(3, 8));

                    // 16383 ノード
                    //_roots.Add(CreateLargeTree(2, 14));

                    // 29524 ノード, JSONファイルサイズ 7.18 MB
                    //_roots.Add(CreateLargeTree(3, 10));

                    // 32767 ノード, JSONファイルサイズ 10.7 MB
                    //_roots.Add(CreateLargeTree(2, 15)); 
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        /// <summary>
        /// 木構造のルートノード群を読み込む
        /// </summary>
        /// <returns>木構造のルートノード群</returns>
        public IReadOnlyList<ITreeNode> LoadTree()
        {
            return _roots;
        }

        /// <summary>
        /// 木構造データを保存する
        /// </summary>
        /// <param name="roots">木構造のルートノード群</param>
        public void SaveTree(IReadOnlyList<ITreeNode> roots)
        {
            // Fakeデータは永続化しない
            return;
        }

        /// <summary>
        /// 通常利用を想定した代表的な木構造データを生成する
        /// </summary>
        /// <returns></returns>
        private static ITreeNode CreateTypicalTree()
        {
            var root =
            new FakeTreeNode("root",
                new FakeTreeNode("bin"),
                new FakeTreeNode("etc"),
                new FakeTreeNode("usr",
                    new FakeTreeNode("local",
                        new FakeTreeNode("app")
                        )
                    ),
                new FakeTreeNode("lib")
            );

            return root;
        }

        /// <summary>
        /// JSON の既定最大深度（技術的上限）まで階層を持つ木構造データを生成する
        /// </summary>
        /// <returns></returns>
        private static ITreeNode CreateMaxDepthTree()
        {
            // JSON データを解析するときに許可される最大深度は 64
            // https://learn.microsoft.com/ja-jp/dotnet/api/system.text.json.jsondocumentoptions.maxdepth
            int maxLevel = 10;

            // 最深ノード
            FakeTreeNode current = new FakeTreeNode(maxLevel.ToString());

            for (int level = maxLevel - 1; level >= 1; level--)
            {
                current = new FakeTreeNode(level.ToString(), current);
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

        private static FakeTreeNode CreateNode(
            string name,
            int currentDepth,
            int childrenPerNode,
            int maxDepth)
        {
            // 最大階層に達したら葉ノード
            if (currentDepth >= maxDepth || childrenPerNode == 0)
            {
                return new FakeTreeNode(name);
            }

            var children = new FakeTreeNode[childrenPerNode];

            for (int i = 0; i < childrenPerNode; i++)
            {
                var childName = "12345678901234567890123456789012345678901234567890"; // 最大文字数まで付与する
                children[i] = CreateNode(childName, currentDepth + 1, childrenPerNode, maxDepth);
            }

            return new FakeTreeNode(name, children);
        }

    }
}
