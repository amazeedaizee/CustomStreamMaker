using AssetsTools.NET.Extra;
using AssetsTools.NET.Texture;
using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using NAudio.Vorbis;
using NAudio.Wave;
using NGO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomStreamMaker
{
    /* 
     *   Copyright (c) 2023 amazeedaizee
     *   
     *   Permission is hereby granted, free of charge, to any person obtaining a copy of this software (AssetExtractor.cs) and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
     *   
     *   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
     *   
     *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
     *
     *   ---------
     *
     *   Parts of the code in this file uses: 
     *   
     *   the AssetTools.NET Library, 
     *   the AssetRipper.TextureDecoder Library,
     *   the Fmod5Sharp Library, 
     *   the NAudio Library, 
     *   the NVorbis Library,
     *   the OggVorbisEncoder Library,
     *   and the ImageSharp library (Apache 2.0 version since this software is visible-source).
     *
     *    Licenses:
     *    
     *    https://github.com/nesrak1/AssetsTools.NET/blob/master/LICENSE
     *    https://github.com/AssetRipper/TextureDecoder/blob/master/LICENSE
     *    https://github.com/SamboyCoding/Fmod5Sharp/blob/master/LICENSE
     *    https://github.com/naudio/NAudio/blob/master/license.txt
     *    https://github.com/SteveLillis/.NET-Ogg-Vorbis-Encoder/blob/master/LICENSE
     *    https://github.com/SixLabors/ImageSharp/blob/main/LICENSE
    */

    internal class AssetExtractor
    {
        public static bool SaveToMemory = false;

        public static string BackgroundDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker\CachedBackgrounds";
        public static string SpriteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker\CachedSprites";
        public static string AudioDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CustomStreamMaker\CachedAudio";

        public static Dictionary<string, SixLabors.ImageSharp.Image> CachedBackgrounds = new();
        public static Dictionary<string, SixLabors.ImageSharp.Image> CachedSprites = new();
        public static Dictionary<string, (byte[], int)> CachedMusic = new();

        public static string GetAddressablesPath()
        {
            var gamePath = GameLocation.InitializeValidGamePath();
            if (!Directory.Exists(gamePath + @"\Windose_Data\StreamingAssets\aa\StandaloneWindows64"))
                return null;
            if (string.IsNullOrEmpty(gamePath))
                return null;
            return gamePath + @"\Windose_Data\StreamingAssets\aa\StandaloneWindows64";
        }

        public static string GetGameDataPath()
        {
            var gamePath = GameLocation.InitializeValidGamePath();
            if (!Directory.Exists(gamePath))
                return null;
            if (string.IsNullOrEmpty(gamePath))
                return null;
            return gamePath + @"\Windose_Data";
        }

        public static System.Drawing.Image GetCachedBackground(string bgName)
        {
            SixLabors.ImageSharp.Image image = null;
            if (!SaveToMemory)
            {
                try
                {
                    var file = File.ReadAllBytes(BackgroundDirectory + @"\" + bgName);
                    image = TextureConverter(file, 348, 227);
                }
                catch
                {
                    if (CustomAssetExtractor.customAssets.Exists(a => a.fileName == bgName && a.customAssetType == CustomAssetType.Background))
                    {
                        var customFile = CustomAssetExtractor.customAssets.Find(a => a.fileName == bgName);
                        return System.Drawing.Image.FromFile(customFile.filePath);
                    }
                    else return null;
                }

            }
            if (SaveToMemory && CachedBackgrounds.Count == 0) return null;
            if (SaveToMemory && !CachedBackgrounds.TryGetValue(bgName, out image))
                return null;
            if (image == null)
                return null;
            using (MemoryStream ms = new MemoryStream())
            {
                image.SaveAsPng(ms);
                return System.Drawing.Image.FromStream(ms);

            }
        }

        public static System.Drawing.Image GetCachedSprite(string bgName)
        {
            SixLabors.ImageSharp.Image image = null;
            if (!SaveToMemory)
            {
                try
                {
                    var file = File.ReadAllBytes(SpriteDirectory + @"\" + bgName);
                    image = TextureConverter(file, 348, 227);
                }
                catch
                {
                    if (CustomAssetExtractor.customAssets.Exists(a => a.fileName == bgName && a.customAssetType == CustomAssetType.Sprite))
                    {
                        var customPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
                        var customFile = CustomAssetExtractor.customAssets.Find(a => a.fileName == bgName);
                        var customPathToBundle = Path.Combine(customPath, Path.GetFileNameWithoutExtension(customFile.filePath));
                        var customPathToPic = Path.Combine(customPathToBundle, customFile.fileName);
                        if (!File.Exists(customPathToPic))
                            return null;
                        byte[] bytes = File.ReadAllBytes(customPathToPic);
                        image = TextureConverter(bytes, customFile.picWidth, customFile.picHeight, false);
                    }
                    else return null;
                }

            }
            if (SaveToMemory && CachedSprites.Count == 0) return null;
            if (SaveToMemory && !CachedSprites.TryGetValue(bgName, out image))
                return null;
            if (image == null)
                return null;
            using (MemoryStream ms = new MemoryStream())
            {
                image.SaveAsPng(ms);
                return System.Drawing.Image.FromStream(ms);
            }
        }
        public static WaveOut GetCachedAudio(string audioName)
        {
            string filePath = "";
            if (audioName.Contains("BGM_"))
            {
                audioName = audioName.Remove(0, 4);
            }
            else if (audioName.Contains("SE_"))
                audioName = audioName.Remove(0, 3);
            if (!SaveToMemory)
                filePath = GetProperFile();
            if (SaveToMemory && CachedMusic.Count == 0) return null;
            if (SaveToMemory && !CachedMusic.TryGetValue(audioName, out var _))
                return null;
            WaveFormat waveFormat = new();
            byte[] musicData;
            if (SaveToMemory)
                musicData = CachedMusic[audioName].Item1;
            else musicData = File.ReadAllBytes(filePath);
            if (musicData == null || musicData.Length == 0)
                return null;
            int compressionFormat;
            if (SaveToMemory)
                compressionFormat = CachedMusic[audioName].Item2;
            else
            {
                var fileName = Path.GetFileName(filePath);
                compressionFormat = int.Parse(fileName.Split('-')[1]);
            }
            byte[] vorbData = null;
            Console.WriteLine(CachedMusic);
            FmodSoundBank help;
            if (compressionFormat == 1)
            {
                help = FsbLoader.LoadFsbFromByteArray(musicData);
                help.Samples[0].RebuildAsStandardFileFormat(out byte[] itWorks, out string ex);
                vorbData = itWorks;
            }
            Console.WriteLine(audioName);

            using (RawSourceWaveStream ws = new RawSourceWaveStream(vorbData != null ? vorbData : musicData, 0, vorbData != null ? vorbData.Length : musicData.Length, waveFormat))
            {
                WaveStream test;
                if (compressionFormat == 0)
                    test = WaveFormatConversionStream.CreatePcmStream(ws);
                else
                {
                    MemoryStream ms = new MemoryStream(vorbData);
                    test = new VorbisWaveReader(ms);
                }
                var output = new WaveOut();
                output.Volume = 0.2f;
                output.Init(test);
                return output;
            }

            string GetProperFile()
            {
                foreach (var file in Directory.GetFiles(AudioDirectory))
                {
                    if (file.Contains(audioName))
                        return file;
                }
                return null;
            }
        }

        public static void CacheAudio()
        {
            List<string> s = Enum.GetNames(typeof(SoundType)).ToList();
            byte[] resourceData = null;
            var filePath = "";
            var gameDataPath = GetGameDataPath();
            if (string.IsNullOrEmpty(gameDataPath))
                return;
            foreach (var file in Directory.GetFiles(gameDataPath))
            {
                if (file.EndsWith("sharedassets0.assets"))
                {
                    filePath = file;
                    continue;
                }
                if (file.EndsWith("sharedassets0.resource"))
                {
                    resourceData = File.ReadAllBytes(file);
                    break;
                }
            }
            var am = new AssetsManager();

            var assInst = am.LoadAssetsFile(filePath, true);
            using (Stream stream = new MemoryStream(Properties.Resources.lz4))
            {
                am.LoadClassPackage(stream);
                am.LoadClassDatabaseFromPackage(assInst.file.Metadata.UnityVersion);
                foreach (var info in assInst.file.GetAssetsOfType(AssetClassID.AudioClip))
                {
                    var audio = am.GetBaseField(assInst, info);
                    if (s.Contains("BGM_" + audio["m_Name"].AsString) || s.Contains("SE_" + audio["m_Name"].AsString))
                    {
                        var offset = audio["m_Resource"].Get("m_Offset").AsInt;
                        var size = audio["m_Resource"].Get("m_Size").AsInt;
                        var compressionFormat = audio["m_CompressionFormat"].AsInt;
                        byte[] audioData = ReadResourcesFile(offset, size);
                        if (SaveToMemory)
                        {
                            try
                            {
                                CachedMusic.Add(audio["m_Name"].AsString, new(audioData, compressionFormat));
                            }
                            catch (ArgumentException) { continue; }
                        }
                        else
                        {
                            try
                            {
                                using (var file = File.Create(AudioDirectory + @"\" + audio["m_Name"].AsString + $"-{compressionFormat}"))
                                {
                                    file.Write(audioData, 0, audioData.Length);
                                    file.Dispose();
                                }
                            }
                            catch (ArgumentException) { continue; }
                        }

                    }
                }
            }

            byte[] ReadResourcesFile(int offset, int size)
            {
                List<byte> b = new();
                int limit = 0;
                for (int i = offset; limit < size; i++)
                {
                    limit++;
                    b.Add(resourceData[i]);
                }
                return b.ToArray();
            }

        }
        public static void CacheBackgrounds()
        {
            List<string> s = new() {
                    "bg_stream",
                    "bg_stream_shield_silver",
                    "bg_stream_shield_gold",
                    "bg_stream_angel_lv1",
                    "bg_stream_angel_lv2",
                    "bg_stream_angel_lv3",
                    "bg_stream_angel_lv4",
                    "bg_stream_angel_lv5",
                    "bg_stream_kyouso",
                    "bg_stream_horror",
                    "bg_stream_mansion",
                    "bg_stream_sayonara_002"
                };
            var filePath = "";
            var addressablesPath = GetAddressablesPath();
            if (string.IsNullOrEmpty(addressablesPath))
                return;
            foreach (var file in Directory.GetFiles(addressablesPath))
            {
                if (file.Contains("defaultlocalgroup"))
                {
                    filePath = file;
                    break;
                }
            }
            if (string.IsNullOrEmpty(filePath))
                return;

            var am = new AssetsManager();

            var bunInst = am.LoadBundleFile(filePath, false);
            var assetInst = am.LoadAssetsFileFromBundle(bunInst, 0, true);

            TextureFile tf;
            foreach (var info in assetInst.file.GetAssetsOfType((int)AssetClassID.Texture2D))
            {
                var pic = am.GetBaseField(assetInst, info);
                var picName = pic["m_Name"].AsString;
                if (s.Contains(picName))
                {
                    if (pic["m_Name"].AsString.Contains("sayonara"))
                        picName = "bg_stream_sayonara";
                    tf = TextureFile.ReadTextureFile(pic);
                    var texDat = tf.GetTextureData(assetInst);
                    if (SaveToMemory == true)
                    {
                        CachedBackgrounds.Add(picName, TextureConverter(texDat, tf));
                    }
                    else
                    {
                        using (var file = File.Create(BackgroundDirectory + @"\" + picName))
                        {
                            file.Write(texDat, 0, texDat.Length);
                            file.Dispose();
                        }
                    }
                }
            }
        }

        private static SixLabors.ImageSharp.Image TextureConverter(byte[] texDat, TextureFile tf)
        {
            return TextureConverter(texDat, tf.m_Width, tf.m_Height);
        }
        private static SixLabors.ImageSharp.Image TextureConverter(byte[] texDat)
        {
            if (texDat != null && texDat.Length > 0)
            {
                SixLabors.ImageSharp.Image test;
                test = SixLabors.ImageSharp.Image.Load<Bgra32>(texDat);
                test.Mutate(i => i.Flip(FlipMode.Vertical));
                return test;
            }
            return null;
        }
        private static SixLabors.ImageSharp.Image TextureConverter(byte[] texDat, int width, int height, bool autoDoubleHeight = true)
        {
            if (texDat != null && texDat.Length > 0)
            {
                if (width == 0 || height == 0)
                    return TextureConverter(texDat);
                if (texDat.Length > 1000000 && autoDoubleHeight)
                {
                    width *= 2;
                    height *= 2;
                }
                SixLabors.ImageSharp.Image test;
                test = SixLabors.ImageSharp.Image.LoadPixelData<Bgra32>(texDat, width, height);
                test.Mutate(i => i.Flip(FlipMode.Vertical));
                return test;
            }
            return null;
        }

        public static void CacheSprites()
        {
            int spriteIndex = 0;
            var addressablesPath = GetAddressablesPath();
            if (string.IsNullOrEmpty(addressablesPath))
                return;
            string everyFileName = string.Join(",", Directory.GetFiles(addressablesPath));
            string filePath;
            while (spriteIndex < ThatOneLongListOfAnimationsOriginallyInTheGame.list.Length)
            {
                if (!everyFileName.Contains(ThatOneLongListOfAnimationsOriginallyInTheGame.list[spriteIndex] + ".anim"))
                {
                    spriteIndex++;
                    continue;
                }
                foreach (var file in Directory.GetFiles(addressablesPath))
                {
                    if (spriteIndex >= ThatOneLongListOfAnimationsOriginallyInTheGame.list.Length)
                        break;
                    if (file.Contains(ThatOneLongListOfAnimationsOriginallyInTheGame.list[spriteIndex] + ".anim"))
                    {
                        filePath = file;
                        CachedSprites.Add(ThatOneLongListOfAnimationsOriginallyInTheGame.list[spriteIndex], SpriteGetter());
                        spriteIndex++;
                        continue;
                    }

                }
            }

            SixLabors.ImageSharp.Image SpriteGetter()
            {
                var am = new AssetsManager();

                var bunInst = am.LoadBundleFile(filePath, false);
                var assetInst = am.LoadAssetsFileFromBundle(bunInst, 0, true);

                var hmm = assetInst.file.GetAssetsOfType((int)AssetClassID.Texture2D);
                var abInfo = hmm.Count > 1 ? hmm[hmm.Count - 2] : hmm[0];

                var why = am.GetBaseField(assetInst, abInfo);

                var tf = TextureFile.ReadTextureFile(why);

                var texDat = tf.GetTextureData(assetInst);
                if (SaveToMemory)
                    return TextureConverter(texDat, tf);
                else
                {
                    using (var file = File.Create(SpriteDirectory + @"\" + ThatOneLongListOfAnimationsOriginallyInTheGame.list[spriteIndex]))
                    {
                        file.Write(texDat, 0, texDat.Length);
                    }
                    return null;
                }
            }

        }
    }
}
