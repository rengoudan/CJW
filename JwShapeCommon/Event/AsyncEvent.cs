using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.Event
{
    public class AsyncEvent<TEventArgs>
    {
        private readonly List<Func<object?, TEventArgs, Task>> _handlers = new();

        public void Subscribe(Func<object?, TEventArgs, Task> handler)
        {
            if (handler != null)
                _handlers.Add(handler);
        }

        public void Unsubscribe(Func<object?, TEventArgs, Task> handler)
        {
            if (handler != null)
                _handlers.Remove(handler);
        }

        public async Task InvokeAsync(object? sender, TEventArgs args)
        {
            var tasks = _handlers.Select(h => SafeInvoke(h, sender, args));
            await Task.WhenAll(tasks);
        }

        private static async Task SafeInvoke(Func<object?, TEventArgs, Task> handler, object? sender, TEventArgs args)
        {
            try
            {
                await handler(sender, args);
            }
            catch (Exception ex)
            {
                // 你可以在这里记录日志或抛出自定义异常
                Console.Error.WriteLine($"AsyncEvent handler exception: {ex}");
            }
        }
    }

}
