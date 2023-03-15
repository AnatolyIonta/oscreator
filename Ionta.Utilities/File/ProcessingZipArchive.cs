using System.IO.Compression;

namespace Ionta.Utilities
{
    public static class ProcessingZipArchive
    {
        public static IEnumerable<byte[]> GetZipeFile(byte[] zip)
        {
            using (Stream data = new MemoryStream(zip)) // The original data
            {
                Stream unzippedEntryStream; // Unzipped data from a file in the archive

                ZipArchive archive = new ZipArchive(data);
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return ReadFully(entry.Open()); // .Open will return a stream                                                      
                    }
                }
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}