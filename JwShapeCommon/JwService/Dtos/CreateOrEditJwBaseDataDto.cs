using RGB.Jw.JW;

using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class CreateOrEditJwBaseDataDto 
    {
        public long? Id { get; set; }
        public string DataName { get; set; }

        public string DataValue { get; set; }

        public BaseDataType LeveType { get; set; }

        public string TypeName { get; set; }

        public string Remark { get; set; }

    }
}