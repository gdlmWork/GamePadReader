using SlotmachineRequest.Models;

namespace SlotmachineRequest.Services
{
    public interface IButtonStateservice
    {
        ButtonState GetState();
        void ChangeState(ButtonState buttonState);
    }
}
