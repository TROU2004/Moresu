using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Client.ClientBuild
{
    class BuildProperties
    {
        public List<FileOperate> fileOperates = new List<FileOperate>();

        public void AddFileOperates(FileOperate operate)
        {
            fileOperates.Add(operate);
        }

        public FileOperate AddGameDirOperates(string path1, bool move)
        {
            var operate = new FileOperate(path1, Path.Combine(GameClient.ClientDir, Path.GetFileName(path1)), move);
            fileOperates.Add(operate);
            return operate;
        }

        public void RemoveFileOperates(FileOperate operate)
        {
            fileOperates.Remove(operate);
        }

        public void DoAllOperates()
        {
            foreach (var operate in fileOperates)
            {
                operate.Operate();
            }
        }

        public void DoAllOperatesReverse()
        {
            foreach (var operate in fileOperates)
            {
                operate.OperateReverse();
            }
        }
    }
}
