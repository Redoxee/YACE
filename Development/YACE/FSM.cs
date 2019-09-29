namespace YACE.FSM
{
    using System.Collections.Generic;

    public class FSM
    {
        private readonly string[] allStates;
        private string currentState;

        private List<StateWatcher> watchers;
        private Dictionary<string, List<StateWatcher>> watcherByState;
        private Dictionary<WatcherAction, string> stateByAction;

        private YACE yaceInstance;

        internal FSM(string[] allStates, string initialState, YACE yace)
        {
            this.allStates = allStates;
            System.Diagnostics.Debug.Assert(System.Array.IndexOf(allStates, initialState) > -1);
            this.currentState = initialState;
            this.watchers = new List<StateWatcher>();
            this.watcherByState = new Dictionary<string, List<StateWatcher>>();
            for (int index = 0; index < allStates.Length; ++index)
            {
                this.watcherByState[allStates[index]] = new List<StateWatcher>();
            }

            this.stateByAction = new Dictionary<WatcherAction, string>();

            this.yaceInstance = yace;
        }

        public void RegisterWatcher(string state, PlayerIndex playerIndex, WatcherAction action)
        {
            System.Diagnostics.Debug.Assert(System.Array.IndexOf(this.allStates, state) > -1);
            System.Diagnostics.Debug.Assert(!stateByAction.ContainsKey(action));

            int playerId = this.yaceInstance.Context.ConvertPlayerIndex(playerIndex);

            StateWatcher watcher = new StateWatcher()
            {
                StateName = state,
                WatchedPlayer = playerId,
                Action = action,
            };

            this.watcherByState[state].Add(watcher);
            this.stateByAction[action] = state;
        }

        public void UnregisterWatcher(WatcherAction action)
        {
            System.Diagnostics.Debug.Assert(stateByAction.ContainsKey(action));
            string state = this.stateByAction[action];
            this.stateByAction.Remove(action);

            int watcherIndex = this.watcherByState[state].FindIndex((StateWatcher watcher) => watcher.Action == action);
            System.Diagnostics.Debug.Assert(watcherIndex > -1);
            this.watcherByState[state].RemoveAt(watcherIndex);
        }

        public void SetState(string state)
        {
            if (this.currentState == state)
            {
                return;
            }

            System.Diagnostics.Debug.Assert(System.Array.IndexOf(this.allStates, state) > -1);
            this.currentState = state;

            int currentPlayerIndex = this.yaceInstance.Context.CurrentPlayer;

            List<StateWatcher> watchers = this.watcherByState[state];
            int numberOfWatcher = watchers.Count;
            for (int index = 0; index < numberOfWatcher; ++index)
            {
                if (watchers[index].WatchedPlayer < 0 || watchers[index].WatchedPlayer == currentPlayerIndex)
                {
                    watchers[index].Action();
                }
            }
        }
    }

    public delegate void WatcherAction();

    public class StateWatcher
    {
        public string StateName;
        public int WatchedPlayer;
        public WatcherAction Action;
    }
}
