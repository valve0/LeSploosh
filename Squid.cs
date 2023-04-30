using System;
namespace LeSploosh;

public class Squid
{

	private int id;
	public int Id
	{

		get { return id; }

		set
		{
			id = value;
		}
	}

    public int Size { get; set; }

    public int HitCounter { get; set; }

	private List<int[]> squidPositions = new List<int[]>();

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

    public Squid(int size, int id)
	{
		Size = size;
		squidStatus = true; //default to true i.e alive
		Id = id;

	
	}

	public void IncreaseHitCounter() 
	{
		this.HitCounter++;

        if (HitCounter == Size)
            this.squidStatus = false;
        else
            this.squidStatus = true;
    }


	public void AddSquidPosition(int[] squidPosition)
	{
		squidPositions.Add(squidPosition);
    }



}
