using System;
using Core.UI.MVP;
using SavingData;
using UnityEngine;

public interface IScoreModelSetter : IModel
{
    CustomValue<int> LevelRound { get; set; }
    CustomValue<int> ScoreRound { get; set; }
    CustomValue<int> TotalScoreRound { get; set; }
    CustomValue<int> TotalScore { get; set; }
    CustomValue<int> BestLevel { get; set; }
    CustomValue<int> BestScore { get; set; }
}

public interface IScoreModelGetter : IModel
{
    CustomValue<int> LevelRound { get; }
    CustomValue<int> ScoreRound { get; }
    CustomValue<int> TotalScoreRound { get; }
    CustomValue<int> TotalScore { get; }
    CustomValue<int> BestLevel { get; }
    CustomValue<int> BestScore { get; }
}

public class ScoreModel : IScoreModelSetter, IScoreModelGetter
{
    public CustomValue<int> LevelRound { get; set; } = new();
    public CustomValue<int> ScoreRound { get; set; } = new();
    public CustomValue<int> TotalScoreRound { get; set; } = new();
    public CustomValue<int> TotalScore { get; set; } = new (SavingKeys.TotalScoreKey);
    public CustomValue<int> BestLevel { get; set; } = new (SavingKeys.BestLevelKey);
    public CustomValue<int> BestScore { get; set; } = new (SavingKeys.BestScoreKey);
}

public class CurrencyManager
{
    private readonly Settings _settings;
    private readonly ScoreModel _scoreModel;
    private readonly float _additionalPercentForRound;
    private readonly float _additionalPercentForStackLevel;

    public CurrencyManager(Settings settings)
    {
        _settings = settings;
        _scoreModel = new ScoreModel();
        
        _additionalPercentForRound = _settings.AdditionalPercentForRound / 100f;
        _additionalPercentForStackLevel = 1 + _settings.AdditionalPercentForStack / 100f;
    }
    
    public IScoreModelGetter ScoreModelGetter => _scoreModel;
    public IScoreModelSetter ScoreModelSetter => _scoreModel;
    
    public int CalculateScoreRound(int area)
    {
        return Mathf.RoundToInt(_scoreModel.LevelRound * _additionalPercentForStackLevel * area);
    }
    
    public void CalculateAndApplyResultScore()
    {
        _scoreModel.TotalScoreRound.Value = Mathf.RoundToInt(_scoreModel.ScoreRound * RewardForRound);
        _scoreModel.TotalScore.Value += _scoreModel.TotalScoreRound.Value;
    }

    private float RewardForRound => 1f + (_scoreModel.LevelRound % _settings.NumberOfStackLevelsToReward) * _additionalPercentForRound;
    
    [Serializable]
    public struct Settings
    {
        [Range(0, 100)] public int AdditionalPercentForStack;
        [Range(0, 100)] public int NumberOfStackLevelsToReward;
        [Range(0, 100)] public int AdditionalPercentForRound;
    }
}
    
public class CustomValue<T> where T : struct
{
    private T _value;
    private readonly string _saveKey;

    public CustomValue()
    {
        _saveKey = null;
        _value = default;
    }
    public CustomValue(string saveKey)
    {
        _saveKey = saveKey;
        _value = Saving.GetValue<T>(_saveKey);
    }
    
    public event Action OnValueChangedEvent;
    
    public static implicit operator T(CustomValue<T> value) => value._value;
    // public static implicit operator CustomValue<T>(T value) => new() { _value = value };
    
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChangedEvent?.Invoke();
            if(_saveKey != null) Saving.SetValue<T>(_saveKey, _value);
        }
    }
}
    