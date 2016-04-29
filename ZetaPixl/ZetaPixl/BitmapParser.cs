/*

 */
using System;
using System.Drawing;
using System.Collections.Generic;
using OmniBean.PowerCrypt4;

namespace ZetaPhase.ZetaPixl
{
	public class BitmapParser
	{
		private Color[,] imagePixels;		
		
		public Color[,] ImagePixels
		{
			get
			{
				return imagePixels;
			}
		}
		
		public int ImageHeight
		{
			get
			{
				return imagePixels.GetLength(1);
			}
		}
		
		public int ImageWidth
		{
			get
			{
				return imagePixels.GetLength(0);
			}
		}
		
		public static Bitmap GetBitmapFromFile(string filePath)
		{
			Bitmap b = new Bitmap(filePath);
			return b;
		}
		public static Bitmap GetBitmapFromPixels(Color[,] pixels)
		{
			int w = pixels.GetLength(0);
			int h = pixels.GetLength(1);
			Bitmap b = new Bitmap(w,h);
			for (int i = 0; i < w; i++)
			{
    			for (int j = 0; j < h; j++)
    			{
    				b.SetPixel(i,j,pixels[i,j]);
    			}
			}
			return b;
		}
		public BitmapParser(Bitmap img)
		{
			int w = img.Width;
			int h = img.Height;
			imagePixels = new Color[w,h];
			
			for (int i = 0; i < w; i++)
			{
			    for (int j = 0; j < h; j++)
			    {
			        Color pixel = img.GetPixel(i,j);
			        imagePixels[i,j] = pixel;
			    }
			}
		}
		
		public static int[,] SquareIntString(int[] intData)
		{
			int lN = intData.Length;
			double mS = Math.Sqrt((double)lN);
			int dMsq = (int)Math.Ceiling(mS);
			int paddBy = dMsq*dMsq-lN;
			List<int> iDtx = new List<int>();
			iDtx.AddRange(intData);
			while (paddBy > 0)
			{
				iDtx.Add(int.MinValue);
				paddBy--;
			}
			int[] n_intData = iDtx.ToArray();
			return ShiftUtils.GetMatrixFromIntString(n_intData, dMsq, dMsq);
		}
		
		public static int[] DeSquareIntString(int[,] squaredIntData)
		{
			int[] sqStrm = ShiftUtils.GetIntStringFromMatrix(squaredIntData);
			List<int> gsqStrm = new List<int>();
			gsqStrm.AddRange(sqStrm);
			for (int i = gsqStrm.Count; i --> 0; )
			{
				if (gsqStrm[i] == int.MinValue)
				{
					gsqStrm.RemoveAt(i);
				}
				else
				{
					break;
				}
			}
			return gsqStrm.ToArray();
		}
		
		public static string EncryptIntData(int[] intData, string password)
		{
			string b64 = __GetBase64FromIntData(intData);
			string key = PowerAES.SHA512Hash(password);
			string aes = PowerAES.Encrypt(b64, key);
			string b64data = Base64._Base64Encode(aes);
			return b64data;
		}
		public static int[] DecryptIntData(string encryptedIntData, string password)
		{
			string dB = Base64._Base64Decode(encryptedIntData);
			string key = PowerAES.SHA512Hash(password);
			string decryptedIntData = PowerAES.Decrypt(dB, key);
			int[] nIntdata = __GetIntDataFromBase64(decryptedIntData);
			return nIntdata;
		}
		
		static string __GetBase64FromIntData(int[] imgIntData)
		{
			string imS = string.Join(".",imgIntData);
			string b64s = Base64._Base64Encode(imS);
			return b64s;
		}
		static int[] __GetIntDataFromBase64(string b64ImgData)
		{
			string uBs = Base64._Base64Decode(b64ImgData);
			List<int> iD = new List<int>();
			string[] sPx = uBs.Split(new char[]{'.'},StringSplitOptions.RemoveEmptyEntries);
			foreach (string w in sPx)
			{
				iD.Add(int.Parse(w));
			}
			return iD.ToArray();
		}
		public static int[] EncodeBase64AsIntString(string b64)
		{
			List<int> intDataL = new List<int>();
			foreach (char c in b64)
			{
				intDataL.Add((int)c);
			}
			return intDataL.ToArray();
		}
		public static string DecodeBase64FromIntString(int[] eb64)
		{
			List<char> cData = new List<char>();
			foreach (int c in eb64)
			{
				cData.Add((char)c);
			}
			return new string(cData.ToArray());
		}
		
	}
}