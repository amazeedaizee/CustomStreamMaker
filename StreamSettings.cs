using NGO;
using ngov3;
using System.Collections.Generic;

namespace CustomStreamMaker
{

    public enum StreamBackground
    {
        Default, Silver, Gold, MileOne, MileTwo, MileThree, MileFour, MileFive, Guru, Horror, BigHouse, Roof, Black, Void, None = 1000
    }
    public enum StreamChatSettings
    {
        Normal,
        Celebration,
        Uncontrollable
    }
    public enum ChatCommentType
    {
        Normal, Stressful, Super
    }
    public class StreamSettings
    {
        public string StringTitle;
        public StreamChatSettings ChatSettings;
        public string StartingAnimation;
        public CustomAsset CustomStartingAnimation;
        public StreamBackground StartingBackground;
        public CustomAsset CustomBackground;
        public SoundType StartingMusic;
        public EffectType StartingEffect;
        public float EffectIntensity;

        public string ReactionAnimation;

        public bool IsIntroPlaying = true;
        public bool IsDarkAngelPlaying;
        public bool HasCustomFollowerCount;
        public int CustomFollowerCount = 0;
        public bool HasCustomDay;
        public int CustomDay = 15;

        public bool HasCustomEndScreen;
        public string CustomEndScreenPath;

        public bool HasChair = true;

        public bool IsInvertedColors;
        public bool isBordersOff;

        public bool hasEndScreen = true;

        public List<PlayingObject> PlayingList = new();

        public StreamSettings() { }
    }
}
