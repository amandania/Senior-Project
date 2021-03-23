using System;
/// <summary>
/// This is an abstract class we use to handle custom dialogue options for any dialogue during runtime.
/// </summary>
public abstract class DialogueOptions {

    public abstract void HandleOption(Player a_player, int a_optionIndex);

    /// <summary>
    /// create a new function to invoke when we call our base HandleOption function
    /// </summary>
    /// <param name="a_button">Custom invoke fuciton to be assigned</param>
    /// <returns></returns>
    public static DialogueOptions CreateFromDynamic(dynamic a_button)
    {
        return new Implementation(a_button);
    }

    public class Implementation : DialogueOptions
    {
        /// <summary>
        /// This class is used to implement the dialogue option and override any default behaviors (default behaviors are normally just a cancel option to close dialogue prompt)
        /// </summary>
        /// <param name="a_button">The function to call when we override</param>
        public Implementation(dynamic a_button)
        {
            PrivateOnClick = a_button.HandleOption;
        }
        private Action PrivateOnClick { get; set; }

        /// <summary>
        /// we define custom functions during run time when we are making dialogues, we also create an option handler that gets set for player.
        /// </summary>
        /// <param name="a_player">Player clicking dialogue option</param>
        /// <param name="a_option">dialouge option index</param>
        public override void HandleOption(Player a_player, int a_option)
        {
            PrivateOnClick();
        }
    }
}