using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// namespace voor helper-klassen
namespace NoviKunstuitleen.Extensions
{
    // statisce klasse voor het verifieren van bestanden
    public static class FileHelperExtensions
    {
        // toegestande bestandsextensies:
        private static readonly string[] _permittedExtensions = { ".gif", ".png", ".jpg", ".jpeg" };


        // filesignatures om bestands-extensie met de inhoud te kunnen vergelijken (https://www.filesignatures.net/):
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 } } },
            { ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 } } }
        };


        /// <summary>
        /// Statische functie om bestand te verifieren
        /// </summary>
        /// <param name="fileName">bestandsnaam + extensie</param>
        /// <param name="data">Inhoud als stream</param>
        /// <param name="permittedExtensions">Toegestane waardes uit (.gif, .png, .jpg, .jpeg)</param>
        /// <param name="permittedFileSize">Maximaal toegestane grootte als long in bytes</param>
        /// <returns>true voor valide bestanden, false voor malafiede bestanden</returns>
        public static bool IsValidFile(string fileName, Stream data, string[] permittedExtensions, long permittedFileSize)
        {
            // check of het bestand inhoud heeft, niet te klein of te groot is.
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0 || data.Length > permittedFileSize) return false;

            // check of een bestandsextensie heeft 
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext)) return false;

            // check de inhoud van het bestand
            data.Position = 0;
            using (var reader = new BinaryReader(data))
            {
                // haal de juiste bytes op aan de hand van de fileextensie, en lees de headerbytes uit het bestand
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                // controleer of de headerbytes overeenkomen met één van de toegestane bytessets van de bestandsextensie
                return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}
