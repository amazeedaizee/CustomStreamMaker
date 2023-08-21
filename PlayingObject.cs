using NGO;
using ngov3;
using System;
using System.Collections.Generic;

namespace CustomStreamMaker
{
    public enum PlayingType
    {
        KAngelSays,
        KAngelCallout,
        ChatSays,
        ChatSuper,
        ChatBad,
        PlaySE,
        PlayBGM,
        PlayEffect,
        ChatFirst,
        ChatMiddle,
        ChatLast,
        ChatRainbow,
        ChatDelete,
        ChatDeleteAll,
        ReadSuperChats
    }

    public enum BorderEffectType
    {
        EaseIn,
        EaseBeforePlay,
        Play,
        EaseOut
    }

    [Serializable]
    public class PlayingObject
    {
        public virtual PlayingType PlayingType { get; set; }

        public static bool IsObjTheSame(PlayingObject obj1, PlayingObject obj2)
        {
            if (obj1.PlayingType != obj2.PlayingType)
                return false;
            switch (obj1.PlayingType)
            {
                case PlayingType.KAngelSays:
                case PlayingType.KAngelCallout:
                    var kOriginal = obj1 as KAngelSays;
                    var kDupe = obj2 as KAngelSays;
                    if (kOriginal.AnimName != kDupe.AnimName)
                        return false;
                    if (kOriginal.Dialogue != kDupe.Dialogue)
                        return false;
                    if (kOriginal.customAnim == null && kDupe.customAnim != null)
                        return false;
                    if (kOriginal.customAnim != null && kDupe.customAnim == null)
                        return false;
                    if (!CustomAsset.IsCustomAssetTheSame(kOriginal.customAnim, kDupe.customAnim))
                        return false;
                    if (kDupe.PlayingType == PlayingType.KAngelCallout)
                    {
                        if ((kDupe as KAngelCallout).HaterComment != (kOriginal as KAngelCallout).HaterComment)
                            return false;
                    }
                    break;
                case PlayingType.ChatSays:
                case PlayingType.ChatSuper:
                case PlayingType.ChatBad:
                    var chatOriginal = obj1 as ChatSays;
                    var chatDupe = obj2 as ChatSays;
                    if (chatDupe.Comment != chatOriginal.Comment)
                        return false;
                    if (chatDupe.PlayingType == PlayingType.ChatSuper)
                    {
                        if (chatOriginal.Replies.Count != chatDupe.Replies.Count)
                            return false;
                        for (int i = 0; i < chatOriginal.Replies.Count; i++)
                        {
                            if (chatOriginal.Replies[i].AnimName != chatDupe.Replies[i].AnimName) return false;
                            if (chatOriginal.Replies[i].Dialogue != chatDupe.Replies[i].Dialogue) return false;
                        }
                    }
                    break;
                case PlayingType.PlaySE:
                case PlayingType.PlayBGM:
                    var audiOriginal = obj1 as PlaySound;
                    var auDupe = obj2 as PlaySound;
                    if (audiOriginal.Audio != auDupe.Audio) return false;
                    break;
                case PlayingType.PlayEffect:
                    var effectOriginal = obj1 as PlayEffect;
                    var effectDupe = obj2 as PlayEffect;
                    if (effectOriginal.BorderEffect != effectDupe.BorderEffect)
                        return false;
                    if (effectDupe.BorderEffectType != effectOriginal.BorderEffectType)
                        return false;
                    break;
                default:
                    break;
            }
            return true;
        }

        public static PlayingObject DupePlayingObj(PlayingObject obj)
        {
            PlayingObject dupeObj;
            switch (obj.PlayingType)
            {
                case PlayingType.KAngelSays:
                case PlayingType.KAngelCallout:
                    var kOriginal = obj as KAngelSays;
                    var kDupe = new KAngelSays();
                    kDupe.PlayingType = kOriginal.PlayingType;
                    kDupe.AnimName = kOriginal.AnimName;
                    kDupe.Dialogue = kOriginal.Dialogue;
                    kDupe.SetCustomAnim(kOriginal.customAnim);
                    if (kDupe.PlayingType == PlayingType.KAngelCallout)
                    {
                        (kDupe as KAngelCallout).HaterComment = (kOriginal as KAngelCallout).HaterComment;
                    }
                    dupeObj = kDupe;
                    break;
                case PlayingType.ChatSays:
                case PlayingType.ChatSuper:
                case PlayingType.ChatBad:
                    var chatOriginal = obj as ChatSays;
                    var chatDupe = new ChatSays();
                    chatDupe.PlayingType = chatOriginal.PlayingType;
                    chatDupe.Comment = chatOriginal.Comment;
                    if (chatDupe.PlayingType == PlayingType.ChatSuper)
                    {
                        List<KAngelSays> dupeList = new();
                        for (int i = 0; i < chatOriginal.Replies.Count; i++)
                        {
                            var kReplyRef = chatOriginal.Replies[i];
                            dupeList.Add(new(kReplyRef.AnimName, kReplyRef.Dialogue, kReplyRef.customAnim));
                        }
                        chatDupe.Replies = dupeList;
                    }
                    dupeObj = chatDupe;
                    break;
                case PlayingType.PlaySE:
                case PlayingType.PlayBGM:
                    var audiOriginal = obj as PlaySound;
                    var auDupe = new PlaySound();
                    auDupe.ChangeSound(audiOriginal.Audio);
                    dupeObj = auDupe;
                    break;
                case PlayingType.PlayEffect:
                    var effectOriginal = obj as PlayEffect;
                    var effectDupe = new PlayEffect();
                    effectDupe.ChangeEffectType(effectOriginal.BorderEffect, effectOriginal.BorderEffectType);
                    dupeObj = effectDupe;
                    break;
                default:
                    var genDupe = new ChatGeneral();
                    genDupe.ChangePlayingType(obj.PlayingType);
                    dupeObj = genDupe;
                    break;
            }
            return dupeObj;
        }
    }

