using System;
using ComputerInterface.Interfaces;

namespace GorillaPronouns.ComputerInterface.Models
{
    internal class PronounEntry : IComputerModEntry
    {
        public string EntryName => "Pronouns";
        public Type EntryViewType => typeof(PronounView);
    }
}
