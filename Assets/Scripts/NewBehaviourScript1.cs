using UnityEngine;
using System.Collections;

interface ITeamOwned
{
    bool blue { get; set; }

    void SetBlue(bool blue_);
    bool GetBlue();
}

