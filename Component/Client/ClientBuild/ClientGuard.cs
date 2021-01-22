using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Moresu.Component.Client.ClientBuild
{
    class ClientGuard
    {
        public static bool GetOsuRunningState()
        {
            return Process.GetProcessesByName("osu!").Length != 0;
        }

        public static void RunOsuWithGuard(Action callback)
        {
            var process = new Process();
            process.StartInfo.FileName = Path.GetFullPath(Path.Combine(GameClient.ClientDir, "osu!.exe"));
            process.StartInfo.WorkingDirectory = GameClient.ClientDir;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(4000);
                while (GetOsuRunningState())
                {
                    Thread.Sleep(1000);
                }
                callback.Invoke();
            })).Start();
        }
    }
}
