namespace Archivers
{
    public interface IArchiver
    {
        void CompressFile(string filename, string fileOutName, out string filePath);

        void DecompressFile(string filename, string fileOutName, out string filePath);
    }
}
