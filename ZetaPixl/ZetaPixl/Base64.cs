
using System;

namespace ZetaPhase.ZetaPixl
{
	/// <summary>
	/// A class providing Base64 Methods
	/// </summary>
	public static class Base64
	{
		public static string _Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string _Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
	}
}
