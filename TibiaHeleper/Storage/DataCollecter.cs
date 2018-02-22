using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Modules;

namespace TibiaHeleper.Storage
{
    [Serializable]
    class DataCollecter
    {
        public Healer healer { get; set; }
        public AutoHaste autoHaste { get; set; }
        public Sio sio { get; set; }

        public DataCollecter()
        {
            healer = ModulesManager.healer;
            autoHaste = ModulesManager.autoHaste;
            sio = ModulesManager.sio;
        }

        public void activateLoadedSettings()
        {
            ModulesManager.SioDisable();
            ModulesManager.sio = sio;
            if (sio.working) ModulesManager.SioEnable();
        

            ModulesManager.HealerDisable();
            ModulesManager.healer = healer;
            if (healer.working) ModulesManager.HealerEnable();

            ModulesManager.AutoHasteDisable();
            ModulesManager.autoHaste = autoHaste;
            if (autoHaste.working) ModulesManager.AutoHasteEnable();
        }
    }
}
