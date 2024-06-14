using System;
using System.Linq;

namespace Core.UI.MVP
{
    public interface IPresenterFactory
    {
        TPresenter Create<TPresenter, TView>(TView view, params object[] model)
            where TPresenter : ScreenPresenterBase<TView>
            where TView : ViewBase;
    }
    public class PresenterFactory : IPresenterFactory
    {
        public TPresenter Create<TPresenter, TView>(TView view, params object[] model) 
            where TPresenter : ScreenPresenterBase<TView> 
            where TView : ViewBase
        {
            var arguments = new object[] { view }.Concat(model.Select(m=> m)).ToArray();
            return (TPresenter)Activator.CreateInstance(typeof(TPresenter), arguments);
        }
    }
}