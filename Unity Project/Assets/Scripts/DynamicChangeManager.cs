using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicChangeManager : MonoBehaviour
{
    public GazeManager gm;

    public EnemyManager em;

    public DifficultyManager dm;
    
    public MazeManager mm;

    //gazeManager
    public void gmReset(RaycastHit hit)
    {
        gm.reset(hit);
    }

    public void gmHide(RaycastHit hit)
    {
        gm.hide(hit);
    }

    public void gmFadeIn(RaycastHit hit)
    {
        gm.fadeIn(hit);
    }

    //EnemyManager

    public void emSprint()
    {
        em.sprint();
    }

    public void emResetSpeed()
    {
        em.resetSpeed();
    }

    public void emFreeze()
    {
        em.freeze();
    }

    //chunkManager

    //difficultyManager

    public void dmResetTimer()
    {
        dm.resetTimer();
    }
    public void dmEnterNewChunk()
    {
        dm.enteredNewChunk();
    }

    public int dmGetDiff()
    {
        return dm.getDiff();
    }
}
