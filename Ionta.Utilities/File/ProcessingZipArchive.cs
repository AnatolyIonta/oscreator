using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public static IEnumerable<byte[]> GetFilesFromFolder(byte[] zip, string foleder, string extension)
        {
            using var fileInMemory = new MemoryStream(zip);
            using (var archive = new ZipArchive(fileInMemory))
            {
                // Выбор всех файлов с расширением .html из папки Section
                var htmlFiles = archive.Entries.Where(entry => entry.FullName.StartsWith($"{foleder}/") && entry.FullName.EndsWith(extension));

                // Проход по каждому файлу и сохранение его содержимого в новой CustomPage
                foreach (var file in htmlFiles)
                {
                    using (var stream = file.Open())
                    {
                        yield return ReadFully(stream);
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