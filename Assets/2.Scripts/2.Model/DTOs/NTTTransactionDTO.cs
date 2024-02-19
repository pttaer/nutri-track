using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTTransactionDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string UserId {  get; set; }
    public string WalletId {  get; set; }
    public int Amount {  get; set; }
    public string Type {  get; set; }
    public string Status {  get; set; }
    public string Method {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
