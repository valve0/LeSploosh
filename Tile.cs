namespace LeSploosh
{
    internal class Tile
    {

        public TileState SeaState { get; set; }
        public bool SquidPresent { get; set; }
        public bool Attackable { get; set; }
        public bool CrosshairBool { get; set; }

        public Tile ()
        {
            this.SeaState = TileState.GameStart;
            this.SquidPresent = false;
            this.Attackable = true;
            this.CrosshairBool = false;
        }
    }
}
