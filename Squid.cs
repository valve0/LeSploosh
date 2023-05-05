using System;
namespace LeSploosh;

public class Squid
{
	public int Id { get; set; }

    public int Size { get; set; }

    public int HitCounter { get; set; }

    public bool SquidStatus { get; set; } //True = alive, false = dead

    private List<int[]> squidPositions;

    public Squid() {

        squidPositions = new List<int[]>();

    }


	public void IncreaseHitCounter() 
	{
		this.HitCounter++;

        if (HitCounter == Size)
            SquidStatus = false;
        else
            SquidStatus = true;
    }


    public void AddSquidPosition(int[] squidPosition)
    {
        squidPositions.Add(squidPosition);
    }

}