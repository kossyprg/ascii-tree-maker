using AsciiTreeMaker.Domain;
using AsciiTreeMaker.Domain.Repositories;
using AsciiTreeMaker.Infrastructure.Fake;
using AsciiTreeMaker.Infrastructure.Json;

namespace AsciiTreeMaker.Infrastructure
{
    public static class Factories
    {
        /// <summary>
        /// データを永続化するためのリポジトリを作成する
        /// </summary>
        /// <returns>木構造データリポジトリー</returns>
        public static ITreeRepository CreateTreeRepository()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                return new TreeRepositoryFake(TreeRepositoryFake.FakeDataKind.Typical);
                //return new TreeRepositoryFake(TreeRepositoryFake.FakeDataKind.Large);
            }
#endif
            return new TreeRepositoryJson();
        }

        /// <summary>
        /// ファイルI/Oを行うためのTreeDocumentStoreを作成する
        /// </summary>
        /// <returns>TreeDocumentStore</returns>
        public static ITreeFileIO CreateTreeFile()
        {
            return new TreeFileIOJson();
        }
    }
}
