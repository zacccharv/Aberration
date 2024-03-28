using System.Collections.Generic;
using DG.Tweening;

interface IArrowStates
{
    public static event DirectionPress CurrentDirectionSet, TowerColorChange;

    public event KillTweens KillAllTweens;

    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; }

    public void Success(ScoreType scoreType);
    public void Fail();
}
