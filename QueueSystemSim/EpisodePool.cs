using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class EpisodePool
    {
        private List<Episode> Episodes { get; set; } = new List<Episode>();

        public Episode GET()
        {
            var episode = Episodes[0];
            Episodes.RemoveAt(0);
            return episode;
        }

        public void PUT(EpisodeType type, double episodeTime)
        {
            Episode episode = null;
            if (type == EpisodeType.Arrival)
            {
                 episode = new Episode { Time = episodeTime, Type = type };
            }
            Episodes.Add(episode);
        }
    }
}
