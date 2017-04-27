using System;

namespace osmutil
{
    public interface IOperation
    {
        void DoIt(Action<string, bool> feedback, bool dryRun);
    }
}