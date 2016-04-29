using System;
using System.Collections.Generic;
using System.Linq;
 
namespace ZetaPhase.ZetaPixl
{
    public class ShiftUtils
    {
    	private int[] datastream;
    	private int[] keystream;
    	
    	public static int[] GetIntStringFromMatrix(int[,] matrix)
    	{
    		int w = matrix.GetLength(0);
			int h = matrix.GetLength(1);
			List<int> intList = new List<int>();
			for (int i = 0; i < w; i++)
			{
    			for (int j = 0; j < h; j++)
    			{
    				intList.Add(matrix[i,j]);
    			}
			}
			return intList.ToArray();
    	}
    	public static int[,] GetMatrixFromIntString(int[] intArray, int _width, int _height)
    	{
    		int width = _height;
    		int height = _width;
    		int[,] matrix = new int[height,width];
    		//columns: width
    		//rows: height
    		for (int i = 0; i < height; i++)
		    {
		        for (int j = 0; j < width; j++)
		        {
		        	//i: row number
		        	//j: column number
		            matrix[i, j] = intArray[i * width + j];
		        }
		    }
    		return matrix;
    	}
    	
    	
    	public ShiftUtils(int[] data, int[] key)
    	{
    		int[] mdata = data;
    		List<int> mkey = new List<int>();
    		while (data.Length>mkey.Count)
    		{
    			mkey.AddRange(key);
    		}
    		keystream = mkey.ToArray();
    		datastream = mdata;
    	}
    	static int ShiftProtect(int x, int y)
    	{
    		return (x&y)+((x^y)/2);
    	}
    	static int MathMod(int x) {
    		int x_min = int.MinValue;
    		int x_max = int.MaxValue;
		    x = (((x - x_min) % (x_max - x_min)) + (x_max - x_min)) % (x_max - x_min) + x_min;
		    return x;
		}
    	public int DeriveShiftedByte(int p, int k, int d) //d is 1 to encrypt, 0 to decrypt
    	{
    		int sB = MathMod((p+k*d));//calculate shifted int
    		return sB;
    	}
    }
}