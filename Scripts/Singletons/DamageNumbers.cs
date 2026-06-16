using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class DamageNumbers : Node
{
    private static DamageNumbers Mambo; // don't ask
    private static Font PoppinsBold;
    public enum NumberType
    {
        BALL_DAMAGE,
        PLAYER_DAMAGE,
        MONEY_GAIN,
        WAVE_END_MONEY
    }
    private static LabelSettings ballDamageNumber = new LabelSettings();
    private static LabelSettings playerDamageNumber = new LabelSettings();
    private static LabelSettings moneyGainNumber = new LabelSettings();
    private static LabelSettings waveEndMoneyNumber = new LabelSettings();

    public override void _Ready()
    {
        Mambo = this;
        PoppinsBold = GD.Load<Font>("res://Fonts/Poppins-Bold.ttf");

        ballDamageNumber.Font = playerDamageNumber.Font = moneyGainNumber.Font = waveEndMoneyNumber.Font = PoppinsBold;
        ballDamageNumber.FontSize = moneyGainNumber.FontSize = 32;
        playerDamageNumber.FontSize = 38;
        waveEndMoneyNumber.FontSize = 50;
        ballDamageNumber.OutlineSize = moneyGainNumber.OutlineSize = 7;
        playerDamageNumber.OutlineSize = 9;
        waveEndMoneyNumber.OutlineSize = 11;
        ballDamageNumber.OutlineColor = playerDamageNumber.OutlineColor = moneyGainNumber.OutlineColor = waveEndMoneyNumber.OutlineColor = Color.FromHtml("#000000");
        ballDamageNumber.FontColor = Color.FromHtml("#ff9090");
        playerDamageNumber.FontColor = Color.FromHtml("#990000");
        moneyGainNumber.FontColor = waveEndMoneyNumber.FontColor = Color.FromHtml("#03d125");
    }

    public static async Task DisplayFloatingNumber(float value, Vector2 position, NumberType type)
    {
        Label number = new Label();
        number.GlobalPosition = position;
        number.Text = GD.VarToStr(value);
        number.ZIndex = 5;

        switch (type)
        {
            case NumberType.BALL_DAMAGE:
                number.LabelSettings = ballDamageNumber;
                break;
            
            case NumberType.PLAYER_DAMAGE:
                number.LabelSettings = playerDamageNumber;
                break;

            case NumberType.MONEY_GAIN:
                number.Text = "+" + GD.VarToStr(value);
                number.LabelSettings = moneyGainNumber;
                break;
            
            case NumberType.WAVE_END_MONEY:
                number.Text = "+" + GD.VarToStr(value);
                number.LabelSettings = waveEndMoneyNumber;
                break;
        }
        

        Mambo.AddChild(number);

        Tween tween = Mambo.GetTree().CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(number, "position:y", position.Y - GD.RandRange(150 , 185), 0.75).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(number, "position:x", position.X - GD.RandRange(-55 , 55), 0.75).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(number, "scale", Vector2.Zero, 0.25).SetEase(Tween.EaseType.In).SetDelay(0.5);

        await Mambo.ToSignal(tween, "finished");
        tween = null; // im scared of a memory leak
        number.QueueFree();
    }

}
