// See https://aka.ms/new-console-template for more information

using System.IO.Compression;

public class Program
{

    private static byte[] s_messageBytes;

    public static void Main()
    {
        s_messageBytes = File.ReadAllBytes("50MB.zip");

        Console.WriteLine($"The original string length is {s_messageBytes.Length} bytes.");
        using var stream = new MemoryStream();
        CompressBytesToStream(stream);
        Console.WriteLine($"The compressed stream length is {stream.Length} bytes.");
        int decompressedLength = DecompressStreamToBytes(stream);
        Console.WriteLine($"The decompressed string length is {decompressedLength} bytes, same as the original length.");
        /*
         Output:
            The original string length is 445 bytes.
            The compressed stream length is 265 bytes.
            The decompressed string length is 445 bytes, same as the original length.
        */
    }



    private static void CompressBytesToStream(Stream stream)
    {
        using var compressor = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);
        compressor.Write(s_messageBytes, 0, s_messageBytes.Length);
    }

    private static int DecompressStreamToBytes(Stream stream)
    {
        stream.Position = 0;
        int bufferSize = 1000000000;
        byte[] decompressedBytes = new byte[bufferSize];
        using var decompressor = new DeflateStream(stream, CompressionMode.Decompress);
        int length = decompressor.Read(decompressedBytes, 0, bufferSize);
        return length;
    }
}
