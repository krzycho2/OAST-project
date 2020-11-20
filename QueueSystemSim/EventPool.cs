using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class EventPool
    {
        private List<QEvent> Events { get; set; } = new List<QEvent>();
        public List<QEvent> OldEvents { get; private set; } = new List<QEvent>();

        public QEvent GET()
        {
            var episode = Events[0];
            Events.RemoveAt(0);
            return episode;
        }

        public void PUT(QEventType type, double episodeTime)
        {
            QEvent episode = null;
            if (type == QEventType.Arrival)
            {
                 episode = new QEvent { Time = episodeTime, Type = type };
            }
            Events.Add(episode);
        }

        public void AddOldEvents(List<QEvent> oldEvents)
        {
            oldEvents = oldEvents.OrderBy((qEvent) => qEvent.Time).ToList(); // Sortowanie rosnąco w czasie
            foreach (var qEvent in oldEvents)
                OldEvents.Add(qEvent);
        }
    }
}
