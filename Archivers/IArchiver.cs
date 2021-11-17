namespace Archivers
{
    public interface IArchiver
    {
        string Compress(string data);

        string Decompress(string data);
    }
}
