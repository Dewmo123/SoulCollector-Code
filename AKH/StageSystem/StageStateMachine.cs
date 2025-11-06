using Scripts.StageSystem.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.StageSystem
{
    public enum StageStateEnum
    {
        Common,
        Boss,
        Dungeon
    }
    public class StageStateMachine
    {
        private Dictionary<StageStateEnum, StageState> _states;
        public StageState CurrentState { get; private set; }
        public StageStateMachine(StageManager manager)
        {
            _states = new Dictionary<StageStateEnum, StageState>();
            Assembly fsmAssembly = Assembly.GetAssembly(typeof(StageState));
            List<Type> types = fsmAssembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(StageState)))
                .ToList();
            types.ForEach(type => _states.Add(
                Enum.Parse<StageStateEnum>(type.Name.Replace("State", ""))
                , Activator.CreateInstance(type,new object[] { manager } ) as StageState));
        }
        public void ChangeState(StageStateEnum type, BaseStageDataSO data)
        {
            if (_states.TryGetValue(type, out StageState state))
            {
                CurrentState?.Exit();
                CurrentState = state;
                state.Enter(data);
            }
            else
            {
                Console.WriteLine($"{type}State does not exist");
                throw new System.Exception();
            }
        }
        public void UpdateState()
            => CurrentState.Update();
    }
}
