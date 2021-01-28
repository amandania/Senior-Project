using Engine.Interfaces;
using Engine.Net;
using Server.Game.model.combat;
using UnityEngine;

public class Player : Character
{
    public PlayerSession _Session { get; set; }
    public readonly IWorld _world;
    public CombatManager CombatManager { get; }
    public GameObject PlayerGameObject { get; set; }
    public MovementControllerComponenent ControllerComponent;

    public string _race { get; set; }
    public string _gender { get; set; }
    public string _umaDataString { get; set; }
    public string _username { get; set; }
    public string _password { get; set; }

    public Player(PlayerSession session, IWorld world)
    {
        _Position = new Position(250.4187f, 50.89893f, -648.4915f);
        _SpeedMagnitude = 0f;
        _OldPosition = _Position;
        _Session = session;
        _world = world;
        _race = "Human";
        _gender = "Male";
        _username = "";
        _umaDataString = "sQ0AAOy9B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/In7xR5d53RTV8qNHe6OPVtn0bT57XVbt06zNnhdN+9Gj5bosRx819Fnzk3sfPfoe/+3++cUfFbOPHn30RVbmz7Jp/hE1ndLvHz3a3dkZfTStVtdns3cfPdreHX1UUVdldt0QEO+tb+fZbGf3I7Qtq5ob04t1PqW+v7cz4v++/0v8fr6dFXXnjd3Nb5xe55O6uuq8tGdfunewP9q7/yn//9P97/+S3tvN+42L3vhSPgw6vDeMpXvjePbTa6K7/96+Q3RvRP/7dD+C5dlymddfVOt2/n644t2vlrO8vsqzLlnvewjv33842t3ZY0J1+35T1U31/t0+qWbXnR4/3TyRr+dF3XZeeeC9cn93T5CkX7pIfjtbzt5zHgeQPOgjGXb1PL/4Znp6uJkcQ/O260tQOHH+69/Js2XTfTUiSuFbz/K8HRrcA39s0A+/xFMV/y/65/s6ZMVy9NH5if5JE7LMFrn53LT63t79+0TAXf6/Eob+k0/d/2/4nAkZBb/76Uj+9/8x2O8J6P07ONgdfbqH//1/EXlp+LPGND/7tPlZQ/2HAv7WgN6/g2+C9KSEmnlW5zPWPSfVetmK7wM/6tFH314vsiUULilb+vXpMvvo0S/+JcZJ4z9JWc2W2ZvrFdp/9cUxfchvVaSsg4Yf/eLf96N5XlzM29+XVPbewQh/ZrPXxQ/y4IPvFrN2bj9Z5tO3b+bF9O0ybxr7aVYvnufLC6/deVXn/U/pkxCcNgs/nMMqB3ick30JPijzi9f5KquzlvxU++l6tSJ/Z91MS69lddX7jNt9Nxw7t+t+RjY76HaSl+W1/esqI2/Y/nVRrtt8HbansfU/eFk1RYA2PnxVteFYllWTB6/ig5N1fRl+0pkd+uRsOS2LZR9Yr1v+sK6WxGTTfBZ8/KzM2jZ3TafzYhkggw8i7/LH3X6I+WbFpAxH89PZVUgZfNB7dTrP87edjumTXjuavBP+oo+S/ar7Dlivx/Dmw34PxSrEdwG3Opzb67w3i/RZyEDE7o3Hyr/kI6iTrsC+WbdVXWRlX2ABjz4qlhf2/e//kv8nAAD//w==";
        //_world.Players.Add(this);
    }

    public void Process()
    {

    }
   
   
}
