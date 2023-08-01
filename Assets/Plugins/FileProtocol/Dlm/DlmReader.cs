using System;
using System.IO;
using DofusCoube.ThirdParty.IO.Binary;
using DofusCoube.ThirdParty.IO.Compression;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class DlmReader : IDisposable
    {
        public delegate string KeyProvider(int mapId);

        public const int Version = 11;
        private BigEndianReader _reader;

        public DlmReader(string filePath) =>
            _reader = new(File.ReadAllBytes(filePath));

        public DlmReader(string filePath, string decryptionKey)
        {
            _reader       = new(File.ReadAllBytes(filePath));
            DecryptionKey = decryptionKey;
        }

        public DlmReader(BigEndianReader reader) =>
            _reader = reader;

        public DlmReader(byte[] buffer) =>
            _reader = new(buffer);

        public DlmReader(byte[] buffer, string decryptionKey)
        {
            _reader       = new(buffer);
            DecryptionKey = decryptionKey;
        }

        public string? DecryptionKey { get; set; } = "649ae451ca33ec53bbcbcc33becf15f4";
    

        public void Dispose()
        {
        }

        public DlmMap ReadMap()
        {
            _reader.Seek(0);
            if (_reader.ReadUInt8() == 77)
            {
                return DlmMap.ReadFromStream(_reader, this);
            }

            try
            {
                _reader.Seek(0);
                var buffer = ZipCompressor.UncompressV2(_reader.ReadSpan(_reader.BytesAvailable));
                _reader = new(buffer);
                if (_reader.ReadUInt8() != 77)
                {
                    throw new FileLoadException("Wrong header file");
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Error reading: " + ex.Message);
            }

            return DlmMap.ReadFromStream(_reader, this);
        }
    }
}