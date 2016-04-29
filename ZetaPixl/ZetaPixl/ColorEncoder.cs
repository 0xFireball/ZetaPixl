/*

 */
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace ZetaPhase.ZetaPixl
{
	public class ColorEncoder
	{
		private int[,] argbData;
		public ColorEncoder(Color[,] imageData)
		{
			int w = imageData.GetLength(0);
			int h = imageData.GetLength(1);
			int[,] argb_Data = new int[w,h];
			for (int i = 0; i < w; i++)
			{
    			for (int j = 0; j < h; j++)
    			{
    				Color ccolor = imageData[i,j];
    				int cInt = ccolor.ToArgb();
    				argb_Data[i,j] = cInt;
    			}
			}
			argbData = argb_Data;
		}
		public int[,] GetIntPixels()
		{
			return argbData;
		}
		
		public static int[,] GetIntPixels(Color[,] imageData)
		{
			int w = imageData.GetLength(0);
			int h = imageData.GetLength(1);
			int[,] argb_Data = new int[w,h];
			for (int i = 0; i < w; i++)
			{
    			for (int j = 0; j < h; j++)
    			{
    				Color ccolor = imageData[i,j];
    				int cInt = ccolor.ToArgb();
    				argb_Data[i,j] = cInt;
    			}
			}
			return argb_Data;
		}
		
		public static Color[,] GetPixels(int[,] intPixels)
		{
			int w = intPixels.GetLength(0);
			int h = intPixels.GetLength(1);
			Color[,] colordata = new Color[w,h];
			for (int i = 0; i < w; i++)
			{
    			for (int j = 0; j < h; j++)
    			{
    				int cInt = intPixels[i,j];
    				Color ccolor = Color.FromArgb(cInt);
    				colordata[i,j] = ccolor;
    			}
			}
			return colordata;
		}
	}
}