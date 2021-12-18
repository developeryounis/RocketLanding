namespace RocketLanding.Library
{
    public class LandingAreaOptions
    {
        public int LandingPlateformSizeX { get; set; } = 10;
        public int LandingPlateformStartY { get; set; } = 5;
        public int LandingPlateformStartX { get; set; } = 5;
        public int LandingPlateformSizeY { get; set; } = 10;
        public bool AllowOnlyLastCheck { get; set; } = false;
    }
}
