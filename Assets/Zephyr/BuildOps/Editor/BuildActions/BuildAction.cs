using UnityEngine;
using System.Collections;

namespace BuildOps.BuildActions
{
    public abstract class BuildAction
    {

        public BuildAction()
        {
            BuildActionRunner.Instance.PreBuild += PreBuildAction;
            BuildActionRunner.Instance.OnBuild += OnBuildAction;
            BuildActionRunner.Instance.PostBuild += PostBuildAction;
        }

        public virtual void PreBuildAction() { }
        public virtual void OnBuildAction() { }
        public virtual void PostBuildAction() { }
    }
}

