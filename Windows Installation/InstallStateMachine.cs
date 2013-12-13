using System;

namespace Windows_Installation
{
    public class InstallStateMachine
    {
        
        // --- States ---
        public static State infoState = new State("Info zur Installation", 0);
        public static State silentState = new State("Installation soll silent durchgeführt werden", 1);
        public static State formatState = new State("Formatierung der Festplatte", 2);
        public static State applyState = new State("Apply des Wims", 3);
        public static State bootloaderState = new State("Bootloader wird geschrieben", 4);
        public static State rebootState = new State("Reboot wird eingeleitet", 5);
        private static State[] states = new State[6] { infoState, silentState, formatState, applyState, bootloaderState, rebootState };
        
        private State currentState = infoState;
        private bool silent = false, cFormattedAndAct = false, applyDone = false, bootloaderDone = false;

        public bool isSilent()              { return silent; }
        public bool isCFormattedAndAct()    { return cFormattedAndAct; }
        public bool isApplyDone()           { return applyDone; }
        public bool isBootloaderDone()      { return bootloaderDone; }

        public void setSilent           (bool v) { silent = v; }
        public void setCFormattedAndAct (bool v) { cFormattedAndAct = v; }
        public void setApplyDone        (bool v) { applyDone = v; }
        public void setBootloaderDone   (bool v) { bootloaderDone = v; }

        private static InstallStateMachine iSM;

        // Singleton pattern
        public static InstallStateMachine getISM()
        {
            if (iSM == null)
            {
                iSM = new InstallStateMachine();
            }

            return iSM;
        }

        public void gotoState(State nextState)
        {
            if (possibleNextState(this.currentState, nextState))
            {
                this.currentState = nextState;
                Console.Write(currentState.getName() + "\n");
            }
        }

        public bool canGotoState(State nextState)
        {
            return possibleNextState(this.currentState, nextState);
        }

        public State getCurrentState()
        {
            return this.currentState;
        }


        private bool possibleNextState(State state, State nextState)
        {
            // Silent install
            if (state.getNumber() == 0 && nextState.getNumber() == 1) return true;
            // Not Silent, format desired
            if (state.getNumber() == 0 && nextState.getNumber() == 2) return true;
            // Format -> Apply
            if (state.getNumber() == 2 && nextState.getNumber() == 3) return this.cFormattedAndAct;
            // Cancel, go back
            if (state.getNumber() == 3 && nextState.getNumber() == 2) return true;
            // Apply -> Bootloader
            if (state.getNumber() == 3 && nextState.getNumber() == 4) return this.applyDone;
            // Cancel, go back
            if (state.getNumber() == 4 && nextState.getNumber() == 3) return true;
            // Initiate reboot
            if (state.getNumber() == 4 && nextState.getNumber() == 5) return this.bootloaderDone;
            // Start over any time
            if (nextState.getNumber() == 0) return true;

            return false;
        }
    }
}
