using System;
namespace LeSploosh;

public class Squid
{

    public int Size { get; set; }

    public int[,] Positions { get; set; }

    public Squid(int size)
	{
		Size = size;
	
	}
}
