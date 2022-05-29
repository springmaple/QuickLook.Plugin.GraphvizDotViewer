using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace QuickLook.Plugin.GraphvizDotViewer
{
    public static class GraphvizWrapper
    {
        public static byte[] RenderImage(string sourceFilePath, string format)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = @"dot.exe",
                Arguments = $@"-T{format} ""{sourceFilePath}""",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(processStartInfo);
            if (process == null)
                throw new Exception("Failed to start dot process");
            if (process.StandardOutput.BaseStream is FileStream outStream)
            {
                var bytes = ReadStream(outStream);
                if (bytes.Length > 0)
                    return bytes;
            }

            if (!(process.StandardError.BaseStream is FileStream errStream))
                throw new Exception("Failed to read error stream");
            var errMsg = Decode(ReadStream(errStream));
            throw new Exception(errMsg);
        }

        private static byte[] ReadStream(Stream stream)
        {
            int lastRead;
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[4096];
                do
                {
                    lastRead = stream.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, lastRead);
                } while (lastRead > 0);

                return ms.ToArray();
            }
        }

        private static string Decode(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}