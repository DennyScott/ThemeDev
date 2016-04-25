using UnityEngine;
using System.Collections;

namespace BuildOps.BuildActions
{
    public sealed class BuildActionRunner
    {

        public System.Action PreBuild;
        public System.Action OnBuild;
        public System.Action PostBuild;

        private readonly static BuildActionRunner _runner = new BuildActionRunner();

        public void TriggerPreBuild()
        {
            PreBuild.Run();
        }

        public void TriggerOnBuild()
        {
            OnBuild.Run();
        }

        public void TriggerPostBuild()
        {
            PostBuild.Run();
        }

        public static BuildActionRunner Instance
        {
            get
            {
                return _runner;
            }
        }
    }
}

