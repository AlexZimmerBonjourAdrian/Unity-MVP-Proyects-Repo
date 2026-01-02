using RetroFPS.Interact;
using UnityEngine;  

public abstract class CharacterDecorator : BaseCharacter
{
    protected ICharacter _character;
    protected Iinteract _interact;

    public void SetCharacter(ICharacter character)
    {
        _character = character;
    }

    public override void Inicilizate()
    {
        _character?.Inicilizate();
    }

    public override void Oninteract()
    {
         _interact?.Oninteract();
    }
}
