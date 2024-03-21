using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NTTUserDTO;

public class NTTRegisterDTO
{
    public UserDTO User { get; set; }
    public NTTBMIRecordPostDTO BmiRecord { get; set; }
    public List<int> MedicalConditionIds { get; set; }
    public List<int> DietRestrictionIds { get; set; }

    public NTTRegisterDTO() { }
}
