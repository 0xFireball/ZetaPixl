using System;
using System.Text;
using System.Linq;
 
namespace ZetaPhase.ZetaPixl
{
	/// <summary>
	/// A Vigenere Cipher provider class
	/// </summary>
    class VigenereCipher
    {
        [Flags]
        public enum LegalCharacters : byte
        {
            Alphabetical = 0x0,
            Alphanumerical = 0x1,
            Symbols = 0x2
        }
        private class CipherTable
        {
            public string RawStringRow { get; private set; }

            public byte[][] Table { get { return Create(); }}

            public static CipherTable operator +(CipherTable tbl1, CipherTable tbl2)
            {
                var fullString = new StringBuilder(tbl1.RawStringRow.Length + tbl2.RawStringRow.Length);
                fullString.Append(tbl1.RawStringRow);
                fullString.Append(tbl2.RawStringRow);

                return new CipherTable(fullString.ToString());
            }

            public CipherTable(string rawStringRow)
            {
                RawStringRow = rawStringRow;
            }

            internal bool VerifyString(string data)
            {
                return data.All(c => RawStringRow.Contains(c) || char.IsWhiteSpace(c));
            }

            private byte[][] Create()
            {
                var tbl = new byte[RawStringRow.Length][];
                var row = Encoding.Default.GetBytes(RawStringRow);

                for (var i = 0; i < RawStringRow.Length; i++)
                {
                    var pushedRow = new byte[row.Length];
                    Buffer.BlockCopy(row, 0, pushedRow, 0, row.Length);

                    if (i == 0)
                    {
                        CaesarShiftOne(ref pushedRow);
                        tbl[i] = pushedRow;
                        continue;
                    }

                    for (var j = 0; j < i + 1; j++)
                    {
                        CaesarShiftOne(ref pushedRow);
                        tbl[i] = pushedRow;
                    }
                }

                return tbl;
            }

            private static void CaesarShiftOne(ref byte[] orig)
            {
                var @new = new byte[orig.Length];
                Buffer.BlockCopy(orig, 0, @new, 0, orig.Length);

                var first = @new[0];

                for (var i = 0; i < @new.Length - 1; i++)
                    orig[i] = @new[i + 1];

                orig[orig.Length - 1] = first;
            }

            public override string ToString()
            {
                var tbl = Create();
                var finalStr = new StringBuilder();

                finalStr.Append("plain\t");

                foreach (char c in RawStringRow)
                    finalStr.Append(c + " ");

                finalStr.AppendLine();

                var curLen = finalStr.Length;
                for (var i = 0; i < curLen; i++)
                    finalStr.Append("_");

                finalStr.AppendLine();

                for (var i = 0; i < tbl.Length; i++)
                {
                    finalStr.Append(i + "\t");
                    for (var j = 0; j < tbl[i].Length; j++)
                        finalStr.Append((char) (tbl[i][j]) + " ");
                    finalStr.AppendLine();
                }

                return finalStr.ToString();
            }
        }

        private const string Alphabetical = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Alphanumerical = "0123456789";
        private const string Symbols = "!\"#¤%&/()=?@£$€{[]}:^´`½§_,.";

        private CipherTable CompleteTable { get; set; }
        private string KeyPhrase { get; set; }

        public VigenereCipher(LegalCharacters chars, string keyPhrase)
        {
            KeyPhrase = keyPhrase.Replace(" ", "");
            CompleteTable = new CipherTable("");

            if ((chars & LegalCharacters.Alphabetical) == LegalCharacters.Alphabetical)
                CompleteTable += new CipherTable(Alphabetical);

            if ((chars & LegalCharacters.Alphanumerical) == LegalCharacters.Alphanumerical)
                CompleteTable += new CipherTable(Alphanumerical);

            if ((chars & LegalCharacters.Symbols) == LegalCharacters.Symbols)
                CompleteTable += new CipherTable(Symbols);
        }

        private string ExtendKeyPhrase(int total)
        {
            var extended = new StringBuilder(total);
            var i = KeyPhrase.Length;

            extended.Append(KeyPhrase);
            while ((i += KeyPhrase.Length) < total)
                extended.Append(KeyPhrase);

            for (var j = 0; j < total - (i - KeyPhrase.Length); j++)
                extended.Append(KeyPhrase[j]);

            return extended.ToString();
        }

        public string EncryptData(string data)
        {
            var extKey = ExtendKeyPhrase(data.Length);
            var encrypted = new byte[data.Length];

            if (!CompleteTable.VerifyString(data))
                throw new ArgumentException("Plaintext data contains characters that does not exist in the cipher table");

            if(!CompleteTable.VerifyString(KeyPhrase))
                throw new ArgumentException("Key data contains characters that does not exist in the cipher table");

            var tbl = CompleteTable.Table;
            for (var i = 0; i < data.Length; i++)
            {
                if (char.IsWhiteSpace(data[i]))
                {
                    encrypted[i] = (byte) ' ';
                    continue;
                }

                int plainPos = 0;

                for (var j = 0; j < CompleteTable.RawStringRow.Length; j++)
                    if (data[i] == CompleteTable.RawStringRow[j])
                    {
                        plainPos = j;
                        break;
                    }

                foreach (var row in tbl.Where(row => row[0] == extKey[i]))
                {
                    encrypted[i] = row[plainPos];
                    break;
                }
            }

            return Encoding.Default.GetString(encrypted);
        }

        public string DecryptData(string encryptedData)
        {
            var extKey = ExtendKeyPhrase(encryptedData.Length);
            var decrypted = new byte[encryptedData.Length];
            var tbl = CompleteTable.Table;

            for (var i = 0; i < encryptedData.Length; i++)
            {
                if (char.IsWhiteSpace(encryptedData[i]))
                {
                    decrypted[i] = (byte)' ';
                    continue;
                }

                for (var j = 0; j < tbl.Length; j++)
                {
                    if (tbl[j][0] != extKey[i]) continue;
                    for(var x = 0; x < tbl[j].Length;x++)
                        if (tbl[j][x] == encryptedData[i])
                            decrypted[i] = (byte)CompleteTable.RawStringRow[x];
                }
            }

            return Encoding.Default.GetString(decrypted);
        }

        public override string ToString()
        {
            return CompleteTable.ToString();
        }
    }
}