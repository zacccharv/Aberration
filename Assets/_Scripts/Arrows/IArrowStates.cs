using System.Collections.Generic;
using DG.Tweening;

public interface IArrowStates
{
    public Arrow Arrow { get; set; }
    public List<Tween> Tweens { get; set; }
    public bool PerfectInputStart { get; set; }

    public void UpdateBounds();
    public void SuccessState(ScoreType scoreType, InteractionType interactionType);
    public void FailState();
}
