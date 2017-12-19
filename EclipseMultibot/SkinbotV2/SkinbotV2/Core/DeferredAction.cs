using CommonBehaviors.Actions;
using Styx.CommonBot.Coroutines;
using Styx.TreeSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Eclipse.MultiBot.Core
{
    public class DeferredAction : Composite
    {
        public static List<DAction> dactions = new List<DAction>();

        public DeferredAction(ActionDelegate action = null)
        {
            Runner = action;
        }

        public DeferredAction(ActionSucceedDelegate action = null)
        {
            SucceedRunner = action;
        }

        public ActionDelegate Runner { get; private set; }

        public ActionSucceedDelegate SucceedRunner { get; private set; }

        /// <summary>
        ///   Runs this action, and returns a <see cref = "RunStatus" /> describing it's current state of execution.
        ///   If this method is not overriden, it returns <see cref = "RunStatus.Failure" />.
        /// </summary>
        /// <returns></returns>
        protected virtual RunStatus Run(object context)
        {
            return RunStatus.Failure;
        }

        protected override sealed IEnumerable<RunStatus> Execute(object context)
        {
            if (Runner != null)
            {
                yield return Runner(context);
            }
            else if (SucceedRunner != null)
            {
                SucceedRunner(context);
                yield return RunStatus.Success;
            }
            else
            {
                yield return Run(context);
            }
        }

        public static bool DoAction(){

            var act = dactions.Where(a => DateTime.Now >=a.ExecutionTime);
            if (act.Count() > 0)
            {

                DAction result = act.FirstOrDefault();
                EC.Log(string.Format("{0}>{1}", result.ExecutionTime, DateTime.Now));
                if (result.HasRun)
                {
                    dactions.Remove(result);
                    return DoAction();
                }
                result.HasRun = true;
                return result.Action.Invoke();
            }
            else return false;
        }
        public DeferredAction(int ms, string Name, Func<bool> action)
        {
            
            if (dactions.Where(d => d.Name == Name).Count() == 0)
            {
                EC.Log("adding a new deferred action:" + Name);
                dactions.Add(new DAction(Name, ms, action));
                DoAction();
            }
            else
            {
                //EC.Log("action already exists - ignoring");
            }
        }
    }
    public class DAction{
        public DAction(string name, int duration, Func<bool> action){
            Name = name;
            ExecutionTime = DateTime.Now.AddMilliseconds(duration);
            Action = action;
        }
        public string Name {get;set;}
        public DateTime ExecutionTime {get;set;}
        public Func<bool> Action { get; set; }
        public bool HasRun = false;

    }


}
