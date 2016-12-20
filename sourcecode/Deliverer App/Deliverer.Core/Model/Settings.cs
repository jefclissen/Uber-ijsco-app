using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deliverer.Core.Model
{
    public class Settings
    {
        public bool nieuweKlantenNotification
        {
            get { return this.nieuweKlantenNotification; }
            set { this.nieuweKlantenNotification = false; }
        }
    }
}
