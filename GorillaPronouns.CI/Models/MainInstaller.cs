using ComputerInterface.Interfaces;
using Zenject;

namespace GorillaPronouns.ComputerInterface.Models
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<PronounEntry>().AsSingle();
        }
    }
}
