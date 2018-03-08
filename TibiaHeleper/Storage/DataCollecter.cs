using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Modules;
using TibiaHeleper.Modules.Targeting;

namespace TibiaHeleper.Storage
{
    [Serializable]
    class DataCollecter
    {
        public Healer healer { get; set; }
        public AutoHaste autoHaste { get; set; }
        public Sio sio { get; set; }
        public AntyPalalyse antyParalyse {get; set;}
        public Targeter targeting { get; set; }


        public DataCollecter()
        {
            healer = ModulesManager.healer;
            autoHaste = ModulesManager.autoHaste;
            sio = ModulesManager.sio;
            antyParalyse = ModulesManager.antyParalyse;
            targeting = ModulesManager.targeting;
        }

        public void activateLoadedSettings()
        {
            ModulesManager.SioDisable();
            ModulesManager.sio = sio;
            if (sio.working) ModulesManager.HardEnableThread((Module)sio);
        

            ModulesManager.HealerDisable();
            ModulesManager.healer = healer;
            if (healer.working) ModulesManager.HardEnableThread((Module)healer);

            ModulesManager.AutoHasteDisable();
            ModulesManager.autoHaste = autoHaste;
            if (autoHaste.working) ModulesManager.HardEnableThread((Module)autoHaste);

            ModulesManager.AntyParalyseDisable();
            ModulesManager.antyParalyse = antyParalyse;
            if (antyParalyse.working) ModulesManager.HardEnableThread((Module)antyParalyse);

            ModulesManager.TargetingDisable();
            ModulesManager.targeting = targeting;
            if (targeting.working)  ModulesManager.HardEnableThread((Module)targeting);

        }
    }
}
