using JwCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class DirectionConfig
    {
        public TaggDirect Direction;
        public IEnumerable<IGrouping<double, JwBeam>> Groups;
        public double Jxpd;
        public Func<IGrouping<double, JwBeam>, List<(JwBeam winner, JwBeam loser, double qKey)>> Matcher;
        public Action<JwBeam, JwBeam, double, TaggDirect> Processor;

        public DirectionConfig(
            TaggDirect dir,
            IEnumerable<IGrouping<double, JwBeam>> groups,
            double jxpd,
            Func<IGrouping<double, JwBeam>, List<(JwBeam winner, JwBeam loser, double qKey)>> matcher,
            Action<JwBeam, JwBeam, double, TaggDirect> processor)
        {
            Direction = dir;
            Groups = groups;
            Jxpd = jxpd;
            Matcher = matcher;
            Processor = processor;
        }

        public void Process()
        {
            foreach (var group in Groups)
            {
                var pairs = Matcher(group);
                foreach (var (winner, loser, qKey) in pairs)
                    Processor(winner, loser, qKey, Direction);
            }
        }
    }


}
