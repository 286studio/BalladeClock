using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class _AlarmListUserData
{
    // user data to save
    // basic types (int, float, double, [], etc) only, no classes

    // AlarmList related
    public int nid;
    public int numAlarms;
    public int[] id;
    public int[] profile_idx;
    public int[] hr_dp_val;
    public int[] min_dp_val;
    public int[] ampm_dp_val;
    public int[] repeat_dp_val;
    public bool[] snooze_tg_val;
    public string[] label_if_val;
}
