using System;
namespace LeSploosh;

public class Squid
{

    public int Size { get; set; }

    public int HitCounter { get; set; }

	// true = alive, false = dead.
	private bool squidStatus;
	public bool SquidStatus { 
		
		get{ return squidStatus; }
		
		set
		{
			if (HitCounter == Size)
				squidStatus = false;
			else
				squidStatus = true;
		}
	
	}

    public Squid(int size)
	{
		Size = size;
		squidStatus = true; //default to true i.e alive
	
	}

	public void IncreaseHitCounter() 
	{
		HitCounter++;

        if (HitCounter == Size)
            squidStatus = false;
        else
            squidStatus = true;
    }



}
