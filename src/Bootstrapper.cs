using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RowingManager.ViewModels;

// REFERENCE: (Corey, 2017)

namespace RowingManager {
    public class Bootstrapper : BootstrapperBase {

        public Bootstrapper() {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            DisplayRootViewForAsync<ShellViewModel>();
        }
    }
}
