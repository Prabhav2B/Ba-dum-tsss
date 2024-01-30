using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMember : MonoBehaviour
{
    [SerializeField] MaleAudience maleSprites;
    [SerializeField] FemaleAudience femaleSprites;
    Sprite idle;
    Sprite laugh;
    Sprite awkward;
    Sprite dead;
    SpriteRenderer sr;
    Material mat;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        mat = sr.material;
        RandomizeSex();
    }
    void RandomizeSex()
    {
        int randomizeSex = UnityEngine.Random.Range(0, 2);
        if (randomizeSex == 0)        {
            
            idle = femaleSprites.idle;
            awkward = femaleSprites.awkward;
            laugh = femaleSprites.laugh;
            dead = femaleSprites.dead;
        }
        else
        {
            idle = maleSprites.idle;
            awkward = maleSprites.awkward;
            laugh = maleSprites.laugh;
            dead = maleSprites.dead;
        }
    }
    private void Start()
    {
        Idle();
    }
    public void Idle()
    {
        sr.sprite = idle;
        mat.SetTexture("_EmissionMap", idle.texture);
    }
    public void Laugh()
    {
        sr.sprite = laugh;
        mat.SetTexture("_EmissionMap", laugh.texture);
    }

    public void Awkward()
    {
        sr.sprite = awkward;
        mat.SetTexture("_EmissionMap", awkward.texture);
    }
    public void Dead()
    {
        sr.sprite = dead;
        mat.SetTexture("_EmissionMap", dead.texture);
    }
}
