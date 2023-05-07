using System;
namespace LeSploosh;

public class Squid
{
	public int Id { get; set; }

    public int size { get; set; }

    public int HitCounter { get; set; }

    public bool SquidStatus { get; set; } //True = alive, false = dead

    public List<int[]> squidPositions;

    public Squid() {

        squidPositions = new List<int[]>();

    }


	public void IncreaseHitCounter() 
	{
		this.HitCounter++;

        if (HitCounter == size)
            SquidStatus = false;
        else
            SquidStatus = true;
    }


    public void AddSquidPosition(int[] squidPosition)
    {
        squidPositions.Add(squidPosition);
    }

}