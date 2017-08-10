﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarElement : MonoBehaviour
{
    public Spell glyph;
    SpellcastingGestureRecognition spellcast;
    SpellCooldowns cooldown;
    float size;
    float sizeCap = .9f;

    // Use this for initialization
    void Start ()
    {
        spellcast = Camera.main.GetComponentInParent<SpellcastingGestureRecognition>();
        cooldown = Camera.main.GetComponentInParent<SpellCooldowns>();
        transform.localScale = new Vector3(-sizeCap * 1.4f, sizeCap, sizeCap);
        transform.GetChild(0).localScale = new Vector3(1, 0, 1);

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(spellcast.isCoolingDown)
        {
            if (spellcast.playerStatus.photonView.isMine)
                UpdateGlyph();
        }
        	
	}

    void UpdateGlyph()
    {
        if (glyph == Spell.blessing)
        {
            if (spellcast.blessingCD > 0)
            {
                size = (cooldown.blessingCD - spellcast.blessingCD) / cooldown.blessingCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.fire)
        {
            if (spellcast.fireCD > 0)
            {
                size = (cooldown.fireCD - spellcast.fireCD) / cooldown.fireCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.flip)
        {
            if (spellcast.flipCD > 0)
            {
                size = (cooldown.flipCD - spellcast.flipCD) / cooldown.flipCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.heal)
        {
            if (spellcast.healCD > 0)
            {
                size = (cooldown.healCD - spellcast.healCD) / cooldown.healCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.ice)
        {
            if (spellcast.iceCD > 0)
            {
                size = (cooldown.iceCD - spellcast.iceCD) / cooldown.iceCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.meteor)
        {
            if (spellcast.meteorCD > 0)
            {
                size = (cooldown.meteorCD - spellcast.meteorCD) / cooldown.meteorCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.pong)
        {
            if (spellcast.pongCD > 0)
            {
                size = (cooldown.pongCD - spellcast.pongCD) / cooldown.pongCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.shield)
        {
            if (spellcast.shieldCD > 0)
            {
                size = (cooldown.shieldCD - spellcast.shieldCD) / cooldown.shieldCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.sword)
        {
            if (spellcast.swordCD > 0)
            {
                size = (cooldown.swordCD - spellcast.swordCD) / cooldown.swordCD;
            }
            else
                size = 1;
        }

        if (glyph == Spell.vines)
        {
            if (spellcast.vinesCD > 0)
            {
                size = (cooldown.vinesCD - spellcast.vinesCD) / cooldown.vinesCD;
            }
            else
                size = 1;
        }


        transform.GetChild(0).localScale = new Vector3(1, 1-size, 1);
    }
}
