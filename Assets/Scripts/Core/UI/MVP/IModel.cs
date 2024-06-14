using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.UI.MVP
{
    public interface IModel
    {
    }
    
    public interface IModelKeeper
    {
        TModel GetModel<TModel>() where TModel : IModel;
    }
    
    public class ModelKeeper : IModelKeeper
    {
        private Dictionary<Type, IModel> _models = new();
        private readonly IModel[] _modelsX;
        public ModelKeeper(params IModel[] models)
        {
            if(models == null) return;
            foreach (var model in models)
            {
                _models.Add(model.GetType(), model);
            }

            _modelsX = models;
        }

        public Dictionary<Type, IModel> _model { get; }

        public TModel GetModel<TModel>() where TModel : IModel
        {
            var t = _modelsX.OfType<TModel>();
            if (!_models.TryGetValue(typeof(TModel), out var model))
                throw new Exception($"Model {typeof(TModel)} not found");
            return (TModel)model;
        }
    }
}