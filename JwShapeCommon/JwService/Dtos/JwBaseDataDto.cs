using RGB.Jw.JW;

using System;

namespace RGB.Jw.JW.Dtos
{
    public class JwBaseDataDto     {
        public long Id { get; set; }
        public string DataName { get; set; }

        public string DataValue { get; set; }

        public BaseDataType LeveType { get; set; }

        public string TypeName { get; set; }

        public string Remark { get; set; }

    }
}