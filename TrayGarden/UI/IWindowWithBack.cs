using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.UI.WindowWithBackStuff;

namespace TrayGarden.UI
{
    public interface IWindowWithBack
    {
        void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings);
        void PrepareAndShow(WindowWithBackVMBase viewModel);
    }
}