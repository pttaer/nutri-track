using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTFoodDTO : MonoBehaviour
{
    public string Id {  get; set; }
    public string Image {  get; set; }
    public string Name {  get; set; }
    public string Description {  get; set; }
    public string Source {  get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<NutrientsDTO> Nutrients { get; set; }

    public class NutrientDTO
    {
        public string Id { get; set; }
        public string Image { get; set; }
    }

    public class NutrientsDTO
    {
        public NutrientDTO Nutrient { get; set; }
        public float Amount { get; set; }
    }
}