    [Serializable]
    public class KAngelSays : PlayingObject
    {
        public override PlayingType PlayingType { get => PlayingType.KAngelSays; }

        public bool IsCustomAnim;
        public CustomAsset customAnim;

        public string AnimName;
        public string Dialogue;

        public KAngelSays() { }

        public KAngelSays(string animName, string dialogue)
        {
            AnimName = animName;
            Dialogue = dialogue;
        }
        public KAngelSays(string animName, string dialogue, CustomAsset customAnimFile)
        {
            SetCustomAnim(customAnimFile);
            AnimName = animName;
            Dialogue = dialogue;
        }

        public void SetCustomAnim(CustomAsset customAnimFile)
        {
            if (customAnimFile != null)
            {
                customAnim = new(CustomAssetType.Sprite, customAnimFile.customAssetFileType, customAnimFile.fileName, customAnimFile.filePath);
                customAnim.picWidth = customAnimFile.picWidth;
                customAnim.picHeight = customAnimFile.picHeight;
                customAnim.catalogPath = customAnimFile.catalogPath;
                return;
            }
            customAnim = null;
        }
    }

    [Serializable]
    public class KAngelCallout : KAngelSays
    {
        public override PlayingType PlayingType { get => PlayingType.KAngelCallout; }

        public string HaterComment;

        public KAngelCallout() { }
        public KAngelCallout(string hateComment)
        {
            HaterComment = hateComment;
        }

        public KAngelCallout(KAngelSays angelSays, string hateComment)
        {
            HaterComment = hateComment;
            IsCustomAnim = angelSays.IsCustomAnim;
            SetCustomAnim(angelSays.customAnim);

            AnimName = angelSays.AnimName;
            Dialogue = angelSays.Dialogue;
        }

        public void AddHateCallout(string hateComment)
        {
            HaterComment = hateComment;
        }
    }

    [Serializable]
    public class ChatSays : PlayingObject
    {
        public string Comment;

        public List<KAngelSays> Replies;

        public ChatSays() { }

        public ChatSays(string comment)
        {
            Comment = comment;
            SetNormalComment();
        }

        public ChatSays(string comment, bool isBad = true)
        {
            Comment = comment;
            SetBadComment();
        }
        public ChatSays(string comment, List<KAngelSays> replies)
        {
            Comment = comment;
            SetSuperChat(replies);
        }
        public void SetBadComment()
        {
            PlayingType = PlayingType.ChatBad;
            Replies = null;
        }

        public void SetSuperChat(List<KAngelSays> replies)
        {
            PlayingType = PlayingType.ChatSuper;
            Replies = replies;
        }

        public void SetNormalComment()
        {
            PlayingType = PlayingType.ChatSays;
            Replies = null;
        }
    }

    [Serializable]
    public class PlaySound : PlayingObject
    {
        public SoundType Audio;

        public PlaySound() { }
        public PlaySound(SoundType sound)
        {
            ChangeSound(sound);
        }

        public void ChangeSound(SoundType sound)
        {
            Audio = sound;
            PlayingType = sound.ToString().Contains("BGM_") ? PlayingType.PlayBGM : PlayingType.PlaySE;
        }
    }

    [Serializable]
    public class PlayEffect : PlayingObject
    {
        public override PlayingType PlayingType { get => PlayingType.PlayEffect; }
        public ChanceEffectType BorderEffect;
        public BorderEffectType BorderEffectType;

        public PlayEffect() { }
        public PlayEffect(ChanceEffectType borderEffect, BorderEffectType borderEffectType)
        {
            ChangeEffectType(borderEffect, borderEffectType);
        }

        public void ChangeEffectType(ChanceEffectType borderEffect, BorderEffectType borderEffectType)
        {
            BorderEffect = borderEffect;
            BorderEffectType = borderEffectType;
        }
    }

    [Serializable]
    public class ChatGeneral : PlayingObject
    {

        public ChatGeneral() { }
        public ChatGeneral(PlayingType playingType)
        {
            ChangePlayingType(playingType);
        }
        public void ChangePlayingType(PlayingType playingType)
        {
            if ((int)playingType < 8)
                throw new ArgumentOutOfRangeException(nameof(playingType) + " - This PlayingType is not supported for this class.");
            PlayingType = playingType;
        }
    }
}
