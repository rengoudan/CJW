using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class JwTempHelper
    {
        private string[] _readlines;
        private SettingObject _settingObject;
        public List<string> ReadLineslst;

        /// <summary>
        /// 主要处理过滤匹配temp txt
        /// </summary>
        /// <param name="readlines"></param>
        /// <param name="setting"></param>
        public JwTempHelper(string[] readlines,SettingObject setting)
        {
            _readlines = readlines;
            ReadLineslst = _readlines.ToList();
            _settingObject = setting;
            parse();
        }

        

        private void parse()
        {
            bool isbeampoint=false;
            beampoints = new List<string>();
            shapestrs = new List<string>();
            taggstrs = new List<string>();
            for(int i=0; i< _readlines.Length; i++)
            {
                string line = _readlines[i];
                if(!string.IsNullOrEmpty(line) )
                {
                    if(!isbeampoint && Regex.IsMatch(line, _settingObject.ParseColor))
                    {
                        isbeampoint = true;
                    }
                    else
                    {
                        string pattern3 = @"^[A-Za-z!~~`#$%^&*()_+-=]";//判断数字
                        bool z = Regex.IsMatch(line, pattern3);
                        if (!z && isbeampoint)
                        {
                            beampoints.Add(line);
                        }
                        else
                        {
                            if (Regex.IsMatch(line, "lt"))
                            {
                                //isbeampoint = true;
                            }
                            else
                            {
                                isbeampoint = false;
                            }
                            if (Regex.IsMatch(line, @"^sl.*"))
                            {
                                isbeampoint = false;
                                shapestrs.Add(line);
                            }
                            if (Regex.IsMatch(line, @"^ch.*"))
                            {
                                isbeampoint = false;
                                taggstrs.Add(line);
                            }
                        }
                    }
                }
            }
            beampoints= beampoints.Distinct().ToList();//去重复
        }

        public List<string> beampoints { get; set; }

        public List<string> shapestrs { get; set; } 

        public List<string> taggstrs { get; set; }

    }
}
