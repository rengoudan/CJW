using JwCore;
using JwShapeCommon.JwService.Models;
using RGB.Jw.JW.Dtos;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon
{
    public class GlobalEvent
    {
        static GlobalEvent instance;
        public static GlobalEvent GetGlobalEvent()
        {
            if (instance == null)
            {
                instance = new GlobalEvent();
            }
            return instance;
        }

        public EventHandler<ChangePageArgs> ChangeJwPage;

        public EventHandler<SetNewPageArgs> SetNewPages;


        public EventHandler<ShowParseLogArgs> ShowParseLogEvent;

        public EventHandler<ApiErrorArgs> ApiErrorEvent;

        public EventHandler LoginUserLoadEvent;

        public EventHandler<AddLinkPartArgs> AddLinkPartEvent;

        public EventHandler<ControlSelectedSquareArgs> ControlSelectedSquareEvent;

        public EventHandler<ControlSelectedSquareArgs> DeleteSelectedSquareEvent;

        /// <summary>
        /// 操作记录日志
        /// </summary>
        public EventHandler<OperateLogArgs> OperateLogEvent;
    }

    public class DeletePartArgs : EventArgs
    {
        public string DeleteId { get; set; }

        public DrawShapeType DrawShapeType { get; set; }
    }


    public class ApiErrorArgs : EventArgs
    {
        public ErrorInfo Error { get; set; }
    }

    public class ChangePageArgs : EventArgs
    {
        public bool ChangeHead { get; set; }
        public int PageId { get; set; }

        public JwProjectClientDto JwProject { get; set; }
    }

    public class SetNewPageArgs : EventArgs
    {
        public UIPage NewPage { get; set; }

        public int PageId { get; set; }

    }


    public class ShowParseLogArgs : EventArgs
    {
        public string Msg { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool ShowTime { get; set; }
    }

    public class AddLinkPartArgs : EventArgs
    {
        public JwLinkPart LinkPart { get; set; }
    }

    public class ControlSelectedSquareArgs : EventArgs
    {
        public string Id { get; set; }

        public DrawShapeType DrawShapeType { get; set; }

        public bool IsLianjie { get; set; }
    }

    public class OperateLogArgs : EventArgs
    {
        public JwOperateLogData JwOperateLogData { get; set; }
    }
}
