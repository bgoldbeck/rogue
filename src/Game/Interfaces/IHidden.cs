using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Interfaces
{
    public interface IHidden
    {
        bool IsHidden { get; }

        void Reveal();
    }
}
