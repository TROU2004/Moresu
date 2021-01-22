using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Client.ClientBuild
{
    class FileOperate
    {
        public string Path1, Path2;
        public bool Move;

        public FileOperate(string path1, string path2, bool move)
        {
            Path1 = path1;
            Path2 = path2;
            Move = move;
        }

        public void Operate()
        {
            DoOperate(Path1, Path2);
        }

        public void OperateReverse()
        {
            DoOperate(Path2, Path1);
        }

        private void DoOperate(string a, string b)
        {
            if (File.Exists(b)) File.Delete(b);
            if (Move)
            {
                File.Move(a, b);
            }
            else
            {
                File.Copy(a, b);
            }
        }
    }
}
