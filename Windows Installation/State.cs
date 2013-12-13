using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Windows_Installation
{
    public class State
    {
        private string name;
        private int number;

        public State(string name, int number)
        {
            this.name = name;
            this.number = number;
        }

        public string getName()
        {
            return this.name;
        }

        public int getNumber()
        {
            return this.number;
        }
    }
}
