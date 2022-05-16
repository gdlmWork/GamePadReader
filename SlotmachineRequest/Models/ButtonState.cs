using Windows.Gaming.Input;

namespace SlotmachineRequest.Models
{
    public class ButtonState
    {
        public ulong Timestamp { get; set; }
        public bool[] Buttons { get; set; }
        public GameControllerSwitchPosition[] switches { get; set; }
        public double[] Axis { get; set; }

    }
}
