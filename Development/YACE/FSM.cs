namespace YACE.FSM
{
    using System.Collections.Generic;

    public class FSM
    {
        private readonly string[] allStates;
        private string currentState;

        private List<StateWatcher> watchers;
        private Dictionary<string, List<StateWatcher>> watcherByState;

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

            this.yaceInstance = yace;
        }

        public void RegisterWatcher(string state, PlayerIndex playerIndex, WatcherAction action)
        {
            System.Diagnostics.Debug.Assert(System.Array.IndexOf(this.allStates, state) > -1);
            int playerId = this.yaceInstance.Context.ConvertPlayerIndex(playerIndex);

            StateWatcher watcher = new StateWatcher()
            {
                StateName = state,
                WatchedPlayer = playerId,
                Action = action,
            };

            this.watcherByState[state].Add(watcher);
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
