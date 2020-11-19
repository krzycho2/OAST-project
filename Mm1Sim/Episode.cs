namespace Mm1Sim
{
    public class Episode
    {
        public double Time { get; set; }
        public EpisodeType EpisodeType { get; set; }
        public int ClientID { get; set; }
    }

    public enum EpisodeType
    {
        Arrival,
        ServiceStart,
        Departure
    }
}
