using JwData;
using NetTopologySuite.Utilities;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBJWMain.Pages
{
    /// <summary>
    /// xiugai
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class JwBasePage:UIPage
    //public class JwBasePage<TModel>:UIPage where TModel : class
    {
        public JwDataContext? dbContext;

        //public BindingList<TModel>? BindingList
        //public BindingList? BindingList;

        public virtual void InitData()
        {
            //dbContext=new JwDataContext();
            //BindingList = dbContext.Set<TModel>().Local.ToBindingList();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this.dbContext?.Dispose();
            this.dbContext = null;
        }
    }
}
