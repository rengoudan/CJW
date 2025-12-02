using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGBControls.Classes
{
    public static class HelperEx
    {
        public static void SuccessModal(this Form form, string msg)
        {
            AntdUI.Modal.open(new AntdUI.Modal.Config(form, "ヒント", msg, AntdUI.TType.Success)
            {
                OnButtonStyle = (id, btn) =>
                {
                    btn.BackExtend = "135, #6253E1, #04BEFE";
                },
                CancelText = null,
                OkText = "確認する"
            });
        }
    }
}
