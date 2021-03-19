using System;

public abstract class DialogueOptions {

    public abstract void HandleOption(Player a_player, int optionIndex);

    public static DialogueOptions CreateFromDynamic(dynamic button)
    {
        return new Implementation(button);
    }

    public class Implementation : DialogueOptions
    {
        public Implementation(dynamic button)
        {
            PrivateOnClick = button.HandleOption;
        }
        private Action PrivateOnClick { get; set; }

        public override void HandleOption(Player a_player, int option)
        {
            PrivateOnClick();
        }
    }
}