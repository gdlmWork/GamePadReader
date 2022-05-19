using SlotmachineRequest.Models;

namespace SlotmachineRequest.Services
{
    public class ButtonStateService : IButtonStateservice
    {
        public ButtonState buttonState = new ButtonState { ButtonReleased = false };

        public void ChangeState(ButtonState buttonState)
        {
            this.buttonState = buttonState;
        }

        public ButtonState GetState()
        {
            return buttonState;
        }
    }
}
