using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Smorgasbord
{
    public interface IAskClipboardEvents
    {
        void OnClipboardTextChanged(string newClipboardValue);
    }
}
