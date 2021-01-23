using Moresu.Component.Profile;
using OsuParsers.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Moresu.Component.Client.ClientBuild
{
    public class BuildProperties
    {
        public List<FileOperate> fileOperates = new List<FileOperate>();

        public void AddOperate(FileOperate operate)
        {
            fileOperates.Add(operate);
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
