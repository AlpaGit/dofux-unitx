using System;
using System.IO;
using System.Linq;
using DofusCoube.ThirdParty.IO.Binary;
using DofusCoube.ThirdParty.IO.Compression;

namespace DofusCoube.FileProtocol.Ele
{
    public sealed class EleReader
    {
        private BigEndianReader _reader;

        public EleReader(string filePath) =>
            _reader = new(File.ReadAllBytes(filePath));
    
        public EleReader(byte[] data) =>
            _reader = new(data);

        public EleInstance ReadElements()
        {
            _reader.Seek(0);

            var header = _reader.ReadInt8();
            if (header == 69)
            {
                return EleInstance.ReadFromStream(_reader);
            }

            try
            {
                _reader.Seek(0);
                var memoryStream = new MemoryStream();

                ZipCompressor.Deflate(new MemoryStream(_reader.ReadSpan(_reader.BytesAvailable).ToArray()), memoryStream);
                var array = memoryStream.ToArray();

                if (array.Length == 0 || array[0] != 69)
                {
                    throw new FileLoadException("Wrong header file");
                }

                _reader = new(array);
                _reader.ReadUInt8();
            }
            catch (Exception)
            {
                throw new FileLoadException("Wrong header file");
            }

            return EleInstance.ReadFromStream(_reader);
        }
    }
}