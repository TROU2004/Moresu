using Moresu.Component.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Client.ClientBuild
{
    public class FileOperate
    {
        public string ProfileName, ItemName;
        public bool Move, ReverseMove;

        public FileOperate(string profileName, string itemName, bool move, bool reverseMove)
        {
            ProfileName = profileName;
            ItemName = itemName;
            Move = move;
            ReverseMove = reverseMove;
        }

        //From Profile to GameFolder
        public void Operate()
        {
            DoOperate(Path.Combine(Profiles.ProfilesDir, ProfileName, ItemName), Path.Combine(GameClient.ClientDir, ItemName), Move);
        }

        //From GameFolder to Profile
        public void OperateReverse()
        {
            
            DoOperate(Path.Combine(GameClient.ClientDir, ItemName), Path.Combine(Profiles.ProfilesDir, ProfileName, ItemName), ReverseMove);
        }

        private void DoOperate(string a, string b, bool move)
        {
            if (File.Exists(b)) File.Delete(b);
            if (move)
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
