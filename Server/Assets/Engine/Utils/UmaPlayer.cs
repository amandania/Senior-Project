using System;
using UMA;
using UnityEngine;

public class UmaPlayer : MonoBehaviour
{
    public string race;
    public string gender;
    public string umaData;


    //public RuntimeAnimatorController animController;
    private UMADynamicAvatar _umaDynamicAvatar;
    public UMAData _umaData { get; set; }
    private UMADnaHumanoid _umaDNA;
    private UMADnaTutorial _umaTutorialDNA;
    private UMAContext umaContext;
    private UMAGenerator umaGenerator;
    public int _numerOfSlots = 40;
    public RuntimeAnimatorController animController;
    public int speed;

    public void StartPlayer(Guid index)
    {
        animController = Resources.Load("PlayerAnimator") as RuntimeAnimatorController;
        umaGenerator = GameObject.Find("UMAGenerator").GetComponent<UMAGenerator>();
        umaContext = GameObject.Find("UMAContext").GetComponent<UMAContext>();
        UmaConstants tmp = new UmaConstants();
        MakePlayer(index, tmp.Decompress(umaData));
    }

    public void MakePlayer(Guid index, byte[] data)
    {
        MakeUma(index, data);
    }
    public string GetUMAData()
    {
        byte[] playerData;
        UMATextRecipe asset = ScriptableObject.CreateInstance<UMATextRecipe>();
        asset.Save(_umaDynamicAvatar.umaData.umaRecipe, umaContext);
        playerData = asset.GetBytes();
        return UmaConstants.instance.Compress(playerData);
    }

    private void MakeUma(Guid index, byte[] data)
    {
        GameObject GO = GameObject.Find("Player: " + index + "");
        GO.transform.parent = this.transform;
        _umaDynamicAvatar = GO.AddComponent<UMADynamicAvatar>();
        _umaDynamicAvatar.Initialize();
        _umaData = _umaDynamicAvatar.umaData;
        _umaDynamicAvatar.umaGenerator = umaGenerator;
        _umaData.umaGenerator = umaGenerator;
        _umaDynamicAvatar.animationController = animController;
        _umaData.umaRecipe.slotDataList = new SlotData[_numerOfSlots];  
        _umaDNA = new UMADnaHumanoid();
        _umaTutorialDNA = new UMADnaTutorial();
        _umaData.umaRecipe.AddDna(_umaDNA);
        _umaData.umaRecipe.AddDna(_umaTutorialDNA);
        UMATextRecipe recipe = ScriptableObject.CreateInstance<UMATextRecipe>();
        recipe.SetBytes(data);
        _umaDynamicAvatar.Load(recipe);
        Destroy(recipe);
        _umaDynamicAvatar.UpdateNewRace();

    }
}
